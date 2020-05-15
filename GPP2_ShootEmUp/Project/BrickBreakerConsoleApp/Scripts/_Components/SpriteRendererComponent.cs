using System;
using System.ComponentModel;
using static SDL2.SDL;

namespace BrickBreakerConsoleApp
{
    internal class SpriteRendererComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }
        public Rectangle Rectangle { get; set; }
        public Sprite Sprite { get; set; }
        public Type Type { get; set; } = Type.RENDER;
        public bool BAnimated { get; set; } = false;
        public int Frames { get; set; } = 0;
        public int Speed { get; set; } = 100;
        public bool[] animationState = { false, false, false, false };

        public SpriteRendererComponent(ISystem system, Sprite sprite)
        {
            System = system;
            Sprite = sprite;
        }

        public SpriteRendererComponent(ISystem system, Spritesheet spritesheet, int nFrames, int mSpeed)
        {
            BAnimated = true;
            Frames = nFrames;
            Speed = mSpeed;
            System = system;
            Sprite = spritesheet;
        }
    }
}