using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class AiComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }
        public Type Type { get; set; } = Type.AI;

        public AiComponent(ISystem system)
        {
            System = system;
        }
    }
}
