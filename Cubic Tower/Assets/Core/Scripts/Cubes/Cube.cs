using System.Threading.Tasks;
using Core.Scripts.Data;
using R3;
using Sirenix.OdinInspector;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Scripts.Cubes
{
    public class Cube : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Fields

        [Title("Components")]
        [SerializeField] private CubeVisual _visual;
        [SerializeField] private AnimationCube _animationCube;
        
        [Title("Drag Properties")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private LayerMask _layerTriggerDrop;
        
        private Vector3 _currentOriginalPosition;
        private CubeData _currentCubeData;
        
        public readonly Subject<bool> IsDragging = new Subject<bool>();
        public readonly Subject<CreateCubicData> IsDrop = new Subject<CreateCubicData>();
        
        #endregion

        public void Initialize(CubeData cube)
        {
            _visual.SetColor(cube.Color);
            _currentCubeData = cube;
        }

        #region Drag

        public void OnBeginDrag(PointerEventData eventData)
        {
            IsDragging?.OnNext(false);
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.9f;
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
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1;
            
            Explosion(eventData);
        }

        private void Explosion(PointerEventData eventData) 
        {
            if (!IsDroppedOnScene(eventData))
            {
                Explosion();
            }
        }

        private async void Explosion(bool isWriteLog = true)
        {
            _animationCube.EnableAnimationGameObject();
            int duration = _animationCube.ShowExplosion();

            if (isWriteLog)
            {
                MainData.SceneData.Logger.TextSwitcher.SetLocalization(LocalizationType.CUBIC_DESTROY);
            }
            
            await Task.Delay(duration);
            ResetPosition();
        }

        private void ResetPosition()
        {
            IsDragging?.OnNext(true);
            _rectTransform.position = _currentOriginalPosition;
            _animationCube.EnableAnimationGameObject(false);
        }

        #region Scene Drop

        private bool IsDroppedOnScene(PointerEventData eventData)
        {
            Ray ray = MainData.SceneData.CameraScene.ScreenPointToRay(eventData.position);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, int.MaxValue, _layerTriggerDrop);
            
            if (hit.collider != null)
            {
                IsDrop.OnNext(new CreateCubicData
                {
                    DropData = new CubicDropData(hit.point, _currentCubeData),
                    ExplosionAction = CheckDropAndExplosion
                });
                return true;
            }

            return false;
        }

        private void CheckDropAndExplosion(bool state)
        {
            if (state)
            {
                Explosion(false);
                return;
            }
            
            ResetPosition();
        }

        #endregion

        #endregion
    }
}
