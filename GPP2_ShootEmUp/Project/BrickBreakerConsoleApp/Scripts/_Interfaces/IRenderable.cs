using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    interface IRenderable
    {
        void Render(IntPtr renderer);
        void Update();
    }
}
