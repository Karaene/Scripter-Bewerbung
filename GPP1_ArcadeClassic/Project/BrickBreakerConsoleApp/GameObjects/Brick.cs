using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class Brick : IRenderable, ICollideable
    {
        Sprite[] sprites;
        private Rectangle brickRect;
        public Transform Transform { get; set; }

        private int lives;
        private string[] imagePaths = { "brick1.png", "brick2.png", "brick3.png" };

        public Brick(IntPtr renderer, Vector2D pos, Dimension size, int lives)
        {
            if (lives > 3)
            {
                throw new ArgumentOutOfRangeException("Not more than 3 lives supported!");
            }

            this.lives = lives;
            sprites = new Sprite[lives];

            for (int i = 0; i < lives; i++)
            {
                sprites[i] = new Sprite(renderer, imagePaths[i]);
            }

            brickRect = new Rectangle(pos, size);
            Transform = new Transform(pos, size);
        }
        /*
        public bool CollisionDetection(Ball ball)
        {
            return brickRect.CollisionDetection(ball.GetSphere());
        }
        */
        public int LoseLive()
        {
            return --lives;
        }
        
        public void Render(IntPtr renderer)
        {
            sprites[lives - 1].Render(renderer, brickRect.rect);
        }
    }
}
