using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class BulletComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }
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
