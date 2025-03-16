using System;
using System.Threading.Tasks;
using Core.Scripts.Data;
using R3;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Scripts.Cubes
{
    public class SceneCubic : MonoBehaviour
    {
        #region Field
        
        [SerializeField] private SpriteRenderer _cubicSprite;
        [SerializeField] private AnimationCubeScene _animationCube;
        
        [Title("Layer Masks")]
        [SerializeField] private LayerMask _layerMaskCube;
        
        private int _currentId;
        
        private bool _isDragging;
        private Vector3 _offset;
        private Vector3 _originalPosition;
        private bool _isInBasket;

        public readonly Subject<SceneCubic> RemoveSceneCube = new Subject<SceneCubic>();

        #region Properties

        public GameObject SpriteRenderCube => _animationCube.CubeVisual;
        public AnimationCubeScene AnimationCube => _animationCube;
        public int Id => _currentId;

        #endregion
        
        #endregion

        public void Init(CubeData cubeData)
        {
            _currentId = cubeData.Id;
            _cubicSprite.color = cubeData.Color;
        }

        #region MonoBehavior

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GetHit(out var hit))
                {
                    return;
                }

                if (hit.collider.gameObject == gameObject)
                {
                    if (!_isDragging)
                    {
                        _originalPosition = transform.position;
                    }
                    
                    _isDragging = true;
                }
            }
            
            MouseUp();
            MoveCube();
        }
        
        #endregion

        #region Triggers

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<WasteBasket>(out WasteBasket basket))
            {
                _isInBasket = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<WasteBasket>(out WasteBasket basket))
            {
                _isInBasket = false;
            }
        }

        #endregion
        
        #region Drag Cube

        private void MoveCube()
        {
            if (_isDragging)
            {
                Vector3 mouseWorldPosition = GetMouseWorldPosition();
                transform.position = mouseWorldPosition + _offset;
            }
        }

        private void MouseUp()
        {
            if (!_isDragging)
            {
                return;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;

                if (_isInBasket)
                {
                    MainData.SceneData.Logger.TextSwitcher.SetLocalization(LocalizationType.CUBIC_REMOVE);
                    DestroyCubic();
                    return;
                } 
                
                transform.position = _originalPosition;
            }
        }

        public async void DestroyCubic()
        {
            try
            {
                _animationCube.EnableAnimationGameObject();
                int durationAnimation = _animationCube.ShowExplosion();
                RemoveSceneCube?.OnNext(this);
            
                await Task.Delay(durationAnimation);
            }
            catch (Exception e)
            {
                Debug.LogError($"Exeption: {e.Message}");
            }

            if (!gameObject)
            {
                return;
            }
            
            Destroy(gameObject);
        }

        private bool GetHit(out RaycastHit2D hit)
        {
            Ray ray = MainData.SceneData.CameraScene.ScreenPointToRay(Input.mousePosition);
            hit = Physics2D.GetRayIntersection(ray, int.MaxValue, _layerMaskCube);

            if (!hit.collider)
            {
                return true;
            }

            return false;
        }
        
        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            // ReSharper disable once PossibleNullReferenceException
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
            return Camera.main.ScreenToWorldPoint(mousePosition);
        }
        
        #endregion
    }
}
