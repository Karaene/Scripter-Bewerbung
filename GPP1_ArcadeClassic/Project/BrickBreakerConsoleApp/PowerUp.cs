using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class PowerUp : IRenderable, IUpdateable, ICollideable
    {
        public Type PowerUpType { get; }

        private Rectangle powerUpRect;
        public Transform Transform { get; set; }
        private Dimension defaultSize = new Dimension(24, 24);
        private static Color color = new Color(056, 056, 056);
        private const float SPEED = 0.5f;

        public PowerUp(int x, int y, Type type)
        {
            PowerUpType = type;
            Transform = new Transform(new Vector2D(x, y), defaultSize);
            powerUpRect =  new Rectangle(Transform.Position, Transform.Dimension);
            SetDirection(Vector2D.UP);
        }

        public void SetDirection(Vector2D direction)
        {
            double length = direction.Norm();
            Transform.Direction = new Vector2D((direction.X / length) * SPEED, (direction.Y / length) * SPEED);
        }

        private bool OutOfBounds(PlayingField playingField)
        {
            if ((powerUpRect.Y > playingField.Height || powerUpRect.Y < 0))
            {
                return true;
            }
            return false;
        }

        public void Update()
        {
            Transform.Position += new Vector2D(Transform.Direction.X, Transform.Direction.Y);
            powerUpRect.Transform = Transform;
            powerUpRect.Update();
        }

        public void Render(IntPtr renderer)
        {
            powerUpRect.Render(renderer);
        }

        public enum Type { BIGGER_PANEL, POINTS_MULTIPLIER , BALL_SLOWDOWN }
    }
}
