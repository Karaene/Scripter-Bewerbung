using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class PlayerShootingSystem : ISystem
    {
        private WeaponComponent weapon;
        private int amountOfBullets = 100;
        private int amountOfSniperBullets = 50;

        public double PISTOL_PROJECTILE_SPEED = 16;
        public int FIRE_RATE = 300;
        public bool Fire { get; set; } = false;

        Vector2D dir;
        InputType inputType;

        LTimer timer = new LTimer();
        uint startTick = 0;

        //private List<WeaponComponent> _activeWeapons = new List<WeaponComponent>();

        private List<GameObject> bullets = new List<GameObject>();
        private List<GameObject> bullets_Sniper = new List<GameObject>();

        private PlayerShootingSystem()
        {
            timer.Start();
        }

        private static PlayerShootingSystem instance = null;
        public static PlayerShootingSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerShootingSystem();
                }
                return instance;
            }
        }

        public void Init()
        {
            EventSystem.Instance.AddListener("FireMouse", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    Fire = true;
                    inputType = InputType.KEYBOARD;
                }
            });
            EventSystem.Instance.AddListener("FireController", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    Fire = true;
                    inputType = InputType.CONTROLLER;
                }
            });
            EventSystem.Instance.AddListener("StopFire", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    Fire = false;
                }
            });
            EventSystem.Instance.AddListener("ChangedDir", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    dir = (Vector2D)parameters[0];
                }
            });

            for (int i = 0; i < amountOfBullets; i++)
            {
                GameObject bullet = GameObjectSystem.Instance.CreateGameObject("bullet" + i);
                bullet.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/Bullet.png", Layer.FOREGROUND));
                bullet.AddComponent(CreateBulletComponent(10, true));
                bullet.AddComponent(PhysicSystem.Instance.CreateCircleCollider(bullet, 36));
                bullet.Transform.Size = new Size(32, 32);
                bullet.Active = false;

                bullets.Add(bullet);
            }
            Console.WriteLine("Amount of ready Bullets: " + bullets.Count);

            for (int i = 0; i < amountOfSniperBullets; i++)
            {
                GameObject sniper_bullet = GameObjectSystem.Instance.CreateGameObject("sniper_bullet" + i);
                sniper_bullet.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/Bulletold.png", Layer.FOREGROUND));
                sniper_bullet.AddComponent(CreateBulletComponent(25, true));
                sniper_bullet.AddComponent(PhysicSystem.Instance.CreateCircleCollider(sniper_bullet, 24));
                sniper_bullet.Transform.Size = new Size(24, 24);
                sniper_bullet.Active = false;

                bullets_Sniper.Add(sniper_bullet);
            }
            Console.WriteLine("Amount of ready Sniper_Bullets: " + bullets_Sniper.Count);
        }

        public WeaponComponent CreateWeaponComponent()
        {
            WeaponComponent weaponComponent = new WeaponComponent(this);
            weapon = weaponComponent;
            //_activeWeapons.Add(weapon);
            return weaponComponent;
        }

        public BulletComponent CreateBulletComponent(int damage, bool friendly)
        {
            BulletComponent bulletComponent = new BulletComponent(this, damage, friendly);
            return bulletComponent;
        }

        public void FireGun(Vector2D dir, InputType inputType)
        {
            switch (weapon.WeaponType)
            {
                case WeaponType.PISTOL:
                    FirePistol(dir, inputType);
                    break;
                case WeaponType.SNIPER:
                    FireSniper(dir, inputType);
                    break;
                case WeaponType.SHOTGUN:
                    FireShotGun();
                    break;
            }
        }

        public Vector2D GetDirection(Vector2D mousePos, Vector2D player)
        {
            Vector2D mousePositionScreen = new Vector2D(mousePos.X + CameraSystem.cameraRect.x, mousePos.Y + CameraSystem.cameraRect.y);
            Vector2D direction = mousePositionScreen - player;
            double length = direction.Norm();
            return new Vector2D((direction.X / length) * PISTOL_PROJECTILE_SPEED, (direction.Y / length) * PISTOL_PROJECTILE_SPEED);
        }
        

        private void FirePistol(Vector2D dir, InputType inputType)
        {
            Console.WriteLine("Fire!!");
            int bulletNr = 0;
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].Active)
                {
                    bullets[i].Transform.Position = weapon.GameObject.Transform.Position;
                    bullets[i].Active = true;
                    bulletNr = i;
                    //Console.WriteLine("Bullet " + i + "Active: " + bullets[i].Active + bullets[i].Transform.Position);
                    break;
                }
            }
            //Console.WriteLine("Mouse pointer at: " + dir);
            GameObject bullet = bullets[bulletNr];

            if(inputType == InputType.CONTROLLER)
            {
                bullet.Transform.Direction = new Vector2D(dir.X * PISTOL_PROJECTILE_SPEED, dir.Y * PISTOL_PROJECTILE_SPEED);
            }
            else if(inputType == InputType.KEYBOARD)
            {
                //Console.WriteLine("MouseInput");
                bullet.Transform.Direction = GetDirection(dir, bullet.Transform.Position);
            }
            //bullet.Transform.Rotation = Vector2D.GetAngle(Vector2D.DOWN, bullet.Transform.Direction);
            //Console.WriteLine("Angle: " + Vector2D.GetAngle(Vector2D.DOWN, bullet.Transform.Direction));
        }


        private void FireSniper(Vector2D dir, InputType inputType)
        {
            int bulletNr = 0;
            for (int i = 0; i < bullets_Sniper.Count; i++)
            {
                if (!bullets_Sniper[i].Active)
                {
                    bullets_Sniper[i].Transform.Position = weapon.GameObject.Transform.Position;
                    bullets_Sniper[i].Active = true;
                    bulletNr = i;
                    break;
                }
            }
            GameObject bullet = bullets_Sniper[bulletNr];

            if (inputType == InputType.CONTROLLER)
            {
                bullet.Transform.Direction = new Vector2D(dir.X * PISTOL_PROJECTILE_SPEED, dir.Y * PISTOL_PROJECTILE_SPEED);
            }
            else if (inputType == InputType.KEYBOARD)
            {
                bullet.Transform.Direction = GetDirection(dir, bullet.Transform.Position);
            }
            bullet.Transform.Rotation = Vector2D.GetAngle(Vector2D.DOWN, bullet.Transform.Direction);
        }

        private void FireShotGun()
        {
            Console.WriteLine("FireShotGun!!");
            
            int[] bulletNrs = new int[3];
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    if (!bullets[i].Active)
                    {
                        bullets[i].Transform.Position = weapon.GameObject.Transform.Position;
                        bullets[i].Active = true;
                        bulletNrs[j] = i;
                        //Console.WriteLine("Bullet " + i + "Active: " + bullets[i].Active + bullets[i].Transform.Position);
                        break;
                    }
                }
            }
            
            GameObject bullet_1 = bullets[bulletNrs[0]];
            GameObject bullet_2 = bullets[bulletNrs[1]];
            GameObject bullet_3 = bullets[bulletNrs[2]];

            if (inputType == InputType.CONTROLLER)
            {
                bullet_1.Transform.Direction = new Vector2D(dir.X * PISTOL_PROJECTILE_SPEED, dir.Y * PISTOL_PROJECTILE_SPEED);
                bullet_2.Transform.Direction = new Vector2D(dir.X * PISTOL_PROJECTILE_SPEED, dir.Y * PISTOL_PROJECTILE_SPEED);
                bullet_3.Transform.Direction = new Vector2D(dir.X * PISTOL_PROJECTILE_SPEED, dir.Y * PISTOL_PROJECTILE_SPEED);
            }
            else if (inputType == InputType.KEYBOARD)
            {
                bullet_1.Transform.Direction = GetDirection(dir, bullet_1.Transform.Position);
                bullet_2.Transform.Direction = GetDirection(new Vector2D(dir.X - 20, dir.Y - 20), bullet_2.Transform.Position);
                bullet_3.Transform.Direction = GetDirection(new Vector2D(dir.X + 20, dir.Y + 20), bullet_3.Transform.Position);
            }
        }

        private void FireUzi() { }

        private void UpdateBullets()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].Active)
                {
                    if (bullets[i].Transform.Position.X - CameraSystem.cameraRect.x < 0 ||
                        bullets[i].Transform.Position.X - CameraSystem.cameraRect.x > Program.SCREEN_WIDTH ||
                        bullets[i].Transform.Position.Y - CameraSystem.cameraRect.y < 0 ||
                        bullets[i].Transform.Position.Y - CameraSystem.cameraRect.y > Program.SCREEN_HEIGHT)
                    {
                        Console.WriteLine("Bullet is outside the screen!");
                        bullets[i].Active = false;
                    }
                    //Console.WriteLine("Bullet " + i + "Active: " + bullets[i].Active + bullets[i].Transform.Position);

                    bullets[i].Transform.Position += new Vector2D(bullets[i].Transform.Direction.X, bullets[i].Transform.Direction.Y);
                }
            }

            for (int i = 0; i < bullets_Sniper.Count; i++)
            {
                if (bullets_Sniper[i].Active)
                {
                    if (bullets_Sniper[i].Transform.Position.X - CameraSystem.cameraRect.x < 0 ||
                        bullets_Sniper[i].Transform.Position.X - CameraSystem.cameraRect.x > Program.SCREEN_WIDTH ||
                        bullets_Sniper[i].Transform.Position.Y - CameraSystem.cameraRect.y < 0 ||
                        bullets_Sniper[i].Transform.Position.Y - CameraSystem.cameraRect.y > Program.SCREEN_HEIGHT)
                    {
                        Console.WriteLine("Bullet is outside the screen!");
                        bullets_Sniper[i].Active = false;
                    }
                    //Console.WriteLine("Bullet " + i + "Active: " + bullets[i].Active + bullets[i].Transform.Position);

                    bullets_Sniper[i].Transform.Position += new Vector2D(bullets_Sniper[i].Transform.Direction.X, bullets_Sniper[i].Transform.Direction.Y);
                }
            }
        }

        public void Update()
        {
            UpdateBullets();
            if (Fire)
            {
                switch (weapon.WeaponType)
                {
                    case WeaponType.PISTOL:
                        FIRE_RATE = 400;
                        PISTOL_PROJECTILE_SPEED = 16;
                        break;
                    case WeaponType.SHOTGUN:
                        FIRE_RATE = 800;
                        PISTOL_PROJECTILE_SPEED = 10;
                        break;
                    case WeaponType.SNIPER:
                        FIRE_RATE = 1200;
                        PISTOL_PROJECTILE_SPEED = 26;
                        break;
                }

                if (timer.GetTicks() > startTick + FIRE_RATE)
                {
                    //Maybe Play shooting sound here?
                    SoundSystem.Instance.sound.PlaySoundtrack();
                    startTick = timer.GetTicks();
                    FireGun(dir, inputType);
                }
            }
        }
    }
    enum WeaponType { PISTOL, UZI, SHOTGUN, SNIPER }
    enum BulletType { NORMALBULLET, REINFORCED }
}
