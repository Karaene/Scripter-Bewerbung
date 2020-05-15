using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{ 
    class PickUpComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }
        public PickUpType PickUpType { get; set; }
        public WeaponType WeaponType { get; set; }

        public PickUpComponent(ISystem system, PickUpType pickUpType)
        {
            System = system;
            PickUpType = pickUpType;
        }
    }
}

public enum PickUpType { COIN, ITEM }
