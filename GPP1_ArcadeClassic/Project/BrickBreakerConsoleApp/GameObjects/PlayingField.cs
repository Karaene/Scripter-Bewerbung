using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace BrickBreakerConsoleApp
{
    class PlayingField : IRenderable
    {
        #region Attributes and Properties

        private static Color _BackgroundColor;
        private Rectangle fieldRect;

        //public Transform Transform { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Sprite Background { get; set; }
        #endregion

        #region Constructors
        public PlayingField(int width, int height, Sprite background)
        {
            Width = width;
            Height = height;
            fieldRect = new Rectangle(new Vector2D(0, 0), new Dimension(width, height));
            //Transform = new Transform(new Vector2D(height, 0), new Dimension(width, height));
            Background = background;
        }

        public PlayingField(int width, int height, Color color)
        {
            Width = width;
            Height = height;
            _BackgroundColor = color; 
        }
        #endregion

        #region Methodes
        public void Render(IntPtr renderer)
        {
            //Set Background Color
            SDL_SetRenderDrawColor(renderer, _BackgroundColor.R, _BackgroundColor.G, _BackgroundColor.B, 255);
            SDL_RenderClear(renderer);

            if (Background != null)
            {
                Background.Render(renderer, fieldRect.rect);
            }
            else
            {
                fieldRect.Render(renderer);
            }

            //Rectangle rect = new Rectangle(new Vector2D(), new Vector2D(width, height));
            //background.Render(renderer, rect.rect);
        }
        #endregion
    }
}
