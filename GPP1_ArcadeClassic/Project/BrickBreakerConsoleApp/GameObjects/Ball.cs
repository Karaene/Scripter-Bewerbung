using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class Ball : IRenderable, IUpdateable, ICollideable
    {
        #region Attributes and Properties
        private Sphere Sphere { get; set; }
        private Rectangle ballRect;
        public Sprite Sprite { get; set; }
        public int damage = 5;
        public int increasedDamage = 5;

        public Transform Transform { get; set; }
        private Dimension defaultSize = new Dimension(20, 20);

        private Origin origin = Origin.UPPERLEFT;

        public const double START_SPEED = 2.5;
        private const float BALL_SPEED_ACCELERATION = 0.15f;
        private const float MAX_BALL_SPEED = 7.0f;

        public double Speed { get; set; } = START_SPEED;
        public bool catched = true;
        #endregion

        #region Constructors
        public Ball()
        {
            Transform = new Transform(new Vector2D(0,0), defaultSize);
            ballRect = new Rectangle(Transform.Position, Transform.Dimension);
        }

        public Ball(int x, int y)
        {
            Transform = new Transform(new Vector2D(x, y), defaultSize);
            ballRect = new Rectangle(Transform.Position, Transform.Dimension);
            Sphere = new Sphere(this.Transform.Position, 7);
        }

        public Ball(int x, int y, Sprite spriteBall)
        {
            Sprite = spriteBall;
            Transform = new Transform(new Vector2D(x, y), defaultSize);
            ballRect = new Rectangle(Transform.Position, Transform.Dimension);
            Sphere = new Sphere(this.Transform.Position, 7);
        }

        public Ball(int x, int y, int width, int height)
        {
            Transform = new Transform(new Vector2D(x, y), new Dimension(width, height));
            ballRect = new Rectangle(Transform.Position, Transform.Dimension);
        }

        public Ball(int x, int y, int width, int height, Sprite spriteBall)
        {
            Sprite = spriteBall;
            Transform = new Transform(new Vector2D(x, y), new Dimension(width, height));
            ballRect = new Rectangle(Transform.Position, Transform.Dimension);
        }
        #endregion

        #region Methodes
        public void SetDirection(Vector2D direction)
        {
            double length = direction.Norm();
            Transform.Direction = new Vector2D((direction.X / length) * Speed, (direction.Y / length) * Speed);
        }

        public Sphere GetSphere()
        {
            return Sphere;
        }
        
        public void Accelerate()
        {
            if (Speed <= MAX_BALL_SPEED)
                Speed += BALL_SPEED_ACCELERATION;
            return;
        }

        public double GetBallCenterX()
        {
            double ballCenterX = ballRect.X + ballRect.Transform.Dimension.Width / 2.0;
            return ballCenterX;
        }

        public void FollowPaddle(Paddle paddle)
        {
            if (paddle.Transform.Position.Y < 100)
            {
                Transform.Position = new Vector2D(paddle.Transform.Position.X + paddle.Transform.Dimension.Width / 2 - Transform.Dimension.Width / 2, paddle.Transform.Position.Y + paddle.Transform.Dimension.Height * 2);
                SetDirection(Vector2D.UP);
            }
            else
            {
                Transform.Position = new Vector2D(paddle.Transform.Position.X + paddle.Transform.Dimension.Width / 2 - Transform.Dimension.Width / 2, paddle.Transform.Position.Y - paddle.Transform.Dimension.Height);
                SetDirection(Vector2D.DOWN);
            }
        }

        public void IncreaseDamage()
        {
            damage = damage + increasedDamage;
        }
        
        public void Update()
        {
            if (!catched)
            {
                Transform.Position += new Vector2D(Transform.Direction.X, Transform.Direction.Y);
            }
            ballRect.Transform = Transform;
            ballRect.Update();
            Sphere.Update(Transform.Position);
        }

        public void Render(IntPtr renderer)
        {
            if (Sprite != null)
            {
                Sprite.Render(renderer, ballRect.rect);
            }
            else
            {
                ballRect.Render(renderer);
            }
        }
        #endregion

        #region Enums
        public enum Origin { UPPERLEFT, UPPERMIDDLE, UPPERRIGHT, LEFT, MIDDLE, RIGHT, LOWERLEFT, LOWERMIDDLE, LOWERRIGHT }
        #endregion
    }
}
