using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SDL2.SDL;
using static SDL2.SDL_mixer;
using System.Threading.Tasks;

namespace GPP_2019
{
    class Sound
    {
        IntPtr music;
        IntPtr soundeffect;
        public String Title { get; set; }
        public Sound(String filePath)
        {
            music = Mix_LoadMUS(filePath);

        }
        
        public Sound(String filePath, String name)
        {
            soundeffect = Mix_LoadWAV(filePath);
            Title = name;
        }
        
        public bool PlaySoundtrack()
        {
            //Play the medium hit effect
            if (Mix_PlayChannel(-1, soundeffect, 0) == -1)
            {
                return true;
            }
            return false;
        }

        public bool PlayMusic()
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

        public void Stop()
        {
            Mix_HaltMusic();
        }
        public void PauseMusic()
        {
            //If the music is paused
            if (Mix_PausedMusic() == 1)
            {
                //Resume the music
                Mix_ResumeMusic();
            }
            //If the music is playing
            else
            {
                //Pause the music
                Mix_PauseMusic();
            }
        }
    }
}
