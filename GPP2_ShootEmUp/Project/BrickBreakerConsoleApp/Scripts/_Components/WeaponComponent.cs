using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class WeaponComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }
        public Type Type { get; set; } = Type.WEAPON;
        public WeaponType WeaponType { get; set; } = WeaponType.PISTOL;

        public WeaponComponent(PlayerShootingSystem shootingSystem)
        {
            System = shootingSystem;
        }
        
    }
}
