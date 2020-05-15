using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace BrickBreakerConsoleApp
{
    class Sprite
    {
        #region Attributes and Properties
        IntPtr sprite;
        SDL_Rect source_Rect;
        #endregion

        #region Constructors
        public Sprite(IntPtr renderer, string filePath)
        {
            int i;
            uint j;
            sprite = IMG_LoadTexture(renderer, filePath);
            Console.Out.WriteLine(SDL_GetError());
            SDL_QueryTexture(sprite, out j, out i, out source_Rect.w, out source_Rect.h);
            source_Rect.x = 0;
            source_Rect.y = 0;
        }
        #endregion

        #region Methodes
        public void Render(IntPtr renderer, SDL_Rect rect)
        {
            SDL_RenderCopy(renderer, sprite, ref source_Rect, ref rect);
        }
        #endregion

    }
}
