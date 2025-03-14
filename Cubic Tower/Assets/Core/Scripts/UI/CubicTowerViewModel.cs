using System;
using Core.Scripts.Cubes;
using Core.Scripts.Data;
using UnityEngine;
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

        public void InitCubicTowerView(Action<CubicDropData> isDrop)
        {
            _currentCubes = _currentView.CreateAllCubes(isDrop).ToArray();
        }
    }
}
