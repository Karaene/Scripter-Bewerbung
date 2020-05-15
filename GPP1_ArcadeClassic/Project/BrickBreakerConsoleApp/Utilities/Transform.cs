using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class Transform
    {
        #region Attributes and Properties
        public Vector2D Position { get; set; }
        public Dimension Dimension { get; set; }
        public Vector2D Direction { get; set; }
        #endregion

        #region Constructors
        public Transform()
        {
            Position = new Vector2D(0, 0);
            Dimension = new Dimension(0, 0);
            Direction = new Vector2D(0, 0);
        }

        public Transform(Vector2D position)
        {
            this.Position = position;
            Dimension = new Dimension(0, 0);
            Direction = new Vector2D(0, 0);
        }

        public Transform(Vector2D position, Dimension dimension)
        {
            this.Position = position;
            this.Dimension = dimension;
            Direction = new Vector2D(0, 0);
        }

        public Transform(Vector2D position, Dimension dimension, Vector2D direction)
        {
            this.Position = position;
            this.Dimension = dimension;
            this.Direction = direction;
        }
        #endregion
    }
}
