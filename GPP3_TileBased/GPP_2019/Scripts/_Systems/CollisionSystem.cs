using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace GPP_2019
{
    class CollisionSystem : ISystem
    {
        //Singleton
        private CollisionSystem() { }
        private static CollisionSystem instance = null;
        public static CollisionSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CollisionSystem();
                }
                return instance;
            }
        }

        public bool Collision(GameObject box_1, GameObject box_2)
        {
            if (box_1.Transform.Position.X < box_2.Transform.Position.X + box_2.Transform.Size.Width &&
                box_1.Transform.Position.X + box_1.Transform.Size.Width > box_2.Transform.Position.X &&
                box_1.Transform.Position.Y < box_2.Transform.Position.Y + box_2.Transform.Size.Height &&
                box_1.Transform.Position.Y + box_1.Transform.Size.Height > box_2.Transform.Position.Y)
            {
                return true;
            }
            return false;
        }

        public bool Collision(GameObject box_1, SDL_Rect box_2)
        {
            if (box_1.Transform.Position.X < box_2.x + box_2.w &&
                box_1.Transform.Position.X + box_1.Transform.Size.Width > box_2.x &&
                box_1.Transform.Position.Y < box_2.y + box_2.h &&
                box_1.Transform.Position.Y + box_1.Transform.Size.Height > box_2.y)
            {
                return true;
            }
            return false;
        }

        public bool InsideCameraFrustrum(GameObject box_1)
        {
            if (box_1.Transform.Position.X < CameraSystem.cameraRect.x + Program.SCREEN_WIDTH + TileSystem.Instance.TileSize &&
                box_1.Transform.Position.X + box_1.Transform.Size.Width > CameraSystem.cameraRect.x &&
                box_1.Transform.Position.Y < CameraSystem.cameraRect.y + Program.SCREEN_HEIGHT + TileSystem.Instance.TileSize &&
                box_1.Transform.Position.Y + box_1.Transform.Size.Height > CameraSystem.cameraRect.y)
            {
                return true;
            }
            return false;
        }
    }
}
