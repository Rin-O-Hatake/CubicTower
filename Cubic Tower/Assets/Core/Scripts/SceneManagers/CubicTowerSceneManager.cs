using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Cubes;
using Core.Scripts.Data;
using Core.Scripts.UI;
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
        
        private CubicTowerViewModel _currentView;
        
        private List<SceneCubic> _currentSceneCubes = new List<SceneCubic>();
        private CompositeDisposable _disposable = new CompositeDisposable();

        private const int FIRST_ELEMENT = 0;
        
        #endregion

        [Inject]
        public void Construct(CubicTowerViewModel cubicViewModel)
        {
            _currentView = cubicViewModel;
            _currentView.InitCubicTowerView(CreateCubic);
        }

        #region Create Cube

        private void CreateCubic(CubicDropData cubicDropData)
        {
            if (!CheckCubePosition(cubicDropData.Position))
            {
                return;
            }

            SceneCubic newSceneCubic = Instantiate(_sceneCubic, _content.transform);
            
            bool isFirstCubic = _currentSceneCubes.Count == 0;
            
            SetPosition();
            InitNewSceneCubic();
            
            _currentSceneCubes.Add(newSceneCubic);
            
            return;

            void InitNewSceneCubic()
            {
                newSceneCubic.Init(cubicDropData.CubeData);
                newSceneCubic.ShowCubic();
                
                newSceneCubic.RemoveSceneCube
                    .Subscribe(RemoveSceneCube)
                    .AddTo(_disposable);
            }

            void SetPosition()
            {
                Vector2 newPosition = new Vector2(isFirstCubic ? cubicDropData.Position.x : GenerateRandomPositionXCubic(cubicDropData.Position.x),
                    isFirstCubic ? _endPositionCube.transform.position.y :
                        _currentSceneCubes.Last().SpriteRenderCube.GetComponent<Renderer>().bounds.size.y + _currentSceneCubes.Last().transform.position.y);
                newSceneCubic.transform.position = newPosition;
            }
        }

        private bool CheckCubePosition(Vector2 position)
        {
            if (_currentSceneCubes.Count > 0)
            {
                Vector3 screenPosition = MainData.SceneData.CameraScene.WorldToScreenPoint(transform.position);
                float screenHeight = Screen.height;

                if (screenPosition.y > screenHeight)
                {
                    MainData.SceneData.Logger.ShowText(MainData.SceneData.Logger.MAXIMUM_HEIGHT);
                    return false;
                }
                
                float halfWidth = GetHalfWidth();
                float positionX = GetLastCubePosition();

                if (positionX + halfWidth < position.x || positionX - halfWidth > position.x)
                {
                    return false;
                }
            }

            return true;
        }

        private float GenerateRandomPositionXCubic(float positionX)
        {
            float halfWidth = GetHalfWidth();
            float lastCubePositionX = GetLastCubePosition();

            float maxRandomPositionX;
            float minRandomPositionX;
            
            if (positionX > lastCubePositionX)
            {
                maxRandomPositionX = lastCubePositionX + halfWidth - positionX;
                minRandomPositionX = halfWidth - maxRandomPositionX;
            }
            else
            {
                minRandomPositionX = lastCubePositionX - halfWidth + Math.Abs(positionX);
                maxRandomPositionX = halfWidth - minRandomPositionX;
            }
            
            float randomWidth =  Random.Range(minRandomPositionX, maxRandomPositionX);
            
            return positionX + randomWidth; 
        }

        private float GetLastCubePosition()
        {
            return _currentSceneCubes.LastOrDefault()!.transform.position.x;
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
                _currentSceneCubes[i].FallCubic(_currentSceneCubes[i].transform.position.y - hight);
            }
        }

        private bool CheckDestroyUpCubes(Vector2 upperCubic, Vector2 lowerCubic)
        {
            float halfWidth = GetHalfWidth();

            return lowerCubic.x + halfWidth < upperCubic.x - halfWidth ||
                    lowerCubic.x - halfWidth > upperCubic.x + halfWidth;

        }

        #endregion

        private float GetHalfWidth()
        {
            return _sceneCubic.SpriteRenderCube.GetComponent<Renderer>().bounds.size.x / 2;
        }
    }
}
