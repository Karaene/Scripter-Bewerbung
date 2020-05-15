using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SDL2.SDL;
using static SDL2.SDL_mixer;
using System.Threading.Tasks;

namespace BrickBreakerConsoleApp
{
    class Sound
    {
        IntPtr music;
        public Sound()
        {
            InitSound();
            LoadMusic();
            PlayMusic();
        }

        private bool InitSound()
        {
            if (Mix_OpenAudio(22050, MIX_DEFAULT_FORMAT, 2, 4096) == -1)
            {
                return false;
            }
            return true;
        }

        private bool LoadMusic()
        {
            music = Mix_LoadMUS("unity.mp3");
            
            if (music == IntPtr.Zero)
            {
                return false;
            }
            return true;
        }

        private bool PlayMusic()
        {
            if (Mix_PlayingMusic() == 0)
            {
                if (Mix_PlayMusic(music, -1) == -1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
