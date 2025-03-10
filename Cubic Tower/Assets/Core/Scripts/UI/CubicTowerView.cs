using System.Collections.Generic;
using Core.Scripts.Cubes;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts.UI
{
    public class CubicTowerView : MonoBehaviour
    {
        #region Field

        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private CubesConfig _cubesConfig; 

        #region Properties

        public Transform ContentCubes => _scrollRect.content;

        #endregion

        #endregion
        
        public List<Cube> CreateAllCubes()
        {
            List<Cube> cubes = new List<Cube>();
            
            foreach (var cube in _cubesConfig.Cubes)
            {
                Cube newCube = Instantiate(_cubesConfig.CubePrefab, ContentCubes.transform);
                newCube.Initialize(cube);
                cubes.Add(newCube);
            }
            
            return cubes;
        }
    }
}
