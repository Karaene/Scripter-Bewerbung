using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class PlayerComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }

        public PlayerComponent(ISystem system)
        {
            System = system;
        }
    }
}
