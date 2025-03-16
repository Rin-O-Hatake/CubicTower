using System.IO;
using Cysharp.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Core.Scripts.Saves
{
    public static class SaveManager
    {
        private static string savePath;

        public static void CombineData()
        {
            savePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        }

        public static async UniTask SaveGame(RootSavesData saveData)
        {
            string json = JsonConvert.SerializeObject(saveData);
            await File.WriteAllTextAsync(savePath, json);
            Debug.Log("Game Saved: " + savePath);
        }

        public static async UniTask<RootSavesData> LoadGame()
        { 
            if (File.Exists(savePath))
            {
                string json = await File.ReadAllTextAsync(savePath);
                return JsonConvert.DeserializeObject<RootSavesData>(json);
            }
            
            return null;
        }
    }
}