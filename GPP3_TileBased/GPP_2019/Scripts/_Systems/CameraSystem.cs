using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace GPP_2019
{
    class CameraSystem : ISystem
    {
        /*
        const int SCREEN_WIDTH = 816;
        const int SCREEN_HEIGHT = 624;
        */
        private CameraFollowComponent cameraComponent;
        public static SDL_Rect cameraRect;
        private bool init = false;

        //private List<CameraFollowComponent> _activeCameras = new List<CameraFollowComponent>();

        private CameraSystem()
        {
            cameraRect.w = Program.SCREEN_WIDTH;
            cameraRect.h = Program.SCREEN_HEIGHT;
        }
        private static CameraSystem instance = null;
        public static CameraSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CameraSystem();
                }
                return instance;
            }
        }
        public SDL_Rect getCamera()
        {
            return cameraRect;
        }

        public void Update()
        {
            /*
            foreach (var camera in _activeCameras)
            {
                Scroll(camera);
            }
            */
            Scroll(cameraComponent);
            
        }

        private void Scroll(CameraFollowComponent camera)
        {
            cameraRect.x = (int)((camera.GameObject.Transform.Position.X + (camera.GameObject.Transform.Size.Width / 2)) - Program.SCREEN_WIDTH / 2);
            cameraRect.y = (int)((camera.GameObject.Transform.Position.Y + (camera.GameObject.Transform.Size.Height / 2)) - Program.SCREEN_HEIGHT / 2);

            if (cameraRect.x + Program.SCREEN_WIDTH + LevelManager.SCREEN_OFFSET_X >= LevelManager.LEVEL_WIDTH)
                cameraRect.x = LevelManager.LEVEL_WIDTH - Program.SCREEN_WIDTH - LevelManager.SCREEN_OFFSET_X;

            if (cameraRect.x - Program.SCREEN_WIDTH - LevelManager.SCREEN_OFFSET_X < -LevelManager.LEVEL_WIDTH)
                cameraRect.x = -LevelManager.LEVEL_WIDTH + Program.SCREEN_WIDTH + LevelManager.SCREEN_OFFSET_X;

            if (cameraRect.y + Program.SCREEN_HEIGHT + LevelManager.SCREEN_OFFSET_Y >= LevelManager.LEVEL_HEIGHT)
                cameraRect.y = LevelManager.LEVEL_HEIGHT - Program.SCREEN_HEIGHT - LevelManager.SCREEN_OFFSET_Y;

            if (cameraRect.y - Program.SCREEN_HEIGHT - LevelManager.SCREEN_OFFSET_Y < -LevelManager.LEVEL_HEIGHT)
                cameraRect.y = -LevelManager.LEVEL_HEIGHT + Program.SCREEN_HEIGHT + LevelManager.SCREEN_OFFSET_Y;

            

            //Console.WriteLine("Player Coordinates: " + camera.GameObject.Transform.Position);

            EventSystem.Instance.RaiseEvent("Cameraupdate", cameraRect);
        }

        private void ScrollBox(CameraFollowComponent camera)
        {
            int offsetX = (int)((camera.GameObject.Transform.Position.X + (camera.GameObject.Transform.Size.Width / 2)) - Program.SCREEN_WIDTH / 2);
            int offsetY = (int)((camera.GameObject.Transform.Position.Y + (camera.GameObject.Transform.Size.Height / 2)) - Program.SCREEN_HEIGHT / 2);

            if (!init)
            {
                cameraRect.x = (int)((camera.GameObject.Transform.Position.X + (camera.GameObject.Transform.Size.Width / 2)) - Program.SCREEN_WIDTH / 2);
                cameraRect.y = (int)((camera.GameObject.Transform.Position.Y + (camera.GameObject.Transform.Size.Height / 2)) - Program.SCREEN_HEIGHT / 2);
                init = true;
            }

            if (offsetX >= 100)
                cameraRect.x = (int)((camera.GameObject.Transform.Position.X + (camera.GameObject.Transform.Size.Width / 2)) - Program.SCREEN_WIDTH / 2) - 100;
            if (offsetX <= -100)
                cameraRect.x = (int)((camera.GameObject.Transform.Position.X + (camera.GameObject.Transform.Size.Width / 2)) - Program.SCREEN_WIDTH / 2) + 100;
            if (offsetY >= 100)
                cameraRect.y = (int)((camera.GameObject.Transform.Position.Y + (camera.GameObject.Transform.Size.Height / 2)) - Program.SCREEN_HEIGHT / 2) - 100;
            if (offsetY <= -100)
                cameraRect.y = (int)((camera.GameObject.Transform.Position.Y + (camera.GameObject.Transform.Size.Height / 2)) - Program.SCREEN_HEIGHT / 2) + 100;
            
            
            if (cameraRect.x + Program.SCREEN_WIDTH + LevelManager.SCREEN_OFFSET_X >= LevelManager.LEVEL_WIDTH)
                cameraRect.x = LevelManager.LEVEL_WIDTH - Program.SCREEN_WIDTH - LevelManager.SCREEN_OFFSET_X;

            if (cameraRect.x - Program.SCREEN_WIDTH - LevelManager.SCREEN_OFFSET_X < -LevelManager.LEVEL_WIDTH)
                cameraRect.x = -LevelManager.LEVEL_WIDTH + Program.SCREEN_WIDTH + LevelManager.SCREEN_OFFSET_X;

            if (cameraRect.y + Program.SCREEN_HEIGHT + LevelManager.SCREEN_OFFSET_Y >= LevelManager.LEVEL_HEIGHT)
                cameraRect.y = LevelManager.LEVEL_HEIGHT - Program.SCREEN_HEIGHT - LevelManager.SCREEN_OFFSET_Y;

            if (cameraRect.y - Program.SCREEN_HEIGHT - LevelManager.SCREEN_OFFSET_Y < -LevelManager.LEVEL_HEIGHT)
                cameraRect.y = -LevelManager.LEVEL_HEIGHT + Program.SCREEN_HEIGHT + LevelManager.SCREEN_OFFSET_Y;

            EventSystem.Instance.RaiseEvent("Cameraupdate", cameraRect);
        }

        public IEntityComponent CreateCamera()
        {
            /*
            if(_activeCameras.Count > 0)
            {
                Program.Error("Handling more than 1 Camera is not implemtented yet!");
                return null;
            }
            CameraFollowComponent camera = new CameraFollowComponent(this);
            _activeCameras.Add(camera);
            */
            CameraFollowComponent camera = new CameraFollowComponent(this);
            cameraComponent = camera;

            return camera;
        }
    }
}
