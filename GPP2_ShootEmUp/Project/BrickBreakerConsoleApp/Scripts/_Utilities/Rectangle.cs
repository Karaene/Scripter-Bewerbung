using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace BrickBreakerConsoleApp
{
    class Rectangle : IRenderable
    {
        #region Attributes and Properties
        public SDL_Rect rect;

        public Transform Transform { get; set; }
        public Color Color { get; set; }
        public Sprite Sprite { get; set; }

        
        public double X {   get {   return Transform.Position.X; }
                            set {   Transform.Position = new Vector2D(value, Transform.Position.Y);
                                    rect.x = (int)Transform.Position.X; } }

        public double Y {   get {   return Transform.Position.Y; }
                            set {   Transform.Position = new Vector2D(Transform.Position.X, value);
                                    rect.y = (int)Transform.Position.Y; } }

        public int Width {  get { return (int)Transform.Size.Width; }
                            set { Transform.Size = new Size(value, Transform.Size.Height);
                                  rect.w = (int)Transform.Size.Width; } }

        public int Height { get { return (int)Transform.Size.Height; }
                            set { Transform.Size = new Size(Transform.Size.Width, value); } }
        #endregion

        #region Constructors
        public Rectangle(Transform transform)
        {
            Transform = transform;

            rect = new SDL_Rect();

            rect.x = (int)Transform.Position.X;
            rect.y = (int)Transform.Position.Y;
            rect.w = (int)Transform.Size.Width;
            rect.h = (int)Transform.Size.Height;
            
        }

        public Rectangle(Vector2D position, Size dimension, Color color)
        {
            Transform = new Transform(position, dimension);

            rect = new SDL_Rect();

            rect.x = (int) Transform.Position.X;
            rect.y = (int) Transform.Position.Y;
            rect.w = (int) Transform.Size.Width;
            rect.h = (int) Transform.Size.Height;

            Color = color;
        }

        public Rectangle(Vector2D position, Size dimension, Sprite sprite)
        {
            Transform = new Transform(position, dimension);

            rect = new SDL_Rect();

            rect.x = (int)Transform.Position.X;
            rect.y = (int)Transform.Position.Y;
            rect.w = (int)Transform.Size.Width;
            rect.h = (int)Transform.Size.Height;

            Sprite = sprite;
        }

        public Rectangle(Vector2D position, Size dimension)
        {
            Transform = new Transform(position, dimension);

            rect = new SDL_Rect();

            rect.x = (int) Transform.Position.X;
            rect.y = (int) Transform.Position.Y;
            rect.w = (int) Transform.Size.Width;
            rect.h = (int) Transform.Size.Height;
            
            Color = new Color(255, 255, 255);
        }
        #endregion

        #region Methodes
        public bool CollisionDetection(Sphere sphere)
        {
            if (sphere.GetPosY() + sphere.GetRadius() > Transform.Position.Y && sphere.GetPosY() - sphere.GetRadius() < (Transform.Position.Y + Transform.Size.Height) && 
                sphere.GetPosX() + sphere.GetRadius() > Transform.Position.X && sphere.GetPosX() - sphere.GetRadius() < (Transform.Position.X + Transform.Size.Width)) // 9
            {
                return true;
            }
            
            return false;
        }

        public void Update()
        {
            rect.x = (int)Transform.Position.X;
            rect.y = (int)Transform.Position.Y;
            rect.w = (int)Transform.Size.Width;
            rect.h = (int)Transform.Size.Height;
        }

        public void Render(IntPtr renderer)
        {
            SDL_SetRenderDrawColor(renderer, Color.R, Color.G, Color.B, 0);
            SDL_RenderFillRect(renderer, ref rect);
        }
        #endregion
    }
}
