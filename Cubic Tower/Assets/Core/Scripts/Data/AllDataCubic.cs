using System;
using Core.Scripts.Cubes;
using Vector2 = UnityEngine.Vector2;

namespace Core.Scripts.Data
{
    public struct CubicDropData
    {
        public Vector2 Position;
        public CubeData CubeData;

        public CubicDropData(Vector2 position, CubeData cubeData)
        {
            Position = position;
            CubeData = cubeData;
        }
    }

    public enum LocalizationType
    {
        CUBIC_INSTALL,
        CUBIC_REMOVE,
        CUBIC_DESTROY,
        MAXIMUM_HEIGHT
    }

    public class CreateCubicData
    {
        public CubicDropData DropData;
        public Action<bool> ExplosionAction;
    }
}



