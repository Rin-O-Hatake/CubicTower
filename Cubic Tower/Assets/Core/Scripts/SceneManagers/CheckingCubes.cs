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
            float halfWidth = GetHalfWidthCubic();
            float width = GetWidthCubic();
            float lastCubePositionX = GetLastCubePositionX();

            float maxRandomPositionX;
            float minRandomPositionX;
            
            if (positionX > lastCubePositionX)
            {
                if (lastCubePositionX + width - positionX > halfWidth)
                {
                    maxRandomPositionX = positionX + halfWidth;
                }
                else
                {
                    maxRandomPositionX = lastCubePositionX + width;
                }
                
                minRandomPositionX = positionX - halfWidth;
            }
            else
            {
                if (lastCubePositionX - width + Math.Abs(positionX) > halfWidth)
                {
                    minRandomPositionX = positionX - halfWidth;
                }
                else
                {
                    minRandomPositionX = lastCubePositionX - width;
                }
                
                maxRandomPositionX = positionX + width;
            }
            
            float randomWidth =  Random.Range(minRandomPositionX, maxRandomPositionX);
            
            return randomWidth; 
        }

        private float GetLastCubePositionX()
        {
            return _currentSceneCubes.LastOrDefault()!.transform.position.x;
        }
        
        public float GetHalfWidthCubic()
        {
            return _sceneCubic.SpriteRenderCube.GetComponent<Renderer>().bounds.size.x / 2;
        }

        public float GetWidthCubic()
        {
            return _sceneCubic.SpriteRenderCube.GetComponent<Renderer>().bounds.size.x;
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
                
                float width = GetWidthCubic();
                float positionX = GetLastCubePositionX();

                if (positionX + width < position.x || positionX - width > position.x)
                {
                    return false;
                }
            }

            return true;
        }
    }
}