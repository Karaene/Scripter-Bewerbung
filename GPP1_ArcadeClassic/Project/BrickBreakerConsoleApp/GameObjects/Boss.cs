using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace BrickBreakerConsoleApp
{
    class Boss : IRenderable, IUpdateable, ICollideable
    {
        #region Attributes and Properties
        public Ball ball;
        public Rectangle bossRect;
        public const int START__BOSS_LIFE = 100;
        public int bossLife;
        public Transform Transform { get; set; }
        public Sprite Sprite { get; set; }

        #endregion

        #region Constructors

        public Boss(int x, int y, int width, int height, Sprite spriteBoss, int bossLife, Ball ball)
        {
            this.ball = ball;
            this.bossLife = bossLife;
            Sprite = spriteBoss;
            bossRect = new Rectangle(new Vector2D(x, y), new Dimension(width, height));
            Transform = new Transform(new Vector2D(x, y), new Dimension(width, height));
        }
        #endregion

        #region Methodes
        public int GetLive()
        {
            return bossLife;
        }

        public int TakeDamage()
        {
            return bossLife = bossLife - ball.damage;
        }

        
        public void Update()
        {
            bossRect.Transform = Transform;
            bossRect.Update();
            //bossLife = bossLife;
        }
        

        public void Render(IntPtr renderer)
        {
            if (Sprite != null)
            {
                if(bossLife >= 1)
                    Sprite.Render(renderer, bossRect.rect);
                else
                {
                    bossRect.Render(renderer);
                }
            }
            else
            {
                bossRect.Render(renderer);
            }
        }
        #endregion
    }
}
