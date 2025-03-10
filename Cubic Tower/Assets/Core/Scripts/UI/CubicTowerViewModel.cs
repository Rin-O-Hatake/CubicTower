using Core.Scripts.Cubes;
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
            _currentView = currentView;
            InitCubicTowerView();
        }

        private void InitCubicTowerView()
        {
            _currentCubes = _currentView.CreateAllCubes().ToArray();
        }
    }
}
