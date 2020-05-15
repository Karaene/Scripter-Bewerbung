using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace BrickBreakerConsoleApp
{
    class CameraFollowComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }
        public Type Type { get; set; }

        public CameraFollowComponent(CameraSystem cameraSystem)
        {
            System = cameraSystem;
            Type = Type.CAMERA;
        }
    }
}
