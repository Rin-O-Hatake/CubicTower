using System;
using System.Collections.Generic;
using Core.Scripts.Cubes;
using Core.Scripts.Data;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts.UI
{
    public class CubicTowerView : MonoBehaviour
    {
        #region Field

        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private CubesConfig _cubesConfig; 
        
        public CompositeDisposable _disposables = new CompositeDisposable();
        #region Properties

        public Transform ContentCubes => _scrollRect.content;

        #endregion

        #endregion
        
        public List<Cube> CreateAllCubes(Action<CubicDropData> isDrop)
        {
            List<Cube> cubes = new List<Cube>();

            for (int index = 0; index < _cubesConfig.Cubes.Length; index++)
            {
                Cube newCube = Instantiate(_cubesConfig.CubePrefab, ContentCubes.transform);

                _cubesConfig.Cubes[index].SetId(index);
                newCube.Initialize(_cubesConfig.Cubes[index]);
                cubes.Add(newCube);
                
                newCube.IsDragging
                    .Subscribe(SetEnableScroll)
                    .AddTo(_disposables);
                
                newCube.IsDrop
                    .Subscribe(isDrop)
                    .AddTo(_disposables);   
            }
            
            return cubes;
        }

        private void SetEnableScroll(bool enable)
        {
            _scrollRect.enabled = enable;
        }

        #region MonoBehavior

        public void OnDestroy()
        {
            _disposables.Dispose();
        }

        #endregion
    }
}
