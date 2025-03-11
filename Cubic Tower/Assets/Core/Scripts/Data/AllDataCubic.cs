using Core.Scripts.Cubes;
using Vector2 = UnityEngine.Vector2;

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




