using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    public class Transform 
    {
        #region Attributes and Properties
        public Vector2D Position { get; set; }
        public Size Size { get; set; }
        public Vector2D Direction { get; set; }
        public double Rotation { get; set; } = 0;
        #endregion

        #region Constructors
        public Transform()
        {
            Position = new Vector2D(0, 0);
            Size = new Size(0, 0);
            Direction = new Vector2D(0, 0);
        }

        public Transform(Vector2D position)
        {
            this.Position = position;
            Size = new Size(0, 0);
            Direction = new Vector2D(0, 0);
        }

        public Transform(Vector2D position, Size size)
        {
            this.Position = position;
            this.Size = size;
            Direction = new Vector2D(0, 0);
        }

        public Transform(Vector2D position, Size size, Vector2D direction)
        {
            this.Position = position;
            this.Size = size;
            this.Direction = direction;
        }
        #endregion
    }
}
