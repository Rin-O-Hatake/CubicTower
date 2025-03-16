using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Cubes;
using Core.Scripts.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Scripts.SceneManagers
{
    [Serializable]
    public class CheckingCubes
    {
        #region Fields

        private List<SceneCubic> _currentSceneCubes = new List<SceneCubic>();
        private SceneCubic _sceneCubic;

        #endregion

        public void Init(List<SceneCubic> sceneCubes, SceneCubic sceneCubic)
        {
            _currentSceneCubes = sceneCubes;
            _sceneCubic = sceneCubic;
        }
        
        public float GenerateRandomPositionXCubic(float positionX)
        {
            float halfWidth = GetHalfWidth();
            float lastCubePositionX = GetLastCubePositionX();

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

        private float GetLastCubePositionX()
        {
            return _currentSceneCubes.LastOrDefault()!.transform.position.x;
        }
        
        public float GetHalfWidth()
        {
            return _sceneCubic.SpriteRenderCube.GetComponent<Renderer>().bounds.size.x / 2;
        }
        
        public bool CheckCubePosition(Vector2 position)
        {
            if (_currentSceneCubes.Count > 0)
            {
                SceneCubic lastCube = _currentSceneCubes.LastOrDefault(); 
                Vector3 positionLastCubic = new Vector3(lastCube.transform.position.x,
                    lastCube.transform.position.y + lastCube.SpriteRenderCube.GetComponent<Renderer>().bounds.size.y,
                    lastCube.transform.position.z);
                
                Vector3 screenPosition = MainData.SceneData.CameraScene.WorldToScreenPoint(positionLastCubic);
                float screenHeight = Screen.height;

                if (screenPosition.y > screenHeight)
                {
                    MainData.SceneData.Logger.TextSwitcher.SetLocalization(LocalizationType.MAXIMUM_HEIGHT);
                    return false;
                }
                
                float halfWidth = GetHalfWidth();
                float positionX = GetLastCubePositionX();

                if (positionX + halfWidth < position.x || positionX - halfWidth > position.x)
                {
                    return false;
                }
            }

            return true;
        }
    }
}