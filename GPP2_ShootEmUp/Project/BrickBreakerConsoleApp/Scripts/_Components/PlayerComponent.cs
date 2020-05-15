using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class PlayerComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }
        public Type Type { get; set; }

        public PlayerComponent(ISystem system)
        {
            System = system;
            Type = Type.PLAYER;
        }
    }
}
