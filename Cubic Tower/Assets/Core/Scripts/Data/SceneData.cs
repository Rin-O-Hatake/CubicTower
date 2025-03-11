using UnityEngine;

namespace Core.Scripts.Data
{
    public struct SceneData
    {
        public Canvas CanvasScene;
        public Camera CameraScene;

        public SceneData(Canvas canvas, Camera camera)
        {
            CanvasScene = canvas;
            CameraScene = camera;
        }
    }
}