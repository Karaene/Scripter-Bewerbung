using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    interface IRenderable
    {
        void Render(IntPtr renderer);
        void Update();
    }
}
