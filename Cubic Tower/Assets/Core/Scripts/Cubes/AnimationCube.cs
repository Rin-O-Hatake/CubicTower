using System;
using UnityEngine;

namespace Core.Scripts.Cubes
{
    [Serializable]
    public class AnimationCube
    {
        #region Fields

        [SerializeField] private Animator _animator;
        [SerializeField] private int _durationExplosionMilliseconds;
        
        [SerializeField] private GameObject _cubeVisual;

        #region Animation Names

        private const string EXPLOSION = "Explosion";

        #endregion

        #region Properties
        
        public GameObject CubeVisual => _cubeVisual;

        #endregion
            
        #endregion

        public int ShowExplosion()
        {
            _animator.CrossFade(EXPLOSION, 0.2f);

            return _durationExplosionMilliseconds;
        }

        public void EnableAnimationGameObject(bool enable = true)
        {
            _animator.gameObject.SetActive(enable);
            _cubeVisual.gameObject.SetActive(!enable);
        }
    }
}