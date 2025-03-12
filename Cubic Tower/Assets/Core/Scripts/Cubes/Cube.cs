using System;
using Core.Scripts.Data;
using R3;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Core.Scripts.Cubes
{
    public class Cube : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Fields

        [Title("Components")]
        [SerializeField] private CubeVisual _visual;
        
        [Title("Drag Properties")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private LayerMask _layerTriggerDrop;
        
        private Vector3 _currentOriginalPosition;
        private CubeData _currentCubeData;
        
        public readonly Subject<bool> IsDragging = new Subject<bool>();
        public readonly Subject<CubicDropData> IsDrop = new Subject<CubicDropData>();
        
        #endregion

        public void Initialize(CubeData cube)
        {
            _visual.SetColor(cube.Color);
            _currentCubeData = cube;
        }

        #region Drag

        public void OnBeginDrag(PointerEventData eventData)
        {
            IsDragging.OnNext(false);
            _currentOriginalPosition = _rectTransform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Canvas canvas = MainData.SceneData.CanvasScene;
            
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var globalMousePosition
            );
            
            _rectTransform.position = globalMousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            IsDragging.OnNext(true);
            
            if (!IsDroppedOnScene(eventData))
            {
                
            }
            
            _rectTransform.position = _currentOriginalPosition;
        }

        #region Scene Drop

        private bool IsDroppedOnScene(PointerEventData eventData)
        {
            Ray ray = MainData.SceneData.CameraScene.ScreenPointToRay(eventData.position);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, int.MaxValue, _layerTriggerDrop);
            
            if (hit.collider != null)
            {
                IsDrop.OnNext(new CubicDropData(hit.point, _currentCubeData));
                return true;
            }

            return false;
        }

        #endregion

        #endregion
    }
}
