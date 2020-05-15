using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class AiComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }

        public AiComponent(ISystem system)
        {
            System = system;
        }
    }
}
