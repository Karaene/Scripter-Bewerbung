using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace GPP_2019
{
    class Sprite
    {
        #region Attributes and Properties
        public IntPtr SpritePtr { get; set; }
        public SDL_Rect source_Rect;
        #endregion

        #region Constructors
        public Sprite(IntPtr renderer, string filePath)
        {
            int i;
            uint j;
            SpritePtr = IMG_LoadTexture(renderer, filePath);
            Console.Out.WriteLine(SDL_GetError());
            SDL_QueryTexture(SpritePtr, out j, out i, out source_Rect.w, out source_Rect.h);
            source_Rect.x = 0;
            source_Rect.y = 0;
        }
    
        public IntPtr createSprite(IntPtr renderer, string texture, int id)
        {
            IntPtr tempSurface = IMG_Load(texture);
            IntPtr tex = SDL_CreateTextureFromSurface(renderer, tempSurface);
            SDL_FreeSurface(tempSurface);
            return tex;
        }
        #endregion

        #region Methodes


        public virtual void Render(IntPtr renderer, SDL_Rect rect)
        {
            SDL_RenderCopy(renderer, SpritePtr, ref source_Rect, ref rect);
        }
        public virtual void Render(IntPtr renderer, SDL_Rect rect, double degree)
        {
            SDL_Point point;
            point.x = rect.w/2;
            point.y = rect.h/2;
            SDL_RenderCopyEx(renderer, SpritePtr, ref source_Rect, ref rect, degree, ref point, SDL_RendererFlip.SDL_FLIP_NONE);
        }
        #endregion

    }
}
