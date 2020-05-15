using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace GPP_2019
{
    class Spritesheet : Sprite
    {
        #region Attributes and Properties
        public int spritePointer = 0;
        public int animationPointer = 0;
        public SDL_Rect sprite_Rect;
        private bool isSpriteFlipped = true;
        public bool IsSpriteFlipped { get { return isSpriteFlipped; } set { isSpriteFlipped = value; } }
        #endregion

        #region Constructors
        public Spritesheet(IntPtr renderer, string filePath, Vector2D spriteSize):base(renderer, filePath)
        {
            sprite_Rect.x = 0;
            sprite_Rect.y = 0;
            sprite_Rect.w = (int)spriteSize.X;
            sprite_Rect.h = (int)spriteSize.Y;
        }

        #endregion

        #region Methodes
        public override void Render(IntPtr renderer, SDL_Rect rect)
        {
            sprite_Rect.x = sprite_Rect.w * spritePointer;

            SDL_RenderCopy(renderer, SpritePtr, ref sprite_Rect, ref rect);
        }

        public override void Render(IntPtr renderer, SDL_Rect rect, double degree)
        {
            SDL_Point point;
            point.x = rect.w / 2;
            point.y = rect.h / 2;

            sprite_Rect.x = sprite_Rect.w * spritePointer ;
            sprite_Rect.y = sprite_Rect.h * animationPointer;

            if (isSpriteFlipped)
                SDL_RenderCopyEx(renderer, SpritePtr, ref sprite_Rect, ref rect, degree, ref point, SDL_RendererFlip.SDL_FLIP_HORIZONTAL);
            else
                SDL_RenderCopyEx(renderer, SpritePtr, ref sprite_Rect, ref rect, degree, ref point, SDL_RendererFlip.SDL_FLIP_NONE);
        }
        #endregion

    }
}
