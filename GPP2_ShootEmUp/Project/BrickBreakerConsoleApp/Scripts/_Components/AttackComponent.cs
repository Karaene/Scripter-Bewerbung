using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class AttackComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }
        public Type Type { get; set; }
        public AttackType AttackType { get; set; }
        public int Damage { get; set; }

        public AttackComponent(ISystem system, AttackType attackType, int damage)
        {
            System = system;
            AttackType = attackType;
            Damage = damage;
        }
    }

    public enum AttackType { SHOOT, PUNCH }
}
