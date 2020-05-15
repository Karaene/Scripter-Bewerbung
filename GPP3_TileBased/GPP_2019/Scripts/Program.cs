using System;
using System.Collections.Generic;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace GPP_2019
{
    class Program
    {
        #region Attributes and Properties

        private const float MS_PER_UPDATE = 16;

        public static bool ScreenChanged { get; set; } = false;
        public static int SCREEN_WIDTH { get; set; } = 960;
        public static int SCREEN_HEIGHT { get; set; } = 960;
        private static float _Elapsed = 0;

        
        private List<GameObject> PlayerList = new List<GameObject>();
        private List<GameObject> PointList = new List<GameObject>();

        public bool IsRunning { get; set; }
        private bool paused;
        private bool pausedTemp = true;

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
            program.Run();

            SDL_Quit();
            return;
        }

        private void StartScreen()
        {
            //TileSystem.Instance.CreateTiles("GPP_2019/stripes.txt");
            

            IsRunning = true;
            float previous = SDL_GetTicks();
            float lag = 0.0f;

            AddEventListeners();
            //ButtonSystem.Instance.Init();
            //UISystem.Instance.Init();
            //UISystem.Instance.ShowStartMenu();
            //TileSystem.Instance.ReadMap(@"D:\Final\GPP_2019\testi.txt");

            while (IsRunning)
            {
                float current = SDL_GetTicks();
                _Elapsed = current - previous;
                previous = current;
                lag += _Elapsed;

                while (lag >= MS_PER_UPDATE)
                {
                    InputSystem.Instance.UpdateMenu();
                   // ButtonSystem.Instance.Update();
                    if (ButtonSystem.Instance.clicked)
                    {
                        Run();
                    }
                    lag -= MS_PER_UPDATE;
                }
                RenderSystem.Instance.Render();
            }
        }


        private void Run()
        {
            //Start FPS Timer
            int countedFrames = 0;

            Utils.Instance.StartTimer();

            SoundSystem.Instance.Init();

            BuildGameObjects();
            CreateEnemiesFloor1();
            SwapEnemies();
            AddEventListeners();

            //UISystem.Instance.Init();
            InventorySystem.Instance.Init();
            HealthSystem.Instance.Init();
            FightSystem.Instance.Init();
            TweenSystem.Instance.Init();
            PlayerShootingSystem.Instance.Init();
            SpawnSystem.Instance.Init();
            PointSystem.Instance.Init();
            CollisionHandlerSystem.Instance.Init();
            UISystem.Instance.InitIngameUI();
            UISystem.Instance.InitPauseMenu();
            SkillSystem.Instance.Init();


            //EventSystem.Instance.RaiseEvent("IncreaseSkill", PlayerControl.Instance.Player.GameObject, SkillSystem.SkillType.HEALTH);


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
                    if (ScreenChanged)
                    {
                        UISystem.Instance.UpdateBackground();
                        CameraSystem.Instance.Update();
                        UISystem.Instance.ShowPauseMenu();
                        UpdatePoint();
                        ScreenChanged = false;
                    }

                    if (!paused)
                    {
                        SpawnSystem.Instance.Update();
                        FightSystem.Instance.Update();
                        PhysicSystem.Instance.Update();
                        PlayerControl.Instance.Update();
                        CameraSystem.Instance.Update();
                        MovementSystem.Instance.Update();
                        PlayerShootingSystem.Instance.Update();
                        BotControl.Instance.Update(_Elapsed);
                        TweenSystem.Instance.Update();
                        PointSystem.Instance.Update();
                        HealthSystem.Instance.Update();
                        UISystem.Instance.EnableUI();
                        UISystem.Instance.Update();

                        pausedTemp = true;
                        UISystem.Instance.DisableMenu();
                    }
                    else
                    {
                        InputSystem.Instance.UpdateMenu();
                        UISystem.Instance.DisableUI();

                        //call Menue
                        if (pausedTemp)
                        {
                            UISystem.Instance.ShowPauseMenu();

                            pausedTemp = false;
                        }
                        ButtonSystem.Instance.Update();
                    }
                    

                    lag -= MS_PER_UPDATE;
                }
                RenderSystem.Instance.Render();
                SoundSystem.Instance.Update();

                //Console.WriteLine(Utils.Instance.CalculateFPS(countedFrames));
                ++countedFrames;
            }
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
                    paused = true;
                    Console.WriteLine("Paused");
                }
            });

            EventSystem.Instance.AddListener("UnPaused", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    paused = false;
                    Console.WriteLine("UnPaused");
                }
            });

            EventSystem.Instance.AddListener("killedEverybody", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    Console.WriteLine("All enemies down");
                    enemyShooting.Active = true;
                }
            });
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

        public void BuildGameObjects()
        {
            TileSystem.Instance.CreateBackgroundTiles("level1.txt", "walls1.txt", "doors1.txt", "Sprites/level1tiles64.png", 1);
            TileSystem.Instance.CreateForeground("foremap.txt", "Sprites/foreground1tiles64.png", 1);
            TileSystem.Instance.TurnOffEverything();
            TileSystem.Instance.CreateBackgroundTiles("start.txt", "wall.txt", "colliders.txt", "Sprites/tiles.v1.png", 0);
            TileSystem.Instance.CreateForeground("hello.txt", "Sprites/tileforeground.v1.png", 0);

            //AStarPathfinding.Instance.ShowAllNodes();
            /*
            GameObject background = GameObjectSystem.Instance.CreateGameObject("background");
            background.Transform.Size = new Size(LevelManager.LEVEL_WIDTH, LevelManager.LEVEL_HEIGHT);
            background.Transform.Position = new Vector2D(background.Transform.Size.Width / 2 - LevelManager.SCREEN_OFFSET_X, background.Transform.Size.Height / 2 - LevelManager.SCREEN_OFFSET_Y);
            background.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/backgroundMapEvenSmaller.png", Layer.BACKGROUND));
            background.AddComponent(SoundSystem.Instance.CreateBackgroundSound("ravens.mp3"));
            background.Active = true;
            UISystem.Instance.backgroundList.Add(background);
            */

            GameObject backgroundSound = GameObjectSystem.Instance.CreateGameObject("´BackgroundSound");
            backgroundSound.AddComponent(SoundSystem.Instance.CreateBackgroundSound("betterdays.mp3"));

            GameObject pointsText = GameObjectSystem.Instance.CreateGameObject("points");
            pointsText.AddComponent(RenderSystem.Instance.CreateTextComponent());
            pointsText.Transform.Size = new Size(50, 40);
            pointsText.GetComponent<TextComponent>().Text.SetColor(0,0,0);
            pointsText.GetComponent<TextComponent>().Text.SetText("0000");
            pointsText.Transform.Position = new Vector2D(SCREEN_WIDTH - pointsText.Transform.Size.Width/1.5, pointsText.Transform.Size.Height/2);
            PointList.Add(pointsText);

            GameObject health = GameObjectSystem.Instance.CreateGameObject("health");
            health.AddComponent(RenderSystem.Instance.CreateTextComponent());
            health.Transform.Size = new Size(50, 40);
            health.GetComponent<TextComponent>().Text.SetColor(0, 0, 0);
            health.GetComponent<TextComponent>().Text.SetText("100");
            health.Transform.Position = new Vector2D(0 + pointsText.Transform.Size.Width / 1.5, pointsText.Transform.Size.Height / 2);

            GameObject player = GameObjectSystem.Instance.CreateGameObject("player");
            player.Transform.Size = new Size(57, 57);
            player.Transform.Position = new Vector2D(800, 0);
            //player.Transform.Position = new Vector2D(200, 200);
            player.AddComponent(PlayerControl.Instance.CreatePlayerComponent());
            player.AddComponent(MovementSystem.Instance.CreateVelocityComponent(new Vector2D(5,5)));

            //player.AddComponent(MovementSystem.Instance.CreateInput(InputType.CONTROLLER));
            player.AddComponent(MovementSystem.Instance.CreateInput(InputType.KEYBOARD));

            player.AddComponent(RenderSystem.Instance.CreateSpritesheetRenderer("Sprites/character_Animation.png", new Vector2D(48,48), 8, 25, Layer.MIDDLEGROUND));
            player.AddComponent(CameraSystem.Instance.CreateCamera());
            player.AddComponent(PlayerShootingSystem.Instance.CreateWeaponComponent());
            player.AddComponent(PhysicSystem.Instance.CreateBoxCollider(player, -10, -10));
            player.AddComponent(SoundSystem.Instance.CreateSoundEffect("shoot3.wav", "bulletshoot"));
            player.AddComponent(HealthSystem.Instance.CreateHealthComponent(100));
            player.AddComponent(InventorySystem.Instance.CreateInventoryComponent());
            GameObject pistol = GameObjectSystem.Instance.CreateGameObject("Pistol");
            pistol.AddComponent(PickUpSystem.Instance.CreatePickUpComponent(PickUpType.ITEM, WeaponType.PISTOL));
            player.GetComponent<InventoryComponent>().Items.Add(pistol);
            player.AddComponent(SkillSystem.Instance.CreateSkillComponent());
            PlayerList.Add(player);

            /*
            GameObject spawner = GameObjectSystem.Instance.CreateGameObject("Spawner");
            spawner.AddComponent(SpawnSystem.Instance.CreateSpawner());
            spawner.Transform.Position = new Vector2D(100, 100);
            spawner.GetComponent<SpawnerComponent>().CoolDown = 5f;
            spawner.GetComponent<SpawnerComponent>().SpawnAmount = 2;
            spawner.GetComponent<SpawnerComponent>().Mode = Mode.SINGLE;
            spawner.AddComponent(TweenSystem.Instance.CreateTweenComponent(spawner.Transform.Position, spawner.Transform.Position + new Vector2D(0, Program.SCREEN_HEIGHT), EaseType.SINE_IN_OUT, 3, true));
            //spawner.Active = false;
            */
        

            GameObject testPickUp = GameObjectSystem.Instance.CreateGameObject("coin");
            testPickUp.Transform.Position = new Vector2D((SCREEN_WIDTH / 2) - 350, (SCREEN_HEIGHT / 2) + 200);
            testPickUp.Transform.Size = new Size(24, 24);
            testPickUp.AddComponent(PickUpSystem.Instance.CreatePickUpComponent(PickUpType.COIN));
            testPickUp.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/Coin.png", Layer.MIDDLEGROUND));
            testPickUp.AddComponent(PhysicSystem.Instance.CreateBoxCollider(testPickUp, 10, 10));
            //testPickUp.AddComponent(PhysicSystem.Instance.CreateCircleCollider(testPickUp, 10));

            GameObject weaponPickUp = GameObjectSystem.Instance.CreateGameObject("Shotgun");
            weaponPickUp.Transform.Position = new Vector2D((SCREEN_WIDTH / 2), (SCREEN_HEIGHT / 2) - 760);
            weaponPickUp.Transform.Size = new Size(48, 24);
            weaponPickUp.AddComponent(PickUpSystem.Instance.CreatePickUpComponent(PickUpType.ITEM, WeaponType.SHOTGUN));
            weaponPickUp.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/shotgun.png", Layer.MIDDLEGROUND));
            weaponPickUp.AddComponent(PhysicSystem.Instance.CreateBoxCollider(weaponPickUp, 5, 5));

            GameObject weaponPickUp_2 = GameObjectSystem.Instance.CreateGameObject("Sniper");
            weaponPickUp_2.Transform.Position = new Vector2D((SCREEN_WIDTH / 2) + 350, (SCREEN_HEIGHT / 2) + 200);
            weaponPickUp_2.Transform.Size = new Size(48, 24);
            weaponPickUp_2.AddComponent(PickUpSystem.Instance.CreatePickUpComponent(PickUpType.ITEM, WeaponType.SNIPER));
            weaponPickUp_2.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/sniper.png", Layer.MIDDLEGROUND));
            weaponPickUp_2.AddComponent(PhysicSystem.Instance.CreateBoxCollider(weaponPickUp_2, 5, 5));

            /*
            enemyShooting = GameObjectSystem.Instance.CreateGameObject("enemyShooting");
            enemyShooting.Transform.Size = new Size(64, 64);
            enemyShooting.Transform.Position = new Vector2D(0, 50);
            enemyShooting.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/paddle.png", Layer.FOREGROUND));
            enemyShooting.AddComponent(PhysicSystem.Instance.CreateBoxCollider(enemyShooting, -2, -2));
            enemyShooting.AddComponent(FightSystem.Instance.CreateAttackComponent(AttackType.SHOOT, 25));
            enemyShooting.AddComponent(TweenSystem.Instance.CreateTweenComponent(enemyShooting.Transform.Position, enemyShooting.Transform.Position + new Vector2D(SCREEN_WIDTH, 50), EaseType.SINE_IN_OUT, 4, true));
            enemyShooting.Active = false;
            */

            /*
            GameObject spawner_1 = GameObjectSystem.Instance.CreateGameObject("spawner_1");
            spawner_1.AddComponent(SpawnSystem.Instance.CreateSpawner());
            spawner_1.Transform.Position = new Vector2D(-350, -300);
            spawner_1.GetComponent<SpawnerComponent>().CoolDown = 1f;
            spawner_1.GetComponent<SpawnerComponent>().SpawnAmount = 10;
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
        public void SwapEnemies()
        {
            SpawnSystem.Instance.DeactivateEverySpawner();

        }

        public void SwapEnemies2()
        {
            SpawnSystem.Instance.ActivateEverySpawner();
        }

        public void CreateEnemiesFloor1()
        {
            GameObject spawner = GameObjectSystem.Instance.CreateGameObject("Spawner");
            spawner.AddComponent(SpawnSystem.Instance.CreateSpawner());
            spawner.Transform.Position = new Vector2D(1204, -130);
            spawner.GetComponent<SpawnerComponent>().CoolDown = .8f;
            spawner.GetComponent<SpawnerComponent>().SpawnAmount = 3;
            spawner.GetComponent<SpawnerComponent>().Mode = Mode.HORDE;
            //spawner.AddComponent(TweenSystem.Instance.CreateTweenComponent(spawner.Transform.Position, spawner.Transform.Position + new Vector2D(0, Program.SCREEN_HEIGHT), EaseType.SINE_IN_OUT, 3, true));

            GameObject spawner2 = GameObjectSystem.Instance.CreateGameObject("Spawner2");
            spawner2.AddComponent(SpawnSystem.Instance.CreateSpawner());
            spawner2.Transform.Position = new Vector2D(1000, 500);
            spawner2.GetComponent<SpawnerComponent>().CoolDown = .8f;
            spawner2.GetComponent<SpawnerComponent>().SpawnAmount = 1;
            spawner2.GetComponent<SpawnerComponent>().Mode = Mode.HORDE;
            //spawner2.AddComponent(TweenSystem.Instance.CreateTweenComponent(spawner.Transform.Position, spawner.Transform.Position + new Vector2D(0, Program.SCREEN_HEIGHT), EaseType.SINE_IN_OUT, 3, true));

            GameObject spawner3 = GameObjectSystem.Instance.CreateGameObject("Spawner3");
            spawner3.AddComponent(SpawnSystem.Instance.CreateSpawner());
            spawner3.Transform.Position = new Vector2D(1300, 1000);
            spawner3.GetComponent<SpawnerComponent>().CoolDown = .8f;
            spawner3.GetComponent<SpawnerComponent>().SpawnAmount = 3;
            spawner3.GetComponent<SpawnerComponent>().Mode = Mode.HORDE;
            //spawner3.AddComponent(TweenSystem.Instance.CreateTweenComponent(spawner.Transform.Position, spawner.Transform.Position + new Vector2D(0, Program.SCREEN_HEIGHT), EaseType.SINE_IN_OUT, 3, true));

            GameObject spawner4 = GameObjectSystem.Instance.CreateGameObject("Spawner4");
            spawner4.AddComponent(SpawnSystem.Instance.CreateSpawner());
            spawner4.Transform.Position = new Vector2D(-500, 700);
            spawner4.GetComponent<SpawnerComponent>().CoolDown = .8f;
            spawner4.GetComponent<SpawnerComponent>().SpawnAmount = 6;
            spawner4.GetComponent<SpawnerComponent>().Mode = Mode.HORDE;
            //spawner4.AddComponent(TweenSystem.Instance.CreateTweenComponent(spawner.Transform.Position, spawner.Transform.Position + new Vector2D(0, Program.SCREEN_HEIGHT), EaseType.SINE_IN_OUT, 3, true));
        }

        public static void Error(string v)
        {
            Console.WriteLine($"ERROR: { v } SDL_Error: {SDL_GetError()}");
        }
        #endregion
    }
}