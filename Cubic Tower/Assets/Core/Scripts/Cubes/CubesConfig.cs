using System;
using UnityEngine;

namespace Core.Scripts.Cubes
{
    [CreateAssetMenu(order = 51, fileName = "Cubes Config", menuName = "Cubes")]
    public class CubesConfig : ScriptableObject
    {
        #region Fields

        private Cube _cubePrefab;
        private CubeData[] _cubes;

        #region Properties

        public CubeData[] Cubes => _cubes;
        public Cube CubePrefab => _cubePrefab;

        #endregion
        
        #endregion
    }

    [Serializable]
    public class CubeData
    {
        #region Fields

        private Color _color;
        private int _id;

        #region Properties

        public Color Color => _color;
        public int Id => _id;

        #endregion

        #endregion
    }
}
