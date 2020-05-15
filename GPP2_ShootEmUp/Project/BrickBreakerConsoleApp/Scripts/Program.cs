using System;
using System.Collections.Generic;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace BrickBreakerConsoleApp
{
    class Program
    {
        #region Attributes and Properties
        int tileSize = 48;
        GameObject[,] tiles = new GameObject[30, 53];
        #region tilemap
        int[,] tilemap = {
                    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,0},
                    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,0},
                    { 0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0,1,1,1,0,0,0,0,0,0,0,1,0},
                    { 0,0,0,0,0,1,1,0,0,0,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0},
                    { 0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0},
                    { 0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0},
                    { 0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                    { 0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                    { 0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0},
                    { 0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0},
                    { 0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                    { 0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,0},
                    { 0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0},
                    { 0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,0,0},
                    { 0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1},
                    { 0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1},
                    { 0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1},
                    { 0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1},
                    { 0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0},
                    { 0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,1,1,0,0,0,1,0},
                    { 0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,1,1,1,0,0,0,1,1},
                    { 0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,1,1,1,1,0,1,0,0,0,0,1},
                    { 0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,1,1,1,0,0,0,1,1},
                    { 0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,1,0},
                    { 0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,1,0,0,1,0,0,0,0,0,1,1},
                    { 0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,1,0,1,1,0,0,0,0,0,0,1},
                    { 0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,1,0,1,0,0,0,0,0,0,0,1},
                    { 0,0,0,1,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1,0,0,0,1,0,1,1,1,1,0,1,0,0,0,0,0,0,1,1},
                    { 0,0,0,1,1,1,1,1,1,0,0,0,1,1,0,0,0,0,0,0,0,0,1,0,0,1,1,0,0,0,0,0,0,1,1,0,0,1,0,0,0,0,0,0,1,1,1,0,1,1,1,1,0},
                    { 0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0}
        };
        #endregion
        private const float MS_PER_UPDATE = 16;


        public static int SCREEN_WIDTH { get; set; } = 1440;
        public static  bool ScreenChanged { get; set; } = false;
        public static int SCREEN_HEIGHT { get; set; } = 800;
        private static float _Elapsed = 0;

        private Utils _Utilities = new Utils();
        Vector2D playerpos;

        private List<GameObject> startObjects = new List<GameObject>();
        private List<GameObject> pauseObjects = new List<GameObject>();
        private List<GameObject> endedObjects = new List<GameObject>();
        private List<String> highscores = new List<string>();
        private List<GameObject> scores = new List<GameObject>();

        private List<GameObject> backgroundList = new List<GameObject>();
        private List<GameObject> PlayerList = new List<GameObject>();
        private List<GameObject> PointList = new List<GameObject>();

        public bool IsRunning { get; set; }
        private bool paused;
        private bool pausedTemp = true;
        bool gameover = false;
        bool gameOverTemp = false;
        GameObject enemyShooting;

        private static Program instance = null;
        #endregion

        #region Methodes
        public static Program Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Program();
                }
                return instance;
            }
        }
        static void Main(string[] args)
        {
            Program program = new Program();
            program.StartScreen();

            SDL_Quit();
            return;
        }

        private void StartScreen()
        {
            IsRunning = true;
            float previous = SDL_GetTicks();
            float lag = 0.0f;

            AddEventListeners();
            ButtonSystem.Instance.initEvent();
            InitStartMenue();

            while (IsRunning)
            {
                float current = SDL_GetTicks();
                _Elapsed = current - previous;
                previous = current;
                lag += _Elapsed;

                while (lag >= MS_PER_UPDATE)
                {
                    InputSystem.Instance.UpdateMenue();
                    ButtonSystem.Instance.Update();
                    if (ButtonSystem.Instance.clicked)
                    {
                        UnShowMenu(startObjects);
                        Run();
                    }
                    lag -= MS_PER_UPDATE;
                }
                RenderSystem.Instance.Render();
            }
        }

        private void InitEndMenue()
        {

            GameObject background = GameObjectSystem.Instance.CreateGameObject("endbackground");
            background.Transform.Size = new Size(2544, 1440);
            background.Transform.Position = new Vector2D(background.Transform.Size.Width / 2 - LevelManager.SCREEN_OFFSET_X, background.Transform.Size.Height / 2 - LevelManager.SCREEN_OFFSET_Y);
            background.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/redout.png"));
            background.AddComponent(InputSystem.Instance.CreateInput(InputType.MENUKEYBOARD));
            endedObjects.Add(background);
            backgroundList.Add(background);
            background.Active = false;

            GameObject highscoretext = GameObjectSystem.Instance.CreateGameObject("highscoretext");
            highscoretext.Transform.Size = new Size(440, 150);
            highscoretext.Transform.Position = new Vector2D(SCREEN_WIDTH / 2, (SCREEN_HEIGHT / 2) - 180);
            highscoretext.AddComponent(RenderSystem.Instance.CreateTextComponent());
            highscoretext.GetComponent<TextComponent>().Text.SetText("Your Points: " + PointSystem.Instance.points + PointSystem.Instance.EaseResult);
            highscoretext.GetComponent<TextComponent>().Text.SetColor(0, 0, 0);
            highscoretext.Active = false;
            endedObjects.Add(highscoretext);


            GameObject highscore = GameObjectSystem.Instance.CreateGameObject("highscore");
            highscore.Transform.Size = new Size(440,150);
            highscore.Transform.Position = new Vector2D(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 + 20);
            highscore.AddComponent(RenderSystem.Instance.CreateTextComponent());
            PointSystem.Instance.GetHighScores();
            highscore.GetComponent<TextComponent>().Text.SetText("Highscore: " + "  " + PointSystem.Instance.GetScore() + "  " + PointSystem.Instance.GetTime());
            highscore.GetComponent<TextComponent>().Text.SetColor(0, 0, 0);
            highscore.Active = false;
            endedObjects.Add(highscore);

            /*
            GameObject button = GameObjectSystem.Instance.CreateGameObject("endbutton");
            button.Transform.Size = new Size(250, 125);
            button.Transform.Position = new Vector2D(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);
            button.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/StartButton.png"));
            button.AddComponent(ButtonSystem.Instance.CreateButton("startbutton"));
            button.Active = false;
            endedObjects.Add(button);
            */


        }
        private void InitStartMenue()
        {
            GameObject background = GameObjectSystem.Instance.CreateGameObject("startbackground");
            background.Transform.Size = new Size(2544, 1440);
            background.Transform.Position = new Vector2D(background.Transform.Size.Width / 2 - LevelManager.SCREEN_OFFSET_X, background.Transform.Size.Height / 2 - LevelManager.SCREEN_OFFSET_Y);
            background.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/pixel.png"));
            background.AddComponent(InputSystem.Instance.CreateInput(InputType.MENUKEYBOARD));
            startObjects.Add(background);

            GameObject button = GameObjectSystem.Instance.CreateGameObject("button");
            button.Transform.Size = new Size(250, 125);
            button.Transform.Position = new Vector2D(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);
            button.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/StartButton.png"));
            button.AddComponent(ButtonSystem.Instance.CreateButton("startbutton"));
            startObjects.Add(button);
        }
        private void Run()
        {
            //Start FPS Timer
            int countedFrames = 0;
            _Utilities.StartTimer();

            SoundSystem.Instance.Init();

            BuildGameObjects();
            AddEventListeners();

            HealthSystem.Instance.Init();
            FightSystem.Instance.Init();
            TweenSystem.Instance.Init();
            PlayerShootingSystem.Instance.Init();
            SpawnSystem.Instance.Init();
            PointSystem.Instance.Init();
            CollisionHandlerSystem.Instance.AddListeners();

            InitPauseMenu();
            InitEndMenue();
            float previous = SDL_GetTicks();
            float lag = 0.0f;


            IsRunning = true;

            while (IsRunning)
            {               
                float current = SDL_GetTicks();
                _Elapsed = current - previous;
                previous = current;
                lag += _Elapsed;
                while (lag >= MS_PER_UPDATE)
                {
                    if (!gameover)
                    {
                        if (ScreenChanged)
                        {
                            UpdateBackground();
                            UpdateTiles();
                            //UpdatePlayer();
                            CameraSystem.Instance.Update();
                            ShowPauseMenu();
                            UpdatePoint();
                            ScreenChanged = false;
                        }

                        if (!paused)
                        {
                            if (!checkCollision())
                            {
                                playerpos = PlayerControl.Instance.Player.GameObject.Transform.Position;
                            }

                            checkCollision();
                            SpawnSystem.Instance.Update();
                            FightSystem.Instance.Update();
                            PhysicSystem.Instance.Update();
                            PlayerControl.Instance.Update();
                            MovementSystem.Instance.Update();
                            CameraSystem.Instance.Update();
                            PlayerShootingSystem.Instance.Update();
                            BotControl.Instance.Update(_Elapsed);
                            TweenSystem.Instance.Update();
                            PointSystem.Instance.Update();
                            HealthSystem.Instance.Update();
                            pausedTemp = true;
                            UnShowMenu(pauseObjects);

                        }
                        else
                        {
                            //call Menue
                            if (pausedTemp)
                            {
                                ShowPauseMenu();

                                pausedTemp = false;
                            }
                            InputSystem.Instance.UpdateMenue();
                            ButtonSystem.Instance.Update();
                        }
                    }
                    else
                    {
                        InputSystem.Instance.UpdateMenue();

                        //ButtonSystem.Instance.Update();
                        if (!gameOverTemp)
                        {
                            PointSystem.Instance.SaveScore();
                           // highscores = PointSystem.Instance.getScores();
                            ShowEndMenu();
                            gameOverTemp = true;
                        }
                        if (ButtonSystem.Instance.clicked == true)
                        {
                            gameover = false;
                            UnShowMenu(endedObjects);
                        }
                    }

                lag -= MS_PER_UPDATE;
                }              
                RenderSystem.Instance.Render();
                SoundSystem.Instance.Update();
                ++countedFrames;
            }
            PointSystem.Instance.SaveScore();

        }

        public void UnShowMenu(List<GameObject> menueObjects)
        {
            foreach (var pm in menueObjects)
            {
                pm.Active = false;
            }
        }

        public void ShowEndMenu()
        {
            foreach (var pm in endedObjects)
            {
                pm.Active = true;
                int x = CameraSystem.Instance.getCamera().x + Program.SCREEN_WIDTH / 2;
                int y = CameraSystem.Instance.getCamera().y + Program.SCREEN_WIDTH / 4;
                if (pm.Id == "endbutton")
                {
                    pm.Transform.Position = new Vector2D(x, y);
                }
                if(pm.Id == "highscoretext")
                {
                    pm.GetComponent<TextComponent>().Text.SetText("Your Points: " + PointSystem.Instance.EaseResult);
                }
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
                if (pm.Id == "window")
                {
                    pm.Transform.Position = new Vector2D(x, y + 360);
                }

            }
        }


        public void InitPauseMenu()
        {
            GameObject blackout = GameObjectSystem.Instance.CreateGameObject("blackout");
            blackout.Transform.Size = new Size(2544, 1440);
            blackout.Transform.Position = new Vector2D(blackout.Transform.Size.Width / 2 - LevelManager.SCREEN_OFFSET_X, blackout.Transform.Size.Height / 2 - LevelManager.SCREEN_OFFSET_Y);
            blackout.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/blackout.png"));
            pauseObjects.Add(blackout);
            backgroundList.Add(blackout);

            GameObject button = GameObjectSystem.Instance.CreateGameObject("Mute");
            button.Transform.Size = new Size(150, 60);
            button.Transform.Position = new Vector2D(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);
            button.AddComponent(ButtonSystem.Instance.CreateButton("Mute"));
            button.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/SoundButton.png"));
            pauseObjects.Add(button);

            GameObject button2 = GameObjectSystem.Instance.CreateGameObject("1920");
            button2.Transform.Size = new Size(150, 60);
            button2.Transform.Position = new Vector2D(button.Transform.Position.X, button.Transform.Position.Y + 90);
            button2.AddComponent(ButtonSystem.Instance.CreateButton("1920"));
            button2.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/FullHDButton.png"));
            pauseObjects.Add(button2);

            GameObject button3 = GameObjectSystem.Instance.CreateGameObject("1280");
            button3.Transform.Size = new Size(150, 60);
            button3.Transform.Position = new Vector2D(button.Transform.Position.X, button.Transform.Position.Y + 180 );
            button3.AddComponent(ButtonSystem.Instance.CreateButton("1280"));
            button3.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/HDButton.png"));
            pauseObjects.Add(button3);


            GameObject button5 = GameObjectSystem.Instance.CreateGameObject("full");
            button5.Transform.Size = new Size(150, 60);
            button5.Transform.Position = new Vector2D(button.Transform.Position.X, button.Transform.Position.Y + 270);
            button5.AddComponent(ButtonSystem.Instance.CreateButton("full"));
            button5.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/FullscreenButton.png"));
            pauseObjects.Add(button5);

            GameObject button6 = GameObjectSystem.Instance.CreateGameObject("window");
            button6.Transform.Size = new Size(150, 60);
            button6.Transform.Position = new Vector2D(button.Transform.Position.X, button.Transform.Position.Y + 360);
            button6.AddComponent(ButtonSystem.Instance.CreateButton("window"));
            button6.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/WindowButton.png"));
            pauseObjects.Add(button6);

        }

        bool checkCollision()
        {
            for (int i = 0; i < tilemap.GetLength(0); i++)
            {
                for (int j = 0; j < tilemap.GetLength(1); j++)
                {
                    if (tiles[i, j] != null)
                    {
                        if (Collision(tiles[i, j], PlayerControl.Instance.Player.GameObject))
                        {
                            PlayerControl.Instance.Player.GameObject.Transform.Position = playerpos;
                            return true;
                        }
                    }
                }
            }
            return false;
         }

        private bool Collision(GameObject box_1, GameObject box_2)
        {
            if (box_1.Transform.Position.X < box_2.Transform.Position.X + box_2.Transform.Size.Width &&
                box_1.Transform.Position.X + box_1.Transform.Size.Width > box_2.Transform.Position.X &&
                box_1.Transform.Position.Y < box_2.Transform.Position.Y + box_2.Transform.Size.Height &&
                box_1.Transform.Position.Y + box_1.Transform.Size.Height > box_2.Transform.Position.Y)
            {
                return true;
            }
            return false;
        }
        public void AddEventListeners()
        {
            EventSystem.Instance.AddListener("Quit", new EventSystem.EventListener(true)
            {
                Method = (object[] parameters) => { IsRunning = false; }
            });
            EventSystem.Instance.AddListener("Pause", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    //Console.WriteLine("Paused");

                    paused = true;
                }
            });

            EventSystem.Instance.AddListener("UnPaused", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    paused = false;
                   // Console.WriteLine("UnPaused");

                }
            });

            EventSystem.Instance.AddListener("GameOver", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    //Console.WriteLine("All enemies down");
                    gameover = true;
                }
            });

            EventSystem.Instance.AddListener("killedEverybody", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                  //  Console.WriteLine("All enemies down");
                    enemyShooting.Active = true;
                }
            });

            
        }

        public void UpdateBackground()
        {
            foreach (var bg in backgroundList)
            {
                bg.Transform.Position =  new Vector2D(bg.Transform.Size.Width / 2 - LevelManager.SCREEN_OFFSET_X, bg.Transform.Size.Height / 2 - LevelManager.SCREEN_OFFSET_Y);
            }
        }
        public void UpdatePoint()
        {
            foreach (var pt in PointList)
            {
                pt.Transform.Position = new Vector2D(SCREEN_WIDTH - pt.Transform.Size.Width / 1.5, pt.Transform.Size.Height / 2);
            }
        }

        public void UpdatePlayer()
        {
            foreach (var player in PlayerList)
            {
                //double x = CameraSystem.Instance.getCamera().x + LevelManager.SCREEN_OFFSET_X/2;
                //double y = CameraSystem.Instance.getCamera().y - LevelManager.SCREEN_OFFSET_Y/2;
                player.Transform.Position = new Vector2D(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);
            }
            
        }
        public void UpdateTiles()
        {
            int posx = 0 - LevelManager.SCREEN_OFFSET_X + tileSize / 2;
            int posy = 0 - LevelManager.SCREEN_OFFSET_Y + tileSize / 2;

            for (int i = 0; i < tilemap.GetLength(0); i++)
            {
                for (int j = 0; j < tilemap.GetLength(1); j++)
                {
                    if (tiles[i, j] != null)
                    {
                        tiles[i, j].Transform.Position = new Vector2D(posx, posy);
                    }
                    posx += tileSize;
                    if (posx >= LevelManager.LEVEL_WIDTH - LevelManager.SCREEN_OFFSET_X)
                    {
                        posx = 0 - LevelManager.SCREEN_OFFSET_X + tileSize / 2;
                    }
                }
                posy += tileSize;
            }
        }
        public void BuildGameObjects()
        {
            GameObject background = GameObjectSystem.Instance.CreateGameObject("background");
            background.Transform.Size = new Size(LevelManager.LEVEL_WIDTH, LevelManager.LEVEL_HEIGHT);
            background.Transform.Position = new Vector2D(background.Transform.Size.Width / 2 - LevelManager.SCREEN_OFFSET_X, background.Transform.Size.Height / 2 - LevelManager.SCREEN_OFFSET_Y);
            background.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/backgroundMapEvenSmaller.png"));
            background.AddComponent(SoundSystem.Instance.CreateBackgroundSound("ravens.mp3"));
            backgroundList.Add(background);

            InitializeTileMap();

            GameObject pointsText = GameObjectSystem.Instance.CreateGameObject("points");
            pointsText.AddComponent(RenderSystem.Instance.CreateTextComponent());
            pointsText.Transform.Size = new Size(150, 80);
            pointsText.GetComponent<TextComponent>().Text.SetColor(0,0,0);
            pointsText.GetComponent<TextComponent>().Text.SetText("0000");
            pointsText.Transform.Position = new Vector2D(SCREEN_WIDTH - pointsText.Transform.Size.Width/1.5, pointsText.Transform.Size.Height/2);
            PointList.Add(pointsText);

            GameObject health = GameObjectSystem.Instance.CreateGameObject("health");
            health.AddComponent(RenderSystem.Instance.CreateTextComponent());
            health.Transform.Size = new Size(150, 80);
            health.GetComponent<TextComponent>().Text.SetColor(0, 0, 0);
            health.GetComponent<TextComponent>().Text.SetText("100");
            health.Transform.Position = new Vector2D(0 + pointsText.Transform.Size.Width / 1.5, pointsText.Transform.Size.Height / 2);

            GameObject player = GameObjectSystem.Instance.CreateGameObject("player");
            player.Transform.Size = new Size(64, 64);
            player.Transform.Position = new Vector2D(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);
            player.AddComponent(PlayerControl.Instance.CreatePlayerComponent());
            player.AddComponent(MovementSystem.Instance.CreateVelocityComponent(new Vector2D(5,5)));
            player.AddComponent(MovementSystem.Instance.CreateInput(InputType.KEYBOARD));
            player.AddComponent(RenderSystem.Instance.CreateSpritesheetRenderer("Sprites/character_Animation.png", new Vector2D(48,48), 8, 80));
            player.AddComponent(CameraSystem.Instance.CreateCamera());
            player.AddComponent(PlayerShootingSystem.Instance.CreateWeaponComponent());
            player.AddComponent(PhysicSystem.Instance.CreateBoxCollider(player));
            player.AddComponent(SoundSystem.Instance.CreateSoundEffect("test2.wav", "bulletshoot"));
            player.AddComponent(HealthSystem.Instance.CreateHealthComponent(100));
            PlayerList.Add(player);

            /*
            GameObject player_2 = GameObjectSystem.Instance.CreateGameObject("player_2");
            player_2.Transform.Size = new Size(64, 64);
            player_2.Transform.Position = new Vector2D(SCREEN_WIDTH / 2 + 100, SCREEN_HEIGHT / 2);
            player_2.AddComponent(PlayerControl.Instance.CreatePlayerComponent());
            player_2.AddComponent(MovementSystem.Instance.CreateVelocityComponent(new Vector2D(5, 5)));
            player_2.AddComponent(MovementSystem.Instance.CreateInput(InputType.CONTROLLER));
            player_2.AddComponent(RenderSystem.Instance.CreateSpritesheetRenderer("Sprites/character_idle.png", new Vector2D(48, 48), 4, 600));
            player_2.AddComponent(PlayerShootingSystem.Instance.CreateWeaponComponent());
            player_2.AddComponent(PhysicSystem.Instance.CreateBoxCollider(player));
            player_2.AddComponent(SoundSystem.Instance.CreateSoundEffect("test.wav", "bulletshoot"));
            */
            
            GameObject spawner = GameObjectSystem.Instance.CreateGameObject("Spawner");
            spawner.AddComponent(SpawnSystem.Instance.CreateSpawner());
            spawner.Transform.Position = new Vector2D(0, 0);
            spawner.GetComponent<SpawnerComponent>().CoolDown = 1.5f;
            spawner.GetComponent<SpawnerComponent>().SpawnAmount = 50;
            spawner.GetComponent<SpawnerComponent>().Mode = Mode.SINGLE;
            spawner.AddComponent(TweenSystem.Instance.CreateTweenComponent(spawner.Transform.Position, spawner.Transform.Position + new Vector2D(0, Program.SCREEN_HEIGHT), EaseType.SINE_IN_OUT, 3, true));
            
            enemyShooting = GameObjectSystem.Instance.CreateGameObject("enemyShooting");
            enemyShooting.Transform.Size = new Size(64, 64);
            enemyShooting.Transform.Position = new Vector2D(0, 50);
            enemyShooting.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/paddle.png"));
            enemyShooting.AddComponent(PhysicSystem.Instance.CreateBoxCollider(enemyShooting));
            enemyShooting.AddComponent(FightSystem.Instance.CreateAttackComponent(AttackType.SHOOT, 25));
            enemyShooting.AddComponent(TweenSystem.Instance.CreateTweenComponent(enemyShooting.Transform.Position, enemyShooting.Transform.Position + new Vector2D(SCREEN_WIDTH, 50), EaseType.SINE_IN_OUT, 4, true));
            enemyShooting.Active = true;


            GameObject spawner2 = GameObjectSystem.Instance.CreateGameObject("Spawner2");
            spawner2.AddComponent(SpawnSystem.Instance.CreateSpawner());
            spawner2.Transform.Position = new Vector2D(SCREEN_WIDTH + 300, SCREEN_HEIGHT/4);
            spawner2.GetComponent<SpawnerComponent>().CoolDown = .8f;
            spawner2.GetComponent<SpawnerComponent>().SpawnAmount = 15;
            spawner2.GetComponent<SpawnerComponent>().Mode = Mode.SINGLE;
            /*
            GameObject spawner_1 = GameObjectSystem.Instance.CreateGameObject("spawner_1");
            spawner_1.AddComponent(SpawnSystem.Instance.CreateSpawner());
            spawner_1.Transform.Position = new Vector2D(-300, -300);
            spawner_1.GetComponent<SpawnerComponent>().CoolDown = 1f;
            spawner_1.GetComponent<SpawnerComponent>().SpawnAmount = 15;
            spawner_1.GetComponent<SpawnerComponent>().Mode = Mode.HORDE;

            GameObject spawner_2 = GameObjectSystem.Instance.CreateGameObject("spawner_2");
            spawner_2.AddComponent(SpawnSystem.Instance.CreateSpawner());
            spawner_2.Transform.Position = new Vector2D(2000, 1000);
            spawner_2.GetComponent<SpawnerComponent>().CoolDown = .75f;
            spawner_2.GetComponent<SpawnerComponent>().SpawnAmount = 20;
            spawner_2.GetComponent<SpawnerComponent>().Mode = Mode.SINGLE;

            GameObject spawner_3 = GameObjectSystem.Instance.CreateGameObject("spawner_3");
            spawner_3.AddComponent(SpawnSystem.Instance.CreateSpawner());
            spawner_3.Transform.Position = new Vector2D(-300, 1000);
            spawner_3.GetComponent<SpawnerComponent>().CoolDown = .5f;
            spawner_3.GetComponent<SpawnerComponent>().SpawnAmount = 20;
            spawner_3.GetComponent<SpawnerComponent>().Mode = Mode.SINGLE;
            */
            /*
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    GameObject enemy = GameObjectSystem.Instance.CreateGameObject("enemy " + i + "_" + j);
                    enemy.Transform.Size = new Size(64, 64);
                    enemy.Transform.Position = new Vector2D(200 * i , 850 * j);
                    enemy.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/enemy.png"));
                    enemy.AddComponent(PhysicSystem.Instance.CreateBoxCollider(enemy));
                    enemy.AddComponent(MovementSystem.Instance.CreateVelocityComponent(new Vector2D(4,4)));
                    enemy.AddComponent(BotControl.Instance.CreateAiComponent());
                }
            }
            */
        }

        private void InitializeTileMap()
        {
            int posx = 0 - LevelManager.SCREEN_OFFSET_X + tileSize / 2;
            int posy = 0 - LevelManager.SCREEN_OFFSET_Y + tileSize / 2;

            int type = 0;

            for (int i = 0; i < tilemap.GetLength(0); i++)
            {
                for (int j = 0; j < tilemap.GetLength(1); j++)
                {
                    type = tilemap[i, j];
                    if (type == 1)
                    {
                        tiles[i, j] = new GameObject("tile");
                        tiles[i, j].Transform.Size = new Size(tileSize, tileSize);
                        tiles[i, j].Transform.Position = new Vector2D(posx, posy);
                        //remove the Sprite later
                        tiles[i, j].AddComponent(PhysicSystem.Instance.CreateBoxCollider(tiles[i,j]));
                        tiles[i, j].AddComponent(PhysicSystem.Instance.CreateObstacleComponent());
                        //tiles[i, j].AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/test.png"));
                    }
                    posx += tileSize;
                    if (posx >= LevelManager.LEVEL_WIDTH - LevelManager.SCREEN_OFFSET_X)
                    {
                        posx = 0 - LevelManager.SCREEN_OFFSET_X + tileSize / 2;
                    }
                }
                posy += tileSize;
            }
        }
        
        public static void Error(string v)
        {
            Console.WriteLine($"ERROR: { v } SDL_Error: {SDL_GetError()}");
        }
        #endregion
        
    }
}
