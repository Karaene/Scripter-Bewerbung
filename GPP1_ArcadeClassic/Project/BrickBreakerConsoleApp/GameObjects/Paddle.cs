using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace BrickBreakerConsoleApp 
{
    class Paddle : IRenderable, IUpdateable, ICollideable
    {
        #region Attributes and Properties
        public Rectangle paddleRect;
        //private Sprite spritePaddle;
        Placed placed;
        public Transform Transform { get; set; }
        public float MoveSpeed { get; set; } = 5;
        public Sprite Sprite { get; set; }
        public bool paddleSizeBuff = false;
        private int speedMultiplier = 1;
        public Placed PlacedPosition { get { return placed; }}

        #endregion

        #region Constructors
        public Paddle(Placed placed)
        {
            paddleRect = new Rectangle(new Vector2D(0, 0), new Dimension(0, 0));
            Transform = new Transform();
            this.placed = placed;
        }
        
        public Paddle(int x, int y, int width, int height, Color color, Placed placed)
        {
            paddleRect = new Rectangle(new Vector2D(x, y), new Dimension(width, height), color);
            Transform = new Transform(new Vector2D(x, y), new Dimension(width, height));
            this.placed = placed;
        }
        

        public Paddle(int x, int y, int width, int height, Sprite sprite, Placed placed)
        {
            Sprite = sprite;
            paddleRect = new Rectangle(new Vector2D(x, y), new Dimension(width, height));
            Transform = new Transform(new Vector2D(x, y), new Dimension(width, height));
            this.placed = placed;
        }
        #endregion

        #region Methodes
        public void MovePaddle(Direction dir, PlayingField playingField)
        {
            switch (dir)
            {
                case Direction.LEFT:
                    if (Transform.Position.X > 0)
                    {
                        Transform.Position = new Vector2D(Transform.Position.X - MoveSpeed * speedMultiplier, Transform.Position.Y);
                    }
                    break;
                case Direction.RIGHT:
                    if (Transform.Position.X < playingField.Width - Transform.Dimension.Width)
                    {
                        Transform.Position = new Vector2D(Transform.Position.X + MoveSpeed * speedMultiplier, Transform.Position.Y);
                    }
                    break;
            }
        }

        public void PaddleMovement(int mouseposition)
        {
            Transform.Position = new Vector2D(mouseposition, Transform.Position.Y);
        }
        public void ChangeSpeed(Speed speed)
        {
            switch (speed)
            {
                case Speed.FAST:
                    {
                        speedMultiplier = 3;
                        break;
                    }
                case Speed.NORMAL:
                    {
                        speedMultiplier = 1;
                        break;
                    }
            }
        }

        public double GetReflection(Ball ball)
        {
            double hitx = ball.GetBallCenterX() - (int)paddleRect.X;
            
            if (hitx < 0)
            {
                hitx = 0;
            }
            else if (hitx > (int)paddleRect.Width)
            {
                hitx = (int)paddleRect.Width;
            }
            hitx -= (int)paddleRect.Width / 2.0f;
            return 2.0f * (hitx / ((int)paddleRect.Width / 2.0f));
        }

        public void Update()
        {
            paddleRect.Transform = Transform;
            paddleRect.Update();
        }

        public void Render(IntPtr renderer)
        {
            if (Sprite != null)
            {
                Sprite.Render(renderer, paddleRect.rect);
            }
            else
            {
                paddleRect.Render(renderer);
            }
        }
        #endregion

        #region Enums
        public enum Direction { LEFT, RIGHT, UP, DOWN }
        public enum Speed { NORMAL, FAST }
        public enum Placed { TOP, BOTTOM }
        #endregion
    }
}
