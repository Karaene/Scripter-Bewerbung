using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace GPP_2019
{
    class InputComponent : IEntityComponent
    {
        public ISystem System { get; }
        public GameObject GameObject { get; set; }
        public InputType InputType { get; set; }
        public IntPtr Controller { get; set; }

        public bool MoveUp { get; set; }
        public bool MoveDown { get; set; }
        public bool MoveLeft { get; set; }
        public bool MoveRight { get; set; }

        public InputComponent(ISystem system, InputType type)
        {
            System = system;
            InputType = type;

            if (InputType == InputType.CONTROLLER)
            {
                Controller = SDL_GameControllerOpen(0);
                Console.Out.WriteLine(SDL_GameControllerMapping(Controller));
            }
        }
    }
}
