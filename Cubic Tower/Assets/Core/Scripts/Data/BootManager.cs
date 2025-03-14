using UnityEngine;

namespace Core.Scripts.Data
{
    public class BootManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Canvas canvas;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CubicTowerLogger logger;

        #endregion

        private void Awake()
        {
            MainData.SceneData = new SceneData(canvas, mainCamera, logger);
        }
    }
}