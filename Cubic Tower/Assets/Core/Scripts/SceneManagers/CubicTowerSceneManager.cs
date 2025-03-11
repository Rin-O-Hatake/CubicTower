using Core.Scripts.Cubes;
using Core.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Core.Scripts.SceneManagers
{
    public class CubicTowerSceneManager : MonoBehaviour
    {
        #region Feild
        
        [SerializeField] private SceneCubic _sceneCubic;
        [SerializeField] private GameObject _content;

        private CubicTowerViewModel _currentView;
        
        #endregion

        [Inject]
        public void Construct(CubicTowerViewModel view)
        {
            _currentView = view;
            _currentView.InitCubicTowerView(CreateCubic);
        }

        private void CreateCubic(CubicDropData cubicDropData)
        {
            SceneCubic newSceneCubic = Instantiate(_sceneCubic, _content.transform);
            
        }
    }
}
