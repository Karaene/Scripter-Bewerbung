using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class SpawnerComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }
        public float CoolDown { get; set; } = 2.0f;
        public EnemyType EnemyType { get; set; } = EnemyType.NORMAL;
        public int SpawnAmount { get; set; } = 0;
        public Mode Mode { get; set; } = Mode.SINGLE;
        public uint StartTick { get; set; } = 0;


        public SpawnerComponent(ISystem system)
        {
            System = system;
        }
    }

    public enum Mode { SINGLE, HORDE }
}
