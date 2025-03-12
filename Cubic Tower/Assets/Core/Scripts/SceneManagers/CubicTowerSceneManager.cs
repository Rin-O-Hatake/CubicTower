using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Cubes;
using Core.Scripts.UI;
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
        [SerializeField] private GameObject _startPositionCube;
        [SerializeField] private GameObject _endPositionCube;
        
        private CubicTowerViewModel _currentView;
        private List<SceneCubic> _currentSceneCubes = new List<SceneCubic>();
        
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
                newSceneCubic.ShowCubic(isFirstCubic ? _endPositionCube.transform.position.y :
                    _currentSceneCubes.Last().GetComponent<Renderer>().bounds.size.y + _currentSceneCubes.Last().transform.position.y);
    
            }

            void SetPosition()
            {
                Vector2 newPosition = new Vector2(isFirstCubic ? cubicDropData.Position.x : GenerateRandomPositionXCubic(cubicDropData.Position.x),
                    _startPositionCube.transform.position.y);
                newSceneCubic.transform.position = newPosition;
            }
        }

        private bool CheckCubePosition(Vector2 position)
        {
            if (_currentSceneCubes.Count > 0)
            {
                float halfWidth = _sceneCubic.GetComponent<Renderer>().bounds.size.x / 2;
                float positionX = _currentSceneCubes.LastOrDefault()!.transform.position.x;

                if (positionX + halfWidth < position.x || positionX - halfWidth > position.x)
                {
                    return false;
                }
            }

            return true;
        }

        private float GenerateRandomPositionXCubic(float positionX)
        {
            float halfWidth = _sceneCubic.GetComponent<Renderer>().bounds.size.x / 2;
            float lastCubePositionX = _currentSceneCubes.LastOrDefault()!.transform.position.x;

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
    
    
        #endregion
    }
}
