using UnityEngine;

namespace Core.Scripts.Cubes
{
    public class Cube : MonoBehaviour
    {
        #region Fields

        [SerializeField] private CubeVisual _visual;

        private int _currentId;

        #endregion

        public void Initialize(CubeData cube)
        {
            _visual.SetColor(cube.Color);
            _currentId = cube.Id;
        }
    }
}
