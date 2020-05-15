using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class VelocityComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }
        public Type Type { get; set; } = Type.VELOCITY;
        public Vector2D Velocity { get; set; } = Vector2D.ZERO;

        public VelocityComponent(ISystem system , Vector2D velocity)
        {
            System = system;
            Velocity = velocity;
        }
    }
}
