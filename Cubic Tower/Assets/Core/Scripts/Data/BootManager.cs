using UnityEngine;

namespace Core.Scripts.Data
{
    public class BootManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Canvas canvas;
        [SerializeField] private Camera mainCamera;
        
        #endregion

        private void Awake()
        {
            MainData.SceneData = new SceneData(canvas, mainCamera);
        }
    }
}