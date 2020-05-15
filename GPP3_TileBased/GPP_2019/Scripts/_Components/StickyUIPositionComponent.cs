using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class StickyUIPositionComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }
        public Vector2D UIPos { get; set; }

        public StickyUIPositionComponent(ISystem system, Vector2D uiPos)
        {
            System = system;
            UIPos = uiPos;
        }
    }
}
