using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class ButtonComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }
        public Rectangle Rectangle { get; set; }
        public String Name { get; set; }

        public ButtonComponent(ButtonSystem buttonSystem, String name)
        {
            System = buttonSystem;
            Name = name;
        }
    }
}
