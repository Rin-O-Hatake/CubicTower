using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts.Saves
{
    public class RootSavesData
    {
        public List<SaveData> Saves = new List<SaveData>();
    }
    public class SaveData
    {
        public int Id;
        public float PositionX;
        public float PositionY;
    }
}
