using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class TileComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }
        public int TileID { get; set; }
        public bool walkable { get; set; }
        public bool door { get; set; }
        public int Level { get; set; }

        public TileComponent(ISystem system, int id, bool walkable, bool door, int level)
        {
            System = system;
            TileID = id;
            this.walkable = walkable;
            this.door = door;
            this.Level = level;
        }
    }
}
