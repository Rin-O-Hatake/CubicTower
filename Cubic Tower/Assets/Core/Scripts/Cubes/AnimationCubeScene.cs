using System;
using DG.Tweening;
using UnityEngine;

namespace Core.Scripts.Cubes
{
    [Serializable]
    public class AnimationCubeScene : AnimationCube
    {
        #region Field

        [SerializeField, Range(0.1f, 5f)] private float _durationJumpCubic;
        [SerializeField, Range(0.1f, 5f)] private float _durationFallCubic; 

        #endregion
        
        public void ShowCubic(Transform transform)
        {
            float heightJump = _cubeVisual.GetComponent<Renderer>().bounds.size.y / 4;
            DOTween.Sequence()
                .Append(transform.DOMoveY(transform.position.y + heightJump, _durationJumpCubic))
                .Append(transform.DOMoveY(transform.position.y, _durationJumpCubic));
        }

        public void FallCubic(Transform transform, float position)
        {
            transform.DOMoveY(position, _durationFallCubic);
        }
    }
}