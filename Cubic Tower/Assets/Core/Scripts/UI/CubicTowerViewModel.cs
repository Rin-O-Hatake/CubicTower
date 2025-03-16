using System;
using Core.Scripts.Cubes;
using Core.Scripts.Data;
using Zenject;

namespace Core.Scripts.UI
{
    public class CubicTowerViewModel
    {
        #region Fields

        private CubicTowerView _currentView;
        
        private Cube[] _currentCubes;

        #region Properties

        public CubicTowerView CubicTowerView => _currentView;

        #endregion

        #endregion

        [Inject]
        public void Construct(CubicTowerView currentView)
        {
            _currentView = currentView;
        }

        public void InitCubicTowerView(Action<CreateCubicData> isDrop, Action saveData)
        {
            _currentCubes = _currentView.CreateAllCubes(isDrop).ToArray();
            _currentView.InitCubicTowerView(saveData);
        }
    }
}
