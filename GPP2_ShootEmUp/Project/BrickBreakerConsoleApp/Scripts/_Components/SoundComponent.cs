using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class SoundComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }
        public Type Type { get; set; }
        public Sound Sound { get; set; }

        public SoundComponent(SoundSystem soundSystem, Sound sound)
        {
            System = soundSystem;
            Sound = sound;
            Type = Type.SOUND;
        }
    }
}
