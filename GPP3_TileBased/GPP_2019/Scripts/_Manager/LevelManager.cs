using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class LevelManager
    {
        public const int LEVEL_WIDTH = 1024*2;
        public const int LEVEL_HEIGHT = 1024*2;

        public static int SCREEN_OFFSET_X = (LEVEL_WIDTH - Program.SCREEN_WIDTH) / 2;
        public static int SCREEN_OFFSET_Y = (LEVEL_HEIGHT - Program.SCREEN_HEIGHT) / 2;

        //Singleton
        private LevelManager() { }
        private static LevelManager instance = null;
        public static LevelManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LevelManager();
                }
                return instance;
            }
        }
        public void Update()
        {
               SCREEN_OFFSET_X = (LEVEL_WIDTH - Program.SCREEN_WIDTH) / 2;
               SCREEN_OFFSET_Y = (LEVEL_HEIGHT - Program.SCREEN_HEIGHT) / 2;
        }

    }
}
