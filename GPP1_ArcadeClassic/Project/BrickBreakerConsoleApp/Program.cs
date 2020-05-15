using System;
using System.Collections.Generic;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace BrickBreakerConsoleApp
{
    class Program
    {
        #region Attributes and Properties
        private static IntPtr _Window = IntPtr.Zero;
        private static IntPtr _Renderer = IntPtr.Zero;
        //private static int gameWidth = 1920;
        //private static int gameHeight = 1080;
        private static int gameWidth = 1280;
        private static int gameHeight = 720;
        //private static int gameWidth = 720;
        //private static int gameHeight = 480;
        #endregion

        #region Methodes
        static void Main(string[] args)
        {
            Program program = new Program();
            if (program.Init() == false) { Error("Failed to initialize!"); }
            program.Run();

            SDL_Quit();
            return;
        }
        
        private bool Init()
        {
            bool success = true;

            if (SDL_Init(SDL_INIT_EVERYTHING) < 0)
            {
                Error("Init failed");
                success = false;
            }

            _Window = SDL_CreateWindow
            (   "Brick_Breaker", 
                SDL_WINDOWPOS_UNDEFINED, 
                SDL_WINDOWPOS_UNDEFINED, 
                gameWidth, 
                gameHeight, 
                SDL_WindowFlags.SDL_WINDOW_SHOWN
            );

            //SDL_SetWindowFullscreen(_Window, (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);

            if (_Window == null)
            {
                Error("Window Creation failed");
                success = false;
            }

            _Renderer = SDL_CreateRenderer(_Window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            return success;
        }

        private void Run()
        {
            Game brickBreaker = new Game(_Renderer, gameWidth, gameHeight);
            //brickBreaker.Start();
            brickBreaker.ShowMenu();
        }

        private static void Error(string v)
        {
            Console.WriteLine($"ERROR: { v } SDL_Error: {SDL_GetError()}");
        }
        #endregion
        
    }
}
