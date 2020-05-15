using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019 { 
    class UISystem : ISystem
    {
        //Singleton
        private UISystem() { }
        private static UISystem instance = null;
        public static UISystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UISystem();
                }
                return instance;
            }
        }

        private List<GameObject> startObjects = new List<GameObject>();
        private List<GameObject> pauseObjects = new List<GameObject>();
        public List<GameObject> backgroundList = new List<GameObject>();

        private List<IEntityComponent> UIElements = new List<IEntityComponent>();
        GameObject buildButton;


        public IEntityComponent CreateStickyUIPosition(Vector2D stickyPos)
        {
            StickyUIPositionComponent pos = new StickyUIPositionComponent(this, stickyPos);
            UIElements.Add(pos);
            return pos;
        }


        public void Init()
        {
            InitStartMenu();
            InitPauseMenu();
            DisableMenu();
        }

        public void InitStartMenu()
        {
            GameObject background = GameObjectSystem.Instance.CreateGameObject("startbackground");
            background.Transform.Size = new Size(2544, 1440);
            background.Transform.Position = new Vector2D(background.Transform.Size.Width / 2 - LevelManager.SCREEN_OFFSET_X, background.Transform.Size.Height / 2 - LevelManager.SCREEN_OFFSET_Y);
            background.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/pixel.png", Layer.UI));
            background.AddComponent(SoundSystem.Instance.CreateBackgroundSound("ravens.mp3"));
            background.AddComponent(InputSystem.Instance.CreateInput(InputType.MENUKEYBOARD));
            startObjects.Add(background);

            GameObject button = GameObjectSystem.Instance.CreateGameObject("button");
            button.Transform.Size = new Size(250, 125);
            button.Transform.Position = new Vector2D(Program.SCREEN_WIDTH / 2, Program.SCREEN_HEIGHT / 2);
            button.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/start.png", Layer.UI));
            button.AddComponent(ButtonSystem.Instance.CreateButton("startbutton"));
            startObjects.Add(button);
        }


        public void InitPauseMenu()
        {
            GameObject blackout = GameObjectSystem.Instance.CreateGameObject("blackout");
            blackout.Transform.Size = new Size(2544, 1440);
            blackout.Transform.Position = new Vector2D(blackout.Transform.Size.Width / 2 - LevelManager.SCREEN_OFFSET_X, blackout.Transform.Size.Height / 2 - LevelManager.SCREEN_OFFSET_Y);
            blackout.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/blackout.png", Layer.UI));
            pauseObjects.Add(blackout);
            backgroundList.Add(blackout);

            GameObject button = GameObjectSystem.Instance.CreateGameObject("Health");
            button.Transform.Size = new Size(150, 60);
            button.Transform.Position = new Vector2D(Program.SCREEN_WIDTH / 2, Program.SCREEN_HEIGHT / 2);
            button.AddComponent(ButtonSystem.Instance.CreateButton("Health"));
            button.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/start.png", Layer.UI));
            pauseObjects.Add(button);

            GameObject button2 = GameObjectSystem.Instance.CreateGameObject("1920");
            button2.Transform.Size = new Size(150, 60);
            button2.Transform.Position = new Vector2D(button.Transform.Position.X, button.Transform.Position.Y + 90);
            button2.AddComponent(ButtonSystem.Instance.CreateButton("1920"));
            button2.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/start.png", Layer.UI));
            pauseObjects.Add(button2);

            GameObject button3 = GameObjectSystem.Instance.CreateGameObject("1280");
            button3.Transform.Size = new Size(150, 60);
            button3.Transform.Position = new Vector2D(button.Transform.Position.X, button.Transform.Position.Y + 180);
            button3.AddComponent(ButtonSystem.Instance.CreateButton("1280"));
            button3.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/start.png", Layer.UI));
            pauseObjects.Add(button3);
            
            GameObject button5 = GameObjectSystem.Instance.CreateGameObject("full");
            button5.Transform.Size = new Size(150, 60);
            button5.Transform.Position = new Vector2D(button.Transform.Position.X, button.Transform.Position.Y + 270);
            button5.AddComponent(ButtonSystem.Instance.CreateButton("full"));
            button5.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/start.png", Layer.UI));
            pauseObjects.Add(button5);
        }


        public void InitIngameUI()
        {
            buildButton = GameObjectSystem.Instance.CreateGameObject("buildBtn");
            buildButton.Transform.Size = new Size(128, 64);
            buildButton.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/paddle.png", Layer.UI));
            buildButton.AddComponent(this.CreateStickyUIPosition(new Vector2D(CameraSystem.cameraRect.w - 96, CameraSystem.cameraRect.h - 64)));
        }


        public void DisableMenu()
        {
            foreach (var pm in pauseObjects)
            {
                if(pm.Active)
                    pm.Active = false;

                //Console.WriteLine("Object: " + pm.Id + " Active: " + pm.Active);
            }

            foreach (var pm in startObjects)
            {
                if(pm.Active)
                    pm.Active = false;

                //Console.WriteLine("StartObjects: " + "Object: " + pm.Id + " Active: " + pm.Active);
            }
        }

        public void DisableUI()
        {
            if(buildButton.Active)
                buildButton.Active = false;
        }

        public void EnableUI()
        {
            if(!buildButton.Active)
                buildButton.Active = true;
        }

        public void ShowStartMenu()
        {
            foreach (var pm in startObjects)
            {
                pm.Active = true;
            }
        }

        public void ShowPauseMenu()
        {
            foreach (var pm in pauseObjects)
            {
                pm.Active = true;

                int x = CameraSystem.Instance.getCamera().x + Program.SCREEN_WIDTH / 2;
                int y = CameraSystem.Instance.getCamera().y + Program.SCREEN_WIDTH / 7;
                if (pm.Id == "Mute")
                {
                    pm.Transform.Position = new Vector2D(x, y);
                }
                if (pm.Id == "1920")
                {
                    pm.Transform.Position = new Vector2D(x, y + 90);
                }
                if (pm.Id == "1280")
                {
                    pm.Transform.Position = new Vector2D(x, y + 180);

                }
                if (pm.Id == "full")
                {
                    pm.Transform.Position = new Vector2D(x, y + 270);

                }
            }
        }
        

        public void UpdateBackground()
        {
            foreach (var bg in backgroundList)
            {
                bg.Transform.Position = new Vector2D(bg.Transform.Size.Width / 2 - LevelManager.SCREEN_OFFSET_X, bg.Transform.Size.Height / 2 - LevelManager.SCREEN_OFFSET_Y);
            }
        }

        public void Update()
        {
            foreach (var element in UIElements)
            {
                element.GameObject.Transform.Position = new Vector2D(element.GameObject.GetComponent<StickyUIPositionComponent>().UIPos.X + CameraSystem.cameraRect.x,
                                                                     element.GameObject.GetComponent<StickyUIPositionComponent>().UIPos.Y + CameraSystem.cameraRect.y);
            }
        }
    }
}
