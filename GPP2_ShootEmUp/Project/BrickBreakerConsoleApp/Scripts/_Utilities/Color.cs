﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    public struct Color
    {
        public byte R;
        public byte G;
        public byte B;

        public Color(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }
    }
}
