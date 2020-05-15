using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace GPP_2019
{
    class Utils
    {
        private Utils() { fpsTimer = new LTimer(); }
        private static Utils instance = null;
        public static Utils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Utils();
                }
                return instance;
            }
        }

        private LTimer fpsTimer;
        
        
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
