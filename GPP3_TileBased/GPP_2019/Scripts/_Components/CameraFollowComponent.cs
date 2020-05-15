using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace GPP_2019
{
    class CameraFollowComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }

        public CameraFollowComponent(CameraSystem cameraSystem)
        {
            System = cameraSystem;
        }
    }
}
