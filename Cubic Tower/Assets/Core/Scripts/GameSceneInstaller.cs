using Core.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Core.Scripts
{
    public class GameSceneInstaller : MonoInstaller
    {
        #region Feilds
        
        [SerializeField] private CubicTowerView _cubicTowerView;

        #endregion

        public override void InstallBindings()
        {
            InjectViewCubicTower();
            InjectViewModelCubicTower();
        }

        private void InjectViewModelCubicTower()
        {
            Container.Bind<CubicTowerViewModel>().AsSingle().NonLazy();
        }

        private void InjectViewCubicTower()
        {
            Container.Bind<CubicTowerView>().FromInstance(_cubicTowerView).AsSingle();
        }
    }
}
