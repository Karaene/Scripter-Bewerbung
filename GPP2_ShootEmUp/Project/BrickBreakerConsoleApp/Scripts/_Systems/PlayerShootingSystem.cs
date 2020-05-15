using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class PlayerShootingSystem : ISystem
    {
        private WeaponComponent weapon;
        private int amountOfBullets = 100;

        public const double PISTOL_PROJECTILE_SPEED = 16;
        public const int FIRE_RATE = 300;
        public bool Fire { get; set; } = false;

        Vector2D dir;
        InputType inputType;

        LTimer timer = new LTimer();
        uint startTick = 0;

        //private List<WeaponComponent> _activeWeapons = new List<WeaponComponent>();

        private List<GameObject> bullets = new List<GameObject>();

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
                bullet.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/Bullet.png"));
                bullet.AddComponent(CreateBulletComponent(10, true));
                bullet.AddComponent(PhysicSystem.Instance.CreateCircleCollider(bullet, 32));
                bullet.Transform.Size = new Size(32, 32);
                bullet.Active = false;

                bullets.Add(bullet);
            }
            Console.WriteLine("Amount of ready Bullets: " + bullets.Count);
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
                case WeaponType.UZI:
                    FireUzi();
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
        

        private void FireUzi() { }

        public void Update()
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
            if (Fire)
            {
                if (timer.GetTicks() > startTick + FIRE_RATE)
                {
                    startTick = timer.GetTicks();
                    FireGun(dir, inputType);
                }
            }
        }
    }
    enum WeaponType { PISTOL, UZI}
    enum BulletType { NORMALBULLET, REINFORCED }
}
