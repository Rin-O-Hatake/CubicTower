using Core.Scripts.Data;
using DG.Tweening;
using UnityEngine;

namespace Core.Scripts.Cubes
{
    public class SceneCubic : MonoBehaviour
    {
        #region Field

        [SerializeField, Range(0.1f, 5f)] private float _duration;
        [SerializeField] private SpriteRenderer _cubicSprite;
        [SerializeField] private LayerMask _layerMask;
        
        private int _currentId;
        
        private bool _isDragging;
        private Vector3 _offset;
        private Vector3 _originalPosition;
        private bool _isInBasket;

        #endregion

        public void Init(CubeData cubeData)
        {
            _currentId = cubeData.Id;
            _cubicSprite.color = cubeData.Color;
        }
        
        public void ShowCubic(float endPositionY)
        {
            transform.DOMoveY(endPositionY, _duration);
        }

        #region MonoBehavior

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = MainData.SceneData.CameraScene.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, int.MaxValue, _layerMask);
            
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("3");
                    _isDragging = true;
                    // _offset = transform.position - hit.point;
                }
                
                Debug.Log("1");
                // Ray ray = MainData.SceneData.CameraScene.ScreenPointToRay(Input.mousePosition);
                // RaycastHit hit;
                //
                // if (Physics.Raycast(ray, out hit, int.MaxValue, _layerMask))
                // {
                //     Debug.Log("2");
                //     if (hit.collider.gameObject == gameObject)
                //     {
                //         Debug.Log("3");
                //         _isDragging = true;
                //         _offset = transform.position - hit.point;
                //     }
                // }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }

            if (_isDragging)
            {
                Vector3 mouseWorldPosition = GetMouseWorldPosition();
                transform.position = mouseWorldPosition + _offset;
            }
        }

        #endregion

        #region Drag Cube

        // void OnMouseDown()
        // {
        //     Vector3 mouseWorldPosition = GetMouseWorldPosition();
        //     _offset = transform.position - mouseWorldPosition;
        //     _isDragging = true;
        //     
        //     _originalPosition = transform.position;
        // }
        //
        // void OnMouseUp()
        // {
        //     Debug.Log("1");
        //     _isDragging = false;
        //
        //     if (!_isInBasket)
        //     {
        //         transform.position = _originalPosition; 
        //     }
        // }

        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
            return Camera.main.ScreenToWorldPoint(mousePosition);
        }

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
    }
}
