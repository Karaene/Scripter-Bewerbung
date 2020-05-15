using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{ 
    class PickUpSystem : ISystem
    {
        private List<IEntityComponent> _activePickUps = new List<IEntityComponent>();

        private PickUpSystem() { }
        private static PickUpSystem instance = null;
        public static PickUpSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PickUpSystem();
                }
                return instance;
            }
        }
        public IEntityComponent CreatePickUpComponent(PickUpType pickUpType)
        {
            PickUpComponent pc = new PickUpComponent(this, pickUpType);
            _activePickUps.Add(pc);
            return pc;
        }

        public IEntityComponent CreatePickUpComponent(PickUpType pickUpType, WeaponType type)
        {
            PickUpComponent pc = new PickUpComponent(this, pickUpType);
            pc.WeaponType = type;
            _activePickUps.Add(pc);
            return pc;
        }

        public void Update()
        {

        }
    }
}

