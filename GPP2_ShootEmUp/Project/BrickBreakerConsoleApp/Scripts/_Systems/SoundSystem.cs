using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL_mixer;
using static SDL2.SDL;

namespace BrickBreakerConsoleApp
{
    class SoundSystem : ISystem
    {
        InputType inputType;
        Sound sound { get; set; }
        public bool Fire { get; set; } = false;

        private SoundSystem(){ }
        private static SoundSystem instance = null;
        public static SoundSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SoundSystem();
                }
                return instance;
            }
        }
        
        public void Init()
        {
            InitEvent();
            InitSound();
        }

        private void InitEvent()
        {
            //USED FOR SHOOTING SOUNDS
            EventSystem.Instance.AddListener("FireMouse", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) =>
                {
                    Fire = true;
                    inputType = InputType.KEYBOARD;
                }
            });
            EventSystem.Instance.AddListener("StopFire", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    Fire = false;
                }
            });
        }
        private bool InitSound()
        {
            //Initialize SDL_mixer
            if (Mix_OpenAudio(22050, MIX_DEFAULT_FORMAT, 2, 4096) == -1)
            {
                return false;
            }
            return true;
        }

        internal IEntityComponent CreateBackgroundSound(string filePath)
        {
            sound = new Sound(filePath);
            sound.PlayMusic();
            SoundComponent sc = new SoundComponent(this, sound);
            return sc;
        }
        internal IEntityComponent CreateSoundEffect(string filePath, string title)
        {
            sound = new Sound(filePath, title);
            SoundComponent sc = new SoundComponent(this, sound);
            return sc;
        }
        public void stopMusic()
        {
            sound.PauseMusic();
        }

        public void Update()
        {
            if (Fire)
            {
                sound.PlaySoundtrack();
            } 
            //other Events to do
        }
    }
}
