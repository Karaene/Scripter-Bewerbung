using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class ButtonSystem : ISystem
    {
        InputType inputType;
        Button button { get; set; }
        Sprite sprite { get; set; }
        Vector2D mousePosition;
        public bool clickTried = false;
        public bool clicked;
        private List<ButtonComponent> activeButtons = new List<ButtonComponent>();


        private static ButtonSystem instance = null;
        public static ButtonSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ButtonSystem();
                }
                return instance;
            }
        }
        public void initEvent()
        {
            EventSystem.Instance.AddListener("ButtonClick", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) =>
                {
                    mousePosition = (Vector2D)parameters[0];
                    mousePosition = new Vector2D(mousePosition.X + CameraSystem.cameraRect.x , mousePosition.Y + CameraSystem.cameraRect.y);
                    clickTried = true;
                    inputType = InputType.KEYBOARD;
                }
            });

            EventSystem.Instance.AddListener("ButtonStop", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) =>
                {
                    clickTried = false;
                    clicked = false;
                    inputType = InputType.KEYBOARD;
                }
            });
        }
        internal IEntityComponent CreateButton(String name)
        {
            ButtonComponent bc = new ButtonComponent(this, name);
            activeButtons.Add(bc);
            return bc;
        }


        private void OnClick(Vector2D mousePos)
        {
            foreach (var bc in activeButtons)
            {
                double minX = bc.GameObject.Transform.Position.X - bc.GameObject.Transform.Size.Width / 2;
                double maxX = bc.GameObject.Transform.Position.X + bc.GameObject.Transform.Size.Width / 2;
                double maxY = bc.GameObject.Transform.Position.Y - bc.GameObject.Transform.Size.Height / 2;
                double minY = bc.GameObject.Transform.Position.Y + bc.GameObject.Transform.Size.Height / 2;

                if (mousePos.X >= minX && mousePos.X <= maxX
                    && mousePos.Y >= maxY && mousePos.Y <= minY)
                {

                    clicked = true;
                    ButtonReaction(bc);

                }
            }
        }

        private void ButtonReaction(ButtonComponent button)
        {
            if (button.Name == "Mute")
            {
                //Console.WriteLine("CLICKED MUTE");

                SoundSystem.Instance.stopMusic();
            }
            if(button.Name == "1920")
            {
                Program.SCREEN_WIDTH = 1920;
                Program.SCREEN_HEIGHT = 1080;
                LevelManager.Instance.Update();
                RenderSystem.Instance.ChangeWindowSize(1920,1080);
                Program.ScreenChanged = true;
            }
            if (button.Name == "1280")
            {
                Program.SCREEN_WIDTH = 1280;
                Program.SCREEN_HEIGHT = 720;
                LevelManager.Instance.Update();
                RenderSystem.Instance.ChangeWindowSize(1280, 720);
                Program.ScreenChanged = true;

            }
            if (button.Name == "full")
            {
                Program.SCREEN_WIDTH = 1920;
                Program.SCREEN_HEIGHT = 1080;
                LevelManager.Instance.Update();
                //RenderSystem.Instance.ChangeWindowSize(1920, 1080);
                RenderSystem.Instance.SetFullScreen();
                Program.ScreenChanged = true;

            }
            if(button.Name == "window")
            {
                LevelManager.Instance.Update();
                RenderSystem.Instance.SetWindowMode();
                Program.ScreenChanged = true;
            }
        }

        public void Update()
        {
            if (clickTried)
            {
                //Console.WriteLine(mousePosition);
                OnClick(mousePosition);
            }
        }
    }
}
