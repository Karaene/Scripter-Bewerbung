using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace BrickBreakerConsoleApp
{
    class LTimer
    {
        //The clock time when the timer started
        uint startTicks;

        //The ticks stored when the timer was paused
        uint pausedTicks;

        //The timer status
        bool paused;
        bool started;

        public LTimer()
        {
            startTicks = 0;
            pausedTicks = 0;
            paused = false;
            started = false;
        }

        public void Start()
        {
            //Start the timer
            started = true;

            //Unpause the timer
            paused = false;

            //Get the current clock time
            startTicks = SDL_GetTicks();
        }


        public void Stop()
        {
            //Stop the timer
            started = false;

            //Unpause the timer
            paused = false;
        }
        public void Pause()
        {
            //If the timer is running and isn't already paused
            if ((started == true) && (paused == false))
            {
                //Pause the timer
                paused = true;

                //Calculate the paused ticks
                pausedTicks = SDL_GetTicks() - startTicks;
            }
        }
        public void Unpause()
        {
            //If the timer is paused
            if (paused == true)
            {
                //Unpause the timer
                paused = false;

                //Reset the starting ticks
                startTicks = SDL_GetTicks() - pausedTicks;

                //Reset the paused ticks
                pausedTicks = 0;
            }
        }

        public uint GetTicks()
        {
            //If the timer is running
            if (started == true)
            {
                //If the timer is paused
                if (paused == true)
                {
                    //Return the number of ticks when the timer was paused
                    return pausedTicks;
                }
                else
                {
                    //Return the current time minus the start time
                    return SDL_GetTicks() - startTicks;
                }
            }

            //If the timer isn't running
            return 0;
        }
        public bool IsStarted()
        {
            return started;
        }

        public bool IsPaused()
        {
            return paused;
        }
    }
}