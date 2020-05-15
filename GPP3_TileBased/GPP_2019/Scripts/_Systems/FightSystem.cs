using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class FightSystem : ISystem
    {
        LTimer timer = new LTimer();
        uint startTick = 0;

        private int amountOfBullets = 100;
        public const double PISTOL_PROJECTILE_SPEED = 10;
        public int fire_rate = 300;
        public bool Fire { get; set; } = false;

        private List<IEntityComponent> _attackComponents = new List<IEntityComponent>();
        private List<GameObject> bullets = new List<GameObject>();

        private FightSystem() { } 
        private static FightSystem instance = null;
        public static FightSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FightSystem();
                }
                return instance;
            }
        }

        public void Init()
        {
            timer.Start();
            for (int i = 0; i < amountOfBullets; i++)
            {
                GameObject bullet = GameObjectSystem.Instance.CreateGameObject("bulletEnemy" + i);
                bullet.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/Bulletold.png", Layer.FOREGROUND));
                bullet.AddComponent(PlayerShootingSystem.Instance.CreateBulletComponent(25, false));
                bullet.AddComponent(PhysicSystem.Instance.CreateCircleCollider(bullet, 32));
                bullet.Transform.Size = new Size(32, 32);
                bullet.Active = false;

                bullets.Add(bullet);
            }

            
            EventSystem.Instance.AddListener("IncreaseAttackspeed", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {

                    fire_rate = (int)parameters[0];
                    Console.WriteLine("Attackspeed: " + (int)parameters[0]);
                }
            });
            
        }

        public IEntityComponent CreateAttackComponent( AttackType attackType, int damage)
        {
            AttackComponent ac = new AttackComponent(this, attackType, damage);
            _attackComponents.Add(ac);
            return ac;
        }

        public Vector2D GetDirection(Vector2D enemy_pos, Vector2D player)
        {
            Vector2D enemyPosScreen = new Vector2D(enemy_pos.X + CameraSystem.cameraRect.x, enemy_pos.Y + CameraSystem.cameraRect.y);
            Vector2D playerPosScreen = new Vector2D(player.X + CameraSystem.cameraRect.x, player.Y + CameraSystem.cameraRect.y);
            Vector2D direction = enemyPosScreen - playerPosScreen;
            double length = direction.Norm();
            return new Vector2D((direction.X / length) * PISTOL_PROJECTILE_SPEED, (direction.Y / length) * PISTOL_PROJECTILE_SPEED);
        }

        public void Update()
        {
            foreach (var ac in _attackComponents)
            {
                if ((timer.GetTicks() > startTick + fire_rate) && ac.GameObject.GetComponent<AttackComponent>().AttackType == AttackType.SHOOT && ac.GameObject.Active)
                {
                    startTick = timer.GetTicks();
                    int bulletNr = 0;
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        if (!bullets[i].Active)
                        {
                            bullets[i].Transform.Position = ac.GameObject.Transform.Position;
                            bullets[i].Transform.Direction = GetDirection(PlayerControl.Instance.Player.GameObject.Transform.Position, bullets[i].Transform.Position);
                            bullets[i].Transform.Rotation = Vector2D.GetAngle(Vector2D.DOWN, bullets[i].Transform.Direction);
                            bullets[i].Active = true;
                            bulletNr = i;
                            break;
                        }
                    }
                    //Console.WriteLine("Mouse pointer at: " + dir);
                    GameObject bullet = bullets[bulletNr];
                }
            }


            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].Active)
                {
                    if (bullets[i].Transform.Position.X - CameraSystem.cameraRect.x < 0 ||
                        bullets[i].Transform.Position.X - CameraSystem.cameraRect.x > Program.SCREEN_WIDTH ||
                        bullets[i].Transform.Position.Y - CameraSystem.cameraRect.y < 0 ||
                        bullets[i].Transform.Position.Y - CameraSystem.cameraRect.y > Program.SCREEN_HEIGHT)
                    {
                        bullets[i].Active = false;
                    }
                    //Console.WriteLine("Bullet " + i + "Active: " + bullets[i].Active + bullets[i].Transform.Position);

                    bullets[i].Transform.Position += new Vector2D(bullets[i].Transform.Direction.X, bullets[i].Transform.Direction.Y);
                }
            }
        }
    }
}
