using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static SDL2.SDL;
using System.Threading.Tasks;
using SDL2;

namespace BrickBreakerConsoleApp
{
    class Text
    {
        #region Attributes and Properties
        public SDL_Rect Message_rect;
        IntPtr Message;
        IntPtr font;
        String text;
        int width;
        int height;
        SDL_Color color = new SDL_Color();
        public int W { get { return width; } }
        public int H { get { return height; } }
        #endregion

        #region Constructors
        public Text(int posx, int posy, int width, int height)
        {
            this.width = width;
            this.height = height;
            Message_rect.x = posx;  //controls the rect's x coordinate 
            Message_rect.y = posy; // controls the rect's y coordinte
            Message_rect.w = width; // controls the width of the rect
            Message_rect.h = height; // controls the height of the rect
            InitText();
            SetFont();
        }
        #endregion

        #region Methodes
        void InitText()
        {
            //Initialize SDL_ttf
            if (SDL_ttf.TTF_Init() == -1)
            {
                Console.WriteLine("Unable to initialize SDL Text. Error: {0}", SDL_GetError());
            }
        }

        void SetFont()
        {
            //Set Font Style
            font = SDL_ttf.TTF_OpenFont("arial.ttf", 100);
            if (font == IntPtr.Zero)
            {
                Console.WriteLine("Unable to find Font");
            }
        }

        public void DeleteOldText()
        {

        }

        public void SetColor(byte r, byte g, byte b)
        {
            color.r = r;
            color.g = g;
            color.b = b;
            color.a = 0;
        }

        public void SetText(String textmessage)
        {
            text = textmessage;
        }

        public void Render(IntPtr renderer)
        {
            IntPtr surfaceMessage = SDL_ttf.TTF_RenderText_Solid(font, text, color);
            Message = SDL_CreateTextureFromSurface(renderer, surfaceMessage);
            SDL_FreeSurface(surfaceMessage);
            if (Message == IntPtr.Zero)
            {
                Console.WriteLine("Message is Empty");
            }
            SDL_RenderCopy(renderer, Message, IntPtr.Zero, ref Message_rect);
            SDL_DestroyTexture(Message);
        }
        #endregion
    }
}
