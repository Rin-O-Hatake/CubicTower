using System;
using System.Collections.Generic;
using Core.Scripts.Cubes;
using Core.Scripts.Data;
using Core.Scripts.Saves;
using Core.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Scripts.SceneManagers
{
    [Serializable]
    public class SaveCubicDataManager
    {
        #region SaveCubes

        public void SaveCubes(List<SceneCubic> sceneCubes)
        {
            RootSavesData rootSavesData = new RootSavesData();
            foreach (var cube in sceneCubes)
            {
                rootSavesData.Saves.Add(new SaveData
                {
                    Id = cube.Id,
                    PositionX = cube.transform.position.x,
                    PositionY = cube.transform.position.y
                });
            }

            SaveManager.SaveGame(rootSavesData).Forget();
        }

        public async void LoadCubes(Action<CubicDropData> loadCubes, CubicTowerViewModel cubicTowerViewModel)
        {
            RootSavesData rootSavesData = await SaveManager.LoadGame();

            if (rootSavesData == null)
            {
                return;
            }
            
            foreach (SaveData saveCube in rootSavesData.Saves)
            {
                loadCubes?.Invoke(new CubicDropData
                {
                    Position = new Vector2(saveCube.PositionX, saveCube.PositionY),
                    CubeData = cubicTowerViewModel.CubicTowerView.CubesConfig.Cubes.Find(cube => cube.Id.Equals(saveCube.Id))
                });   
            }
        }

        #endregion

    }
}