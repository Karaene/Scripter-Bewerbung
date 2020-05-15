using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class InventoryComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }
        public List<GameObject> Items { get; set; }

        public InventoryComponent(ISystem system)
        {
            System = system;
            Items = new List<GameObject>();
        }
    }
}
