using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts.Cubes
{
    [Serializable]
    public class CubeVisual
    {
        #region Fields

        [SerializeField] private Image _image;
        
        #endregion

        public void SetColor(Color color)
        {
            _image.color = color;
        }
    }
}