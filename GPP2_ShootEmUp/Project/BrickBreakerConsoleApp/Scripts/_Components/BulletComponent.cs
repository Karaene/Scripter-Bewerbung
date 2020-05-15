using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class BulletComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }
        public Type Type { get; set; } = Type.BULLET;
        public bool Friendly { get; set; } 
        public Vector2D Direction { get; set; }
        public int Damage { get; set; }

        public BulletComponent(PlayerShootingSystem shootingSystem, int damage, bool friendly)
        {
            System = shootingSystem;
            Damage = damage;
            Friendly = friendly;
            //Type = Type.PHYSICS;
        }
    }
}
