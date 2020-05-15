using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class ObstacleComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }

        public ObstacleComponent(ISystem system)
        {
            System = system;
        }
    }
}
