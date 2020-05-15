using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace BrickBreakerConsoleApp
{
    class InputSystem : ISystem
    {
        public static SDL_Event e;

        public float MoveSpeed { get; set; } = 6f;
        public bool MoveRight { get; set; }
        public bool MoveLeft { get; set; }
        public bool MoveUp { get; set; }
        public bool MoveDown { get; set; }

        public int X_Dir { get; set; } = 0;
        public int Y_Dir { get; set; } = 0;

        private List<InputComponent> _activePlayers = new List<InputComponent>();


        #region Constructors
        private InputSystem() { }
        private static InputSystem instance = null;
        private bool paused = false;
        public static InputSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputSystem();
                }
                return instance;
            }
        }
        #endregion

        public void UpdateMenue()
        {
            while (SDL_PollEvent(out e) != 0)
            {
                        switch (e.type)
                        {
                            case SDL_EventType.SDL_QUIT:
                                EventSystem.Instance.RaiseEvent("Quit");
                                break;

                            case SDL_EventType.SDL_KEYDOWN:
                                switch (InputSystem.e.key.keysym.sym)
                                {
                                    case SDL_Keycode.SDLK_p:
                                           // EventSystem.Instance.RaiseEvent("Pause");
                                            EventSystem.Instance.RaiseEvent("UnPaused");
                                            Console.WriteLine("UnPaused");
                                        break;
                                    case SDL_Keycode.SDLK_q:
                                             EventSystem.Instance.RaiseEvent("Quit"); ;
                                        break;
                                    case SDL_Keycode.SDLK_ESCAPE:
                                        EventSystem.Instance.RaiseEvent("Quit"); ;
                                        break;
                                 }
                                break;
                            case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                                EventSystem.Instance.RaiseEvent("ButtonClick", new Vector2D(e.motion.x, e.motion.y));
                                break;
                            case SDL_EventType.SDL_MOUSEBUTTONUP:
                                EventSystem.Instance.RaiseEvent("ButtonStop", new Vector2D(e.motion.x, e.motion.y));
                                break;
                        }       
            }
        }
        #region Methodes
            /*
        public void Update()
        {
            while (SDL_PollEvent(out e) != 0)
            {
                foreach (var inputComponent in _activePlayers)
                {
                    if (inputComponent.InputType == InputType.KEYBOARD)
                    {
                        switch (InputSystem.e.type)
                        {
                            case SDL_EventType.SDL_QUIT:
                                EventSystem.Instance.RaiseEvent("Quit");
                                break;

                            case SDL_EventType.SDL_KEYDOWN:
                                switch (InputSystem.e.key.keysym.sym)
                                {
                                    case SDL_Keycode.SDLK_ESCAPE:
                                        EventSystem.Instance.RaiseEvent("Quit");
                                        break;
                                    case SDL_Keycode.SDLK_d:
                                        MoveRight = true;
                                        break;
                                    case SDL_Keycode.SDLK_a:
                                        MoveLeft = true;
                                        break;
                                    case SDL_Keycode.SDLK_w:
                                        MoveUp = true;
                                        break;
                                    case SDL_Keycode.SDLK_s:
                                        MoveDown = true;
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
                                break;
                            case SDL_EventType.SDL_KEYUP:
                                switch (InputSystem.e.key.keysym.sym)
                                {
                                    case SDL_Keycode.SDLK_d:
                                        MoveRight = false;
                                        break;
                                    case SDL_Keycode.SDLK_a:
                                        MoveLeft = false;
                                        break;
                                    case SDL_Keycode.SDLK_w:
                                        MoveUp = false;
                                        break;
                                    case SDL_Keycode.SDLK_s:
                                        MoveDown = false;
                                        break;
                                    case SDL_Keycode.SDLK_m:
                                        EventSystem.Instance.RaiseEvent("ShowNearbyCollider", inputComponent.GameObject);
                                        break;
                                    case SDL_Keycode.SDLK_n:
                                        EventSystem.Instance.RaiseEvent("StopDrawOutline");
                                        break;
                                }
                                break;
                        }
                    }
                    if (inputComponent.InputType == InputType.CONTROLLER)
                    {
                        //Console.WriteLine("Controller Input needs to be Implemented");
                        switch (InputSystem.e.type)
                        {
                            case SDL_EventType.SDL_CONTROLLERAXISMOTION:
                                switch (InputSystem.e.caxis.axis)
                                {
                                    case 0: // Left Stick X-Axis
                                        if (InputSystem.e.caxis.axisValue > 8000)
                                        {
                                            MoveRight = true;
                                        }
                                        else if (InputSystem.e.caxis.axisValue < -8000)
                                        {
                                            MoveLeft = true;
                                        }
                                        else
                                        {
                                            MoveRight = false;
                                            MoveLeft = false;
                                        }
                                        break;
                                    case 1:
                                        if (InputSystem.e.caxis.axisValue > 8000)
                                        {
                                            MoveDown = true;
                                        }
                                        else if (InputSystem.e.caxis.axisValue < -8000)
                                        {
                                            MoveUp = true;
                                        }
                                        else
                                        {
                                            MoveUp = false;
                                            MoveDown = false;
                                        }
                                        break;
                                    case 2:
                                        if (InputSystem.e.caxis.axisValue > 8000)
                                        {
                                            X_Dir = 1;
                                            EventSystem.Instance.RaiseEvent("ChangedDir", new Vector2D(X_Dir, Y_Dir));
                                        }
                                        else if (InputSystem.e.caxis.axisValue < -8000)
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
                                        if (InputSystem.e.caxis.axisValue > 8000)
                                        {
                                            Y_Dir = 1;
                                            EventSystem.Instance.RaiseEvent("ChangedDir", new Vector2D(X_Dir, Y_Dir));
                                        }
                                        else if (InputSystem.e.caxis.axisValue < -8000)
                                        {
                                            Y_Dir = -1;
                                            EventSystem.Instance.RaiseEvent("ChangedDir", new Vector2D(X_Dir, Y_Dir));
                                        }
                                        else
                                        {
                                            Y_Dir = 0;
                                        }
                                        break;
                                    case 5:
                                        /*
                                        if (InputSystem.e.caxis.axisValue > 8000)
                                        {
                                            EventSystem.Instance.RaiseEvent("ShotFiredController", new Vector2D(X_Dir, Y_Dir));
                                        }
                                        
                                        break;
                                }
                                break;
                            case SDL_EventType.SDL_CONTROLLERBUTTONDOWN:
                                switch (InputSystem.e.cbutton.button)
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
                                switch (InputSystem.e.cbutton.button)
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
            
            foreach (var inputComponent in _activePlayers)
            {
                if (MoveRight && (inputComponent.GameObject.Transform.Position.X <= LevelManager.LEVEL_WIDTH - inputComponent.GameObject.Transform.Size.Width/2 - LevelManager.SCREEN_OFFSET_X))
                    inputComponent.GameObject.Transform.Position = new Vector2D(inputComponent.GameObject.Transform.Position.X + MoveSpeed, inputComponent.GameObject.Transform.Position.Y);
                if (MoveLeft && (inputComponent.GameObject.Transform.Position.X >= - LevelManager.SCREEN_OFFSET_X + inputComponent.GameObject.Transform.Size.Width / 2))
                    inputComponent.GameObject.Transform.Position = new Vector2D(inputComponent.GameObject.Transform.Position.X - MoveSpeed, inputComponent.GameObject.Transform.Position.Y);
                if (MoveUp && (inputComponent.GameObject.Transform.Position.Y >= -LevelManager.SCREEN_OFFSET_Y + inputComponent.GameObject.Transform.Size.Height / 2))
                    inputComponent.GameObject.Transform.Position = new Vector2D(inputComponent.GameObject.Transform.Position.X, inputComponent.GameObject.Transform.Position.Y - MoveSpeed);
                if (MoveDown && (inputComponent.GameObject.Transform.Position.Y <= LevelManager.LEVEL_HEIGHT - inputComponent.GameObject.Transform.Size.Height/2 - LevelManager.SCREEN_OFFSET_Y))
                    inputComponent.GameObject.Transform.Position = new Vector2D(inputComponent.GameObject.Transform.Position.X, inputComponent.GameObject.Transform.Position.Y + MoveSpeed);
            }
        }
            */

        public InputComponent CreateInput(InputType inputType)
        {
            InputComponent input = new InputComponent(this, inputType);
            _activePlayers.Add(input);
            
            return input;
        }
        

        #endregion
    }
    enum InputType { KEYBOARD, CONTROLLER, MENUKEYBOARD }
}
