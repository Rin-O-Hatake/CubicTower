using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Cubes;
using Core.Scripts.Data;
using Core.Scripts.Saves;
using Core.Scripts.UI;
using Cysharp.Threading.Tasks;
using R3;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Core.Scripts.SceneManagers
{
    public class CubicTowerSceneManager : MonoBehaviour
    {
        #region Feild
        
        [Title("Cubic Data")]
        [SerializeField] private SceneCubic _sceneCubic;
        [SerializeField] private GameObject _content;
        
        [Space(10)]
        [SerializeField] private GameObject _endPositionCube;
        
        private CheckingCubes _checkingCubes = new CheckingCubes();
        private CubicTowerViewModel _currentViewModel;
        
        private List<SceneCubic> _currentSceneCubes = new List<SceneCubic>();
        private CompositeDisposable _disposable = new CompositeDisposable();

        private const int FIRST_ELEMENT = 0;
        
        #endregion

        [Inject]
        public void Construct(CubicTowerViewModel cubicViewModel)
        {
            _currentViewModel = cubicViewModel;
            _currentViewModel.InitCubicTowerView(CreateCubic, SaveCubes);
            _checkingCubes.Init(_currentSceneCubes, _sceneCubic);
        }

        #region MonoBehavior

        public void Start()
        {
            LoadCubes();
        }


        #endregion
        
        #region Create Cube

        private void CreateCubic(CubicDropData cubicDropData)
        {
            CreateCubic(cubicDropData,true);
        }

        private void LoadCubic(CubicDropData cubicDropData)
        {
            CreateCubic(cubicDropData, false);
        }

        private void CreateCubic(CubicDropData cubicDropData, bool positionDisplacement)
        {
            if (!_checkingCubes.CheckCubePosition(cubicDropData.Position))
            {
                return;
            }

            SceneCubic newSceneCubic = Instantiate(_sceneCubic, _content.transform);
            
            bool isFirstCubic = _currentSceneCubes.Count == 0;
            
            SetPosition();
            InitNewSceneCubic(newSceneCubic, cubicDropData);
            
            MainData.SceneData.Logger.TextSwitcher.SetLocalization(LocalizationType.CUBIC_INSTALL);
            _currentSceneCubes.Add(newSceneCubic);
            
            return;

            void SetPosition()
            {
                Vector2 newPosition = new Vector2(isFirstCubic || !positionDisplacement ? cubicDropData.Position.x : _checkingCubes.GenerateRandomPositionXCubic(cubicDropData.Position.x),
                    isFirstCubic ? _endPositionCube.transform.position.y :
                        _currentSceneCubes.Last().SpriteRenderCube.GetComponent<Renderer>().bounds.size.y + _currentSceneCubes.Last().transform.position.y);
                
                newSceneCubic.transform.position = newPosition;
            }
        }
        
        private void InitNewSceneCubic(SceneCubic newSceneCubic, CubicDropData cubicDropData)
        {
            newSceneCubic.Init(cubicDropData.CubeData);
            newSceneCubic.AnimationCube.ShowCubic(newSceneCubic.transform);
                
            newSceneCubic.RemoveSceneCube
                .Subscribe(RemoveSceneCube)
                .AddTo(_disposable);
        }
        
        
        #endregion

        #region Remove Cube

        private void RemoveSceneCube(SceneCubic cubic)
        {
            int index = _currentSceneCubes.FindIndex(cube => cube.Equals(cubic));
            _currentSceneCubes.Remove(cubic);

            if (_currentSceneCubes.Count == 0)
            {
                return;
            }

            if (_currentSceneCubes.Count >= 1 && index != FIRST_ELEMENT)
            {
                if (CheckDestroyUpCubes(_currentSceneCubes[index].transform.position, _currentSceneCubes[index - 1].transform.position))
                {
                    for (int i = index; i < _currentSceneCubes.Count; i++)
                    {
                        _currentSceneCubes[i].DestroyCubic();
                    }
                    
                    return;
                }
            }
            
            float hight = _sceneCubic.SpriteRenderCube.GetComponent<Renderer>().bounds.size.y;
            for (int i = index; i < _currentSceneCubes.Count; i++)
            {
                _currentSceneCubes[i].AnimationCube.FallCubic(_currentSceneCubes[i].transform, _currentSceneCubes[i].transform.position.y - hight);
            }
        }

        private bool CheckDestroyUpCubes(Vector2 upperCubic, Vector2 lowerCubic)
        {
            float halfWidth = _checkingCubes.GetHalfWidth();

            return lowerCubic.x + halfWidth < upperCubic.x - halfWidth ||
                    lowerCubic.x - halfWidth > upperCubic.x + halfWidth;

        }

        #endregion

        #region SaveCubes

        private void SaveCubes()
        {
            RootSavesData rootSavesData = new RootSavesData();
            foreach (var cube in _currentSceneCubes)
            {
                rootSavesData.Saves.Add(new SaveData
                {
                    Id = cube.Id,
                    PositionX = cube.transform.position.x,
                    PositionY = cube.transform.position.y
                });
            }

            SaveManager.SaveGame(rootSavesData).Forget();
        }

        private async void LoadCubes()
        {
            RootSavesData rootSavesData = await SaveManager.LoadGame();

            if (rootSavesData == null)
            {
                return;
            }
            
            foreach (SaveData saveCube in rootSavesData.Saves)
            {
                LoadCubic(new CubicDropData
                {
                    Position = new Vector2(saveCube.PositionX, saveCube.PositionY),
                    CubeData = _currentViewModel.CubicTowerView.CubesConfig.Cubes.Find(cube => cube.Id.Equals(saveCube.Id))
                });   
            }
        }

        #endregion
    }
}
