using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace GPP_2019
{
    public class PhysicSystem : ISystem
    {
        LTimer timer = new LTimer();
        uint startTick = 0;
        public const int DELAY_MS = 30;
        Vector2D lastPlayerPos;
        Vector2D lastEnemyPos;
        LTimer leveltimer = new LTimer();
        uint levelTicker = 0;
        bool waiting = false;
        private static SpatialHash _SpatialHash = new SpatialHash();
        private List<IEntityComponent> _activeColliders { get; set; } = new List<IEntityComponent>();

        private PhysicSystem()
        {
            _SpatialHash.Setup(100);
            timer.Start();

            EventSystem.Instance.AddListener("ShowNearbyCollider", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    List<GameObject> objs = _SpatialHash.GetNearby((GameObject)parameters[0]);
                    Console.WriteLine("Player could collide with: ");
                    foreach (var obj in objs)
                    {
                        Console.WriteLine(obj.Id);
                        EventSystem.Instance.RaiseEvent("DrawOutline", obj);
                    }
                }
            });
        }

        private static PhysicSystem instance = null;
        public static PhysicSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PhysicSystem();
                }
                return instance;
            }
        }
        /*
        internal BoxColliderComponent CreateBoxCollider(GameObject obj)
        {
            BoxColliderComponent bc = new BoxColliderComponent(this, obj);
            _activeColliders.Add(bc);
            return bc;
        }
        */
        internal BoxColliderComponent CreateBoxCollider(GameObject go, double offsetX, double offsetY)
        {
            BoxColliderComponent bc = new BoxColliderComponent(this, new Size(go.Transform.Size.Width + offsetX, go.Transform.Size.Height + offsetY));
            _activeColliders.Add(bc);
            return bc;
        }

        internal CircleColliderComponent CreateCircleCollider(GameObject obj, int radius)
        {
            CircleColliderComponent cc = new CircleColliderComponent(this);
            cc.GameObject = obj;
            cc.Radius = radius;
            _activeColliders.Add(cc);
            return cc;
        }

        internal ObstacleComponent CreateObstacleComponent()
        {
            ObstacleComponent obstacle = new ObstacleComponent(this);
            _activeColliders.Add(obstacle);
            return obstacle;
        }

        private void RegisterColliderGameObjects()
        {
            foreach (var coll in _activeColliders)
            {
                if (coll.GameObject.Active)
                    _SpatialHash.RegisterObject(coll.GameObject);
            }
        }

        private void CheckCollisions()
        {
            foreach (var obj in _activeColliders)
            {
                    foreach (var item in _SpatialHash.GetNearby(obj.GameObject))
                    {
                        if (obj.GameObject.Active && item.Active)
                        {
                            if (obj.GetType() == typeof(BoxColliderComponent) && (item.GetComponent<BoxColliderComponent>() != null))
                            {
                                if (Collision((BoxColliderComponent)obj, item.GetComponent<BoxColliderComponent>()))
                                {
                                  EventSystem.Instance.RaiseEvent("Collision", obj.GameObject, item);
                                }
                            }

                            if (obj.GetType() == typeof(CircleColliderComponent) && (item.GetComponent<BoxColliderComponent>() != null))
                            {
                                if (Collision(item.GetComponent<BoxColliderComponent>(), (CircleColliderComponent)obj))
                                {
                                    EventSystem.Instance.RaiseEvent("Collision", obj.GameObject, item);
                                }
                            }
                        }
                    }
                
            }
        }

        public bool CheckEnemyCollision(GameObject obj)
        {
            if (obj.GetComponent<AiComponent>() != null)
            {
                foreach (var item in _SpatialHash.GetNearby(obj))
                {
                    if (obj.Active && item.Active)
                    {
                        if ((obj.GetComponent<BoxColliderComponent>() != null) && (item.GetComponent<BoxColliderComponent>() != null))
                        {
                            if (item.GetComponent<TileComponent>() != null)
                            {
                                if (item.GetComponent<TileComponent>().walkable == false)
                                {
                                    if (Collision(obj.GetComponent<BoxColliderComponent>(), item.GetComponent<BoxColliderComponent>()))
                                    {
                                        EventSystem.Instance.RaiseEvent("Collision", obj, item);
                                        return true;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            return false;
        }

        private bool CheckPlayerTileCollision()
        {
            foreach (var item in _SpatialHash.GetNearby(PlayerControl.Instance.Player.GameObject))
            {
                if (PlayerControl.Instance.Player.GameObject.Active && item.Active)
                {
                    if (item.GetComponent<BoxColliderComponent>() != null && item.GetComponent<TileComponent>() != null && item.GetComponent<TileComponent>().walkable == false)
                    {
                        //if (CollisionSystem.Instance.Collision(PlayerControl.Instance.Player.GameObject,item))                      
                        if (Collision((BoxColliderComponent)PlayerControl.Instance.Player.GameObject.GetComponent<BoxColliderComponent>(), item.GetComponent<BoxColliderComponent>()))
                        {
                            PlayerControl.Instance.Player.GameObject.Transform.Position = lastPlayerPos;
                            return true;
                        }
                    } else if (item.GetComponent<BoxColliderComponent>() != null && item.GetComponent<TileComponent>() != null && item.GetComponent<TileComponent>().door == true && item.GetComponent<TileComponent>().Level == TileSystem.Instance.CurrentLevel)
                        {
                            leveltimer.Start();
                            if (Collision((BoxColliderComponent)PlayerControl.Instance.Player.GameObject.GetComponent<BoxColliderComponent>(), item.GetComponent<BoxColliderComponent>()))
                            {
                                lastPlayerPos = PlayerControl.Instance.Player.GameObject.Transform.Position;
                                if (!waiting) {
                                    waiting = true;
                                    if (TileSystem.Instance.CurrentLevel == 1) {
                                        TileSystem.Instance.SwapLevels(0);
                                        Console.WriteLine("HIT THE DOOR");
                                    }
                                    else
                                    {
                                        TileSystem.Instance.SwapLevels(1);
                                        Console.WriteLine("HIT THE DOOR");
                                    }
                                    return true;
                                }
                            }
                        }
                }
            }
            return false;
        }

        private bool Collision(BoxColliderComponent box, CircleColliderComponent circle)
        {
            double DeltaX = circle.GameObject.Transform.Position.X - Math.Max(box.GameObject.Transform.Position.X,
                            Math.Min(circle.GameObject.Transform.Position.X, box.GameObject.Transform.Position.X - box.GameObject.Transform.Size.Width / 2));

            double DeltaY = circle.GameObject.Transform.Position.Y - Math.Max(box.GameObject.Transform.Position.Y,
                Math.Min(circle.GameObject.Transform.Position.Y, box.GameObject.Transform.Position.Y - box.GameObject.Transform.Size.Height / 2));

            return (DeltaX * DeltaX + DeltaY * DeltaY) < (circle.Radius * circle.Radius);
        }

        /*
        private bool Collision(BoxColliderComponent box, CircleColliderComponent circle)
        {
            SDL_Rect circleDistance = new SDL_Rect();

            SDL_Rect rect = new SDL_Rect();
            rect.x = (int)(box.GameObject.Transform.Position.X - (int)box.GameObject.Transform.Size.Width / 2) - CameraSystem.cameraRect.x;
            rect.y = (int)(box.GameObject.Transform.Position.Y - (int)box.GameObject.Transform.Size.Height / 2) - CameraSystem.cameraRect.y;
            rect.x += (int)(box.GameObject.Transform.Size.Width - box.Size.Width) / 2;
            rect.y += (int)(box.GameObject.Transform.Size.Height - box.Size.Height) / 2;
            rect.w = (int)box.Size.Width;
            rect.h = (int)box.Size.Height;

            Vector2D circlePos = new Vector2D(  (circle.GameObject.Transform.Position.X - circle.Radius) - CameraSystem.cameraRect.x,
                                                (circle.GameObject.Transform.Position.Y - circle.Radius) - CameraSystem.cameraRect.y);

            circleDistance.x = (int)Math.Abs(circlePos.X - rect.x);
            circleDistance.y = (int)Math.Abs(circlePos.Y - rect.y);

            return ((circleDistance.x - rect.w/2) * (circleDistance.x - rect.w / 2) + (circleDistance.y - rect.h/2) * (circleDistance.y - rect.h / 2)) <= (circle.Radius) * (circle.Radius);
        }
        */
        
        private bool Collision(BoxColliderComponent box_1, BoxColliderComponent box_2)
        {
            SDL_Rect rect_1 = new SDL_Rect();
            SDL_Rect rect_2 = new SDL_Rect();

            rect_1.x = (int)(box_1.GameObject.Transform.Position.X - (int)box_1.GameObject.Transform.Size.Width / 2) - CameraSystem.cameraRect.x;
            rect_1.y = (int)(box_1.GameObject.Transform.Position.Y - (int)box_1.GameObject.Transform.Size.Height / 2) - CameraSystem.cameraRect.y;
            rect_1.x += (int)(box_1.GameObject.Transform.Size.Width - box_1.Size.Width) / 2;
            rect_1.y += (int)(box_1.GameObject.Transform.Size.Height - box_1.Size.Height) / 2;
            rect_1.w = (int)box_1.Size.Width;
            rect_1.h = (int)box_1.Size.Height;

            rect_2.x = (int)(box_2.GameObject.Transform.Position.X - (int)box_2.GameObject.Transform.Size.Width / 2) - CameraSystem.cameraRect.x;
            rect_2.y = (int)(box_2.GameObject.Transform.Position.Y - (int)box_2.GameObject.Transform.Size.Height / 2) - CameraSystem.cameraRect.y;
            rect_2.x += (int)(box_2.GameObject.Transform.Size.Width - box_2.Size.Width) / 2;
            rect_2.y += (int)(box_2.GameObject.Transform.Size.Height - box_2.Size.Height) / 2;
            rect_2.w = (int)box_2.Size.Width;
            rect_2.h = (int)box_2.Size.Height;

            if (rect_1.x < rect_2.x + rect_2.w &&
                rect_1.x + rect_1.w > rect_2.x &&
                rect_1.y < rect_2.y + rect_2.h &&
                rect_1.y + rect_1.h > rect_2.y)
            {
                return true;
            }
            return false;
        }

        public void Update()
        {

            _SpatialHash.ClearBuckets();
            RegisterColliderGameObjects();

            if (timer.GetTicks() > startTick + DELAY_MS)
            {
               startTick = timer.GetTicks();          
               CheckCollisions();
            }
            
            if (!CheckPlayerTileCollision())
            {
                lastPlayerPos = PlayerControl.Instance.Player.GameObject.Transform.Position;
            }
            levelTicker = leveltimer.GetTicks();
            if (levelTicker >= 5000)
            {
                waiting = false;
            }
        }
    }
}
