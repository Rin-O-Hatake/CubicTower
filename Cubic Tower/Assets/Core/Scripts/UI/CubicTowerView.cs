using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts.UI
{
    public class CubicTowerView : MonoBehaviour
    {
        #region Field

        [SerializeField] private ScrollRect scrollRect;

        #region Properties

        public Transform ContentCubes => scrollRect.content;

        #endregion

        #endregion
    }
}
