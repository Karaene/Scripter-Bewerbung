using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace BrickBreakerConsoleApp
{
    class Utils
    {
        private LTimer fpsTimer;
        

        public Utils()
        {
            fpsTimer = new LTimer();
        }
        
        public void StartTimer()
        {
            fpsTimer.Start();
        }

        public float CalculateFPS (int countedFrames)
        {
            float avgFPS = countedFrames / (fpsTimer.GetTicks() / 1000f);
            if (avgFPS > 2000000)
            {
                avgFPS = 0;
            }
            return avgFPS;
        }
    }
}
