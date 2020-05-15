using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class SoundComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }
        public Sound Sound { get; set; }

        public SoundComponent(SoundSystem soundSystem, Sound sound)
        {
            System = soundSystem;
            Sound = sound;
        }
    }
}
