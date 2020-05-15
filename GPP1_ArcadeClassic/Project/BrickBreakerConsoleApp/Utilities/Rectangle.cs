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

        public int Width {  get { return (int)Transform.Dimension.Width; }
                            set { Transform.Dimension = new Dimension(value, Transform.Dimension.Height);
                                  rect.w = (int)Transform.Dimension.Width; } }

        public int Height { get { return (int)Transform.Dimension.Height; }
                            set { Transform.Dimension = new Dimension(Transform.Dimension.Width, value); } }
        #endregion

        #region Constructors
        public Rectangle(Vector2D position, Dimension dimension, Color color)
        {
            Transform = new Transform(position, dimension);

            rect = new SDL_Rect();

            rect.x = (int) Transform.Position.X;
            rect.y = (int) Transform.Position.Y;
            rect.w = (int) Transform.Dimension.Width;
            rect.h = (int) Transform.Dimension.Height;

            Color = color;
        }

        public Rectangle(Vector2D position, Dimension dimension, Sprite sprite)
        {
            Transform = new Transform(position, dimension);

            rect = new SDL_Rect();

            rect.x = (int)Transform.Position.X;
            rect.y = (int)Transform.Position.Y;
            rect.w = (int)Transform.Dimension.Width;
            rect.h = (int)Transform.Dimension.Height;

            Sprite = sprite;
        }

        public Rectangle(Vector2D position, Dimension dimension)
        {
            Transform = new Transform(position, dimension);

            rect = new SDL_Rect();

            rect.x = (int) Transform.Position.X;
            rect.y = (int) Transform.Position.Y;
            rect.w = (int) Transform.Dimension.Width;
            rect.h = (int) Transform.Dimension.Height;
            
            Color = new Color(255, 255, 255);
        }
        #endregion

        #region Methodes
        public bool CollisionDetection(Sphere sphere)
        {
            /*
            if (sphere.GetPosY() + sphere.GetRadius() < MinPosY() && sphere.GetPosX()  > MinPosX() && sphere.GetPosX() < MaxPosX()) // 1
            {
                return true;
            }

            if (sphere.GetPosY() > MinPosY() && sphere.GetPosY() < MaxPosY() && sphere.GetPosX() > MaxPosX() + sphere.GetRadius()) // 3
            {
                return true;
            }

            if (sphere.GetPosY() > MaxPosY() + sphere.GetRadius() && sphere.GetPosX() > MinPosX() && sphere.GetPosX() < MaxPosX()) // 5
            {
                return true;
            }

            if (sphere.GetPosY() > MinPosY() && sphere.GetPosY() < MaxPosY() && sphere.GetPosX() < MinPosX() - sphere.GetRadius()) // 7
            {
                return true;
            }

            if (sphere.GetPosY() < MinPosY()  && sphere.GetPosX() > MaxPosX()) // 2
            {
                return true;
            }

            if (sphere.GetPosY() > MaxPosY() && sphere.GetPosX() > MaxPosX()) // 4
            {
                return true;
            }

            if (sphere.GetPosY() > MaxPosY() && sphere.GetPosX() < MinPosX()) // 6
            {
                return true;
            }

            if (sphere.GetPosY() < MinPosY() && sphere.GetPosX() < MinPosX()) // 8
            {
                return true;
            }*/
            
            if (sphere.GetPosY() + sphere.GetRadius() > Transform.Position.Y && sphere.GetPosY() - sphere.GetRadius() < (Transform.Position.Y + Transform.Dimension.Height) && 
                sphere.GetPosX() + sphere.GetRadius() > Transform.Position.X && sphere.GetPosX() - sphere.GetRadius() < (Transform.Position.X + Transform.Dimension.Width)) // 9
            {
                return true;
            }
            
            return false;
        }

        public void Update()
        {
            rect.x = (int)Transform.Position.X;
            rect.y = (int)Transform.Position.Y;
            rect.w = (int)Transform.Dimension.Width;
            rect.h = (int)Transform.Dimension.Height;
        }

        public void Render(IntPtr renderer)
        {
            SDL_SetRenderDrawColor(renderer, Color.R, Color.G, Color.B, 0);
            SDL_RenderFillRect(renderer, ref rect);
        }
        #endregion
    }
}
