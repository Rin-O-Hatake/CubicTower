using UnityEngine;

namespace Core.Scripts.Data
{
    public struct SceneData
    {
        public Canvas CanvasScene;
        public Camera CameraScene;
        public CubicTowerLogger Logger;

        public SceneData(Canvas canvas, Camera camera, CubicTowerLogger logger)
        {
            CanvasScene = canvas;
            CameraScene = camera;
            Logger = logger;
        }
    }
}