using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class HealthComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }
        public int Health { get; set; }
        public Type Type { get; set; }

        public HealthComponent(ISystem system ,int health)
        {
            System = system;
            Health = health;
            Type = Type.HEALTH;
        }

    }
}
