using Core.Scripts.UI.Localization;
using TMPro;
using UnityEngine;

namespace Core.Scripts.Data
{
    public class CubicTowerLogger : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_Text _loggerText;
        [SerializeField] private TextSwitcher _textSwitcher;

        #region propeties

        public TextSwitcher TextSwitcher => _textSwitcher;

        #endregion

        #endregion
        
        #region MonoBehavior

        public void Start()
        {
            _textSwitcher.Init(_loggerText);
        }

        #endregion
    }
}