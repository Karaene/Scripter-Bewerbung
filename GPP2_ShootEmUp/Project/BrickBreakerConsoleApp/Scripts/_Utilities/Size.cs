using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    public struct Size
    {
        #region Attributes and Properties

        public double Width { get; set; }
        public double Height { get; set; }
        #endregion

        #region Constructors
        public Size(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }
        #endregion

        #region Methodes
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Size)
            {
                Size v = (Size)obj;
                if (v.Width == Width && v.Height == Height)
                    return obj.GetType().Equals(this.GetType());
            }
            return false;
        }

        public override string ToString()
        {
            return String.Format("{{Width={0}, Height={1}}}", Width, Height);
        }

        #endregion
    }
}
