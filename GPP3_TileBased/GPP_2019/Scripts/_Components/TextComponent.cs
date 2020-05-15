using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class TextComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }
        public Text Text { get; set; }

        public TextComponent(ISystem system, Text text)
        {
            System = system;
            Text = text;
        }
    }
}
