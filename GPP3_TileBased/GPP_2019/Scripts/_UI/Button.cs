using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class Button : IRenderable , IUpdateable
    {
        private Rectangle buttonRect;
        public Transform Transform { get; set; }
        public Sprite Sprite { get; set; }
        public Color Color { get; set; }
        public bool clicked = false;
        
        public Button()
        {
            Color = new Color(100,100,100);
            buttonRect = new Rectangle(new Vector2D(), new Size(), Color);
            Transform = new Transform(new Vector2D(), new Size());
        }

        public Button(int x, int y, int width, int height, Color color)
        {
            buttonRect = new Rectangle(new Vector2D(x - width / 2, y), new Size(width, height), color);
            Transform = new Transform(new Vector2D(x - width / 2, y), new Size(width, height));
        }

        public Button(int x, int y, int width, int height, Sprite sprite)
        {
            Sprite = sprite;
            buttonRect = new Rectangle(new Vector2D(x - width / 2, y), new Size(width, height));
            Transform = new Transform(new Vector2D(x - width / 2, y), new Size(width, height));
        }

        public void OnClick(Vector2D mousePos)
        {
            if (mousePos.X <= Transform.Position.X + Transform.Size.Width && mousePos.X >= Transform.Position.X &&
                mousePos.Y <= Transform.Position.Y + Transform.Size.Height && mousePos.Y >= Transform.Position.Y)
            {
                clicked = true;
                Console.WriteLine("button clicked: " + clicked);
            }
        }

        public void Update()
        {
            buttonRect.Color = Color;
            buttonRect.Transform = Transform;
            buttonRect.Update();
        }
        
        public void Render(IntPtr renderer)
        {
            if (Sprite != null)
            {
                Sprite.Render(renderer, buttonRect.rect);
            }
            else
            {
                buttonRect.Render(renderer);
            }
            
        }
    }
}
