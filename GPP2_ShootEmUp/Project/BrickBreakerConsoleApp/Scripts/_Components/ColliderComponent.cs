using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class ColliderComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }
        public Type Type { get; set; }
        public AABB AABB { get; set; }

        public ColliderComponent(PhysicSystem physicSystem, GameObject obj)
        {
            System = physicSystem;
            Type = Type.PHYSICS;

            double minX = obj.Transform.Position.X - obj.Transform.Size.Width / 2;
            double maxX = obj.Transform.Position.X + obj.Transform.Size.Width / 2;
            double maxY = obj.Transform.Position.Y - obj.Transform.Size.Height / 2;
            double minY = obj.Transform.Position.Y + obj.Transform.Size.Height / 2;

            AABB = new AABB(minX, minY, maxX, maxY);
            
         //   Console.WriteLine("Collider minX: " + minX);
           // Console.WriteLine("Collider minY: " + minY);
         //   Console.WriteLine("Collider maxX: " + maxX);
         //   Console.WriteLine("Collider maxY: " + maxY);

        }
    }
}
