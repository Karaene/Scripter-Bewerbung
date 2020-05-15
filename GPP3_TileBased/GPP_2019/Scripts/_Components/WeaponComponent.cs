using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class WeaponComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }
        public WeaponType WeaponType { get; set; }
        public int Damage { get; set; }
        public int FireRate { get; set; }

        public WeaponComponent(PlayerShootingSystem shootingSystem)
        {
            System = shootingSystem;
        }
        
    }
}
