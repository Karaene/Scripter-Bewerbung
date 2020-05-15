using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace BrickBreakerConsoleApp
{
    class PlayerControl : ISystem
    {
        public static SDL_Event e;

        public int X_Dir { get; set; } = 0;
        public int Y_Dir { get; set; } = 0;

        public PlayerComponent Player { get; set; }

        private List<PlayerComponent> _activePlayers = new List<PlayerComponent>();
        
        #region Constructors
        private PlayerControl() { }
        private static PlayerControl instance = null;
        public static PlayerControl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerControl();
                }
                return instance;
            }
        }
        #endregion

        public PlayerComponent CreatePlayerComponent()
        {
            PlayerComponent player = new PlayerComponent(this);
            _activePlayers.Add(player);
            Player = player;
            return player;
        }

        #region Methodes
       

        public void Update()
        {
            while (SDL_PollEvent(out e) != 0)
            {
                foreach (var player in _activePlayers)
                {
                    InputComponent tmpInput = (InputComponent)player.GameObject.GetComponent(Type.INPUT);
                    if ((tmpInput != null) && tmpInput.InputType == InputType.KEYBOARD)
                    {
                        switch (e.type)
                        {
                            case SDL_EventType.SDL_QUIT:
                                EventSystem.Instance.RaiseEvent("Quit");
                                break;

                            case SDL_EventType.SDL_KEYDOWN:
                                EventSystem.Instance.RaiseEvent("InputKeyDown", tmpInput);
                                EventSystem.Instance.RaiseEvent("KeyDown", e.key.keysym.sym, player.GameObject);
                                switch (e.key.keysym.sym)
                                {
                                    case SDL_Keycode.SDLK_ESCAPE:
                                        EventSystem.Instance.RaiseEvent("Quit");
                                        break;
                                    case SDL_Keycode.SDLK_d:
                                        tmpInput.MoveRight = true;
                                        break;
                                    case SDL_Keycode.SDLK_a:
                                        tmpInput.MoveLeft = true;
                                        break;
                                    case SDL_Keycode.SDLK_w:
                                        tmpInput.MoveUp = true;
                                        break;
                                    case SDL_Keycode.SDLK_s:
                                        tmpInput.MoveDown = true;
                                        break;
                                    case SDL_Keycode.SDLK_p:
                                        EventSystem.Instance.RaiseEvent("Pause");
                                        tmpInput.MoveDown = false;
                                        tmpInput.MoveLeft = false;
                                        tmpInput.MoveRight = false;
                                        tmpInput.MoveUp = false;
                                        break;
                                }
                                break;

                            case SDL_EventType.SDL_KEYUP:
                                EventSystem.Instance.RaiseEvent("InputKeyUp", tmpInput);
                                EventSystem.Instance.RaiseEvent("KeyUp", e.key.keysym.sym, player.GameObject);
                                switch (e.key.keysym.sym)
                                {
                                    case SDL_Keycode.SDLK_d:
                                        tmpInput.MoveRight = false;
                                        break;
                                    case SDL_Keycode.SDLK_a:
                                        tmpInput.MoveLeft = false;
                                        break;
                                    case SDL_Keycode.SDLK_w:
                                        tmpInput.MoveUp = false;
                                        break;
                                    case SDL_Keycode.SDLK_s:
                                        tmpInput.MoveDown = false;
                                        break;
                                    case SDL_Keycode.SDLK_m:
                                        EventSystem.Instance.RaiseEvent("ShowNearbyCollider", tmpInput.GameObject);
                                        break;
                                    case SDL_Keycode.SDLK_n:
                                        EventSystem.Instance.RaiseEvent("StopDrawOutline");
                                        break;
                                }
                                break;

                            case SDL_EventType.SDL_MOUSEMOTION:
                                EventSystem.Instance.RaiseEvent("ChangedDir", new Vector2D(e.motion.x, e.motion.y));
                                break;
                            case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                                EventSystem.Instance.RaiseEvent("FireMouse", new Vector2D(e.motion.x, e.motion.y));
                                break;
                            case SDL_EventType.SDL_MOUSEBUTTONUP:
                                EventSystem.Instance.RaiseEvent("StopFire");
                                EventSystem.Instance.RaiseEvent("ButtonStop", new Vector2D(e.motion.x, e.motion.y));

                                break;
                        }
                    }
                    if (tmpInput != null && tmpInput.InputType == InputType.CONTROLLER)
                    {
                        //Console.WriteLine("Controller Input needs to be Implemented");
                        switch (e.type)
                        {
                            case SDL_EventType.SDL_CONTROLLERAXISMOTION:
                                switch (e.caxis.axis)
                                {
                                    case 0: // Left Stick X-Axis
                                        if (e.caxis.axisValue > 8000)
                                        {
                                            tmpInput.MoveRight = true;
                                        }
                                        else if (InputSystem.e.caxis.axisValue < -8000)
                                        {
                                            tmpInput.MoveLeft = true;
                                        }
                                        else
                                        {
                                            tmpInput.MoveRight = false;
                                            tmpInput.MoveLeft = false;
                                        }
                                        break;
                                    case 1:
                                        if (e.caxis.axisValue > 8000)
                                        {
                                            tmpInput.MoveDown = true;
                                        }
                                        else if (e.caxis.axisValue < -8000)
                                        {
                                            tmpInput.MoveUp = true;
                                        }
                                        else
                                        {
                                            tmpInput.MoveDown = false;
                                            tmpInput.MoveUp = false;
                                        }
                                        break;
                                    case 2:
                                        if (e.caxis.axisValue > 8000)
                                        {
                                            X_Dir = 1;
                                            EventSystem.Instance.RaiseEvent("ChangedDir", new Vector2D(X_Dir, Y_Dir));
                                        }
                                        else if (e.caxis.axisValue < -8000)
                                        {
                                            X_Dir = -1;
                                            EventSystem.Instance.RaiseEvent("ChangedDir", new Vector2D(X_Dir, Y_Dir));
                                        }
                                        else
                                        {
                                            X_Dir = 0;
                                        }
                                        break;
                                    case 3:
                                        if (e.caxis.axisValue > 8000)
                                        {
                                            Y_Dir = 1;
                                            EventSystem.Instance.RaiseEvent("ChangedDir", new Vector2D(X_Dir, Y_Dir));
                                        }
                                        else if (e.caxis.axisValue < -8000)
                                        {
                                            Y_Dir = -1;
                                            EventSystem.Instance.RaiseEvent("ChangedDir", new Vector2D(X_Dir, Y_Dir));
                                        }
                                        else
                                        {
                                            Y_Dir = 0;
                                        }
                                        break;
                                }
                                break;
                            case SDL_EventType.SDL_CONTROLLERBUTTONDOWN:
                                switch (e.cbutton.button)
                                {
                                    case 10:
                                        EventSystem.Instance.RaiseEvent("FireController", new Vector2D(X_Dir, Y_Dir));
                                        break;
                                    case 6:
                                        EventSystem.Instance.RaiseEvent("Quit");
                                        break;
                                }

                                break;
                            case SDL_EventType.SDL_CONTROLLERBUTTONUP:
                                switch (e.cbutton.button)
                                {
                                    case 10:
                                        EventSystem.Instance.RaiseEvent("StopFire");
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
        }
    }

}
#endregion