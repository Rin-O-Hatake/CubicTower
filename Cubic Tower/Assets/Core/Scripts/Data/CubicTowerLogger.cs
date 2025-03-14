using TMPro;
using UnityEngine;

namespace Core.Scripts.Data
{
    public class CubicTowerLogger : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_Text _loggerText;

        #region Const Logs

        public const string CUBIC_INSTALL = "The cube is installed";
        public const string CUBIC_REMOVE = "The cube is thrown out";
        public const string CUBIC_DESTROY = "The cube is destroyed";
        public const string MAXIMUM_HEIGHT = "You can not put new cubes, the maximum height";

        #endregion

        #endregion

        public void ShowText(string message)
        {
            _loggerText.text = message;
        }
    }
}