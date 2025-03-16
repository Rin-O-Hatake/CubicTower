using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts.Cubes
{
    [CreateAssetMenu(order = 51, fileName = "Cubes Config", menuName = "Cubes")]
    public class CubesConfig : ScriptableObject
    {
        #region Fields

        [SerializeField] private Cube _cubePrefab;
        [SerializeField] private List<CubeData> _cubes;

        #region Properties

        public List<CubeData> Cubes => _cubes;
        public Cube CubePrefab => _cubePrefab; 

        #endregion
        
        #endregion
    }

    [Serializable]
    public class CubeData
    {
        #region Fields

        [SerializeField] private Color _color;
        private int _id;

        #region Properties

        public Color Color => _color;
        public int Id => _id;

        #endregion

        #endregion

        public void SetId(int id)
        {
            _id = id;
        }

        public void SetColor(Color color)
        {
            _color = color;
        }
    }
}
