using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class CircleColliderComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }
        public Type Type { get; set; }
        public bool HitCollider { get; set; } = false;
        public int Radius { get; set; }

        public CircleColliderComponent(PhysicSystem physicSystem)
        {
            System = physicSystem;
            Type = Type.CIRCLE_COLLIDER;
        }
    }
}
