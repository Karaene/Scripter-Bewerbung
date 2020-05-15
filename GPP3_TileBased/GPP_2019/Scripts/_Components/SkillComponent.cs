using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class SkillComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }
        public Type Type { get; set; }
        public Dictionary<string, int> Skills { get; set; }

        //public int Health { get; set; }

        public SkillComponent(ISystem system)
        { 
            System = system;

            Skills = new Dictionary<string, int>();
            Skills.Add("Health", 0);
            Skills.Add("Attackspeed", 0);
        }
    }
}