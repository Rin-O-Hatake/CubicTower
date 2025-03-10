using Core.Scripts.Cubes;
using UnityEngine;
using Zenject;

namespace Core.Scripts.UI
{
    public class CubicTowerViewModel
    {
        #region Fields

        private CubicTowerView _currentView;
        
        private Cube[] _currentCubes;

        #endregion

        [Inject]
        public void Construct(CubicTowerView currentView)
        {
            Debug.Log('1');
            _currentView = currentView;
            InitCubicTowerView();
        }

        private void InitCubicTowerView()
        {
            _currentCubes = _currentView.CreateAllCubes().ToArray();
        }
    }
}
