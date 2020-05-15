using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class CircleColliderComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }
        public bool HitCollider { get; set; } = false;
        public int Radius { get; set; }

        public CircleColliderComponent(PhysicSystem physicSystem)
        {
            System = physicSystem;
        }
    }
}
