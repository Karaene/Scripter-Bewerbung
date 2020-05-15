using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SDL2.SDL;
using System.Threading.Tasks;

namespace BrickBreakerConsoleApp
{
    class EventManager
    {
        #region Attributes and Properties
        private Game game;
        private static SDL_Event e;
        private IntPtr controller;
        #endregion

        #region Constructors
        public EventManager(Game game)
        {
            this.game = game;
            controller = SDL_GameControllerOpen(0);
            Console.Out.WriteLine(SDL_GameControllerMapping(controller));
        }
        #endregion

        #region Methodes
        public void UpdateInputEvents()
        {
            while (SDL_PollEvent(out e) != 0)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        game.IsRunning = false;
                        break;

                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        Game._StartButton.OnClick(new Vector2D(e.motion.x, e.motion.y));
                        break;
                        
                    case SDL_EventType.SDL_MOUSEMOTION:
                        game.PaddleMovement(e.motion.x);
                        break;
                    
                    case SDL_EventType.SDL_KEYDOWN:
                        switch (e.key.keysym.sym)
                        {
                            case SDL_Keycode.SDLK_ESCAPE:
                                game.IsRunning = false;
                                break;
                            case SDL_Keycode.SDLK_RIGHT:
                                if (game.TwoPlayerMode)
                                {
                                    game.PlayerTwoMoveRight = true;
                                }
                                else
                                {
                                    game.PlayerOneMoveRight = true;
                                    game.PlayerTwoMoveRight = true;
                                }
                                break;
                            case SDL_Keycode.SDLK_LEFT:
                                if (game.TwoPlayerMode)
                                {
                                    game.PlayerTwoMoveLeft = true;
                                }
                                else
                                {
                                    game.PlayerOneMoveLeft = true;
                                    game.PlayerTwoMoveLeft = true;
                                }
                                break;
                            case SDL_Keycode.SDLK_SPACE:
                                game.Space = true;
                                break;
                            case SDL_Keycode.SDLK_LSHIFT:
                                game.SpeedUp = true;
                                break;
                            case SDL_Keycode.SDLK_d:
                                if (game.TwoPlayerMode)
                                {
                                    game.PlayerOneMoveRight = true;
                                }
                                else
                                {
                                    game.PlayerOneMoveRight = true;
                                    game.PlayerTwoMoveRight = true;
                                }
                                break;
                            case SDL_Keycode.SDLK_a:
                                if (game.TwoPlayerMode)
                                {
                                    game.PlayerOneMoveLeft = true;
                                }
                                else
                                {
                                    game.PlayerOneMoveLeft = true;
                                    game.PlayerTwoMoveLeft = true;
                                }
                                break;
                            case SDL_Keycode.SDLK_RETURN:
                                game.SwitchPlayerMode();
                                break;
                        }
                        break;

                    case SDL_EventType.SDL_KEYUP:
                        switch (e.key.keysym.sym)
                        {
                            case SDL_Keycode.SDLK_RIGHT:
                                if (game.TwoPlayerMode)
                                {
                                    game.PlayerTwoMoveRight = false;
                                }
                                else
                                {
                                    game.PlayerOneMoveRight = false;
                                    game.PlayerTwoMoveRight = false;
                                }
                                break;
                            case SDL_Keycode.SDLK_LEFT:
                                if (game.TwoPlayerMode)
                                {
                                    game.PlayerTwoMoveLeft = false;
                                }
                                else
                                {
                                    game.PlayerOneMoveLeft = false;
                                    game.PlayerTwoMoveLeft = false;
                                }
                                break;
                            case SDL_Keycode.SDLK_SPACE:
                                game.Space = false;
                                break;
                            case SDL_Keycode.SDLK_LSHIFT:
                                game.SpeedUp = false;
                                break;
                            case SDL_Keycode.SDLK_d:
                                if (game.TwoPlayerMode)
                                {
                                    game.PlayerOneMoveRight = false;
                                }
                                else
                                {
                                    game.PlayerOneMoveRight = false;
                                    game.PlayerTwoMoveRight = false;
                                }
                                break;
                            case SDL_Keycode.SDLK_a:
                                if (game.TwoPlayerMode)
                                {
                                    game.PlayerOneMoveLeft = false;
                                }
                                else
                                {
                                    game.PlayerOneMoveLeft = false;
                                    game.PlayerTwoMoveLeft = false;
                                }
                                break;
                        }
                        break;

                    case SDL_EventType.SDL_CONTROLLERAXISMOTION:
                        switch (e.caxis.axis)
                        {
                            case 0: // Left Stick X-Axis
                                if (e.caxis.axisValue > 100)
                                    {
                                        if (game.TwoPlayerMode)
                                        {
                                            game.PlayerTwoMoveRight = true;
                                            game.PlayerTwoMoveLeft = false;
                                        }
                                        else
                                        {
                                            game.PlayerOneMoveRight = true;
                                            game.PlayerOneMoveLeft = false;
                                            game.PlayerTwoMoveRight = true;
                                            game.PlayerTwoMoveLeft = false;
                                        }
                                    }
                                    else if (e.caxis.axisValue < -100)
                                    {
                                        if (game.TwoPlayerMode)
                                        {
                                            game.PlayerTwoMoveLeft = true;
                                            game.PlayerTwoMoveRight = false;
                                        }
                                        else
                                        {
                                            game.PlayerOneMoveLeft = true;
                                            game.PlayerOneMoveRight = false;
                                            game.PlayerTwoMoveLeft = true;
                                            game.PlayerTwoMoveRight = false;
                                        }
                                    }
                                    else
                                    {
                                        if (game.TwoPlayerMode)
                                        {
                                            game.PlayerTwoMoveRight = false;
                                            game.PlayerTwoMoveLeft = false;
                                        }
                                        else
                                        {
                                            game.PlayerOneMoveRight = false;
                                            game.PlayerOneMoveLeft = false;
                                            game.PlayerTwoMoveRight = false;
                                            game.PlayerTwoMoveLeft = false;
                                        }
                                    }
                                break;

                            case 1: //Left Stick Y-Axis
                                break;

                            case 2: //Right Stick X-Axis
                                break;

                            case 3: //Right Stick Y-Axis
                                break;

                            case 4: //Left Trigger
                                break;

                            case 5: //Right Trigger
                                if (e.caxis.axisValue > 100)
                                {
                                    game.SpeedUp = true;
                                }
                                else if (e.caxis.axisValue < -100)
                                {
                                    game.SpeedUp = true;
                                }
                                else
                                {
                                    game.SpeedUp = false;
                                }
                                break;
                        }
                        break;
                }
            }
        }
        #endregion
    }
}
