using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    public class PhysicSystem : ISystem
    {
        LTimer timer = new LTimer();
        uint startTick = 0;
        public const int DELAY_MS = 30;

        private static SpatialHash _SpatialHash = new SpatialHash();
        Vector2D playerpos;
        private List<IEntityComponent> _activeColliders { get; set; } = new List<IEntityComponent>();
        private Vector2D previousPos;

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

        internal BoxColliderComponent CreateBoxCollider(GameObject obj)
        {
            BoxColliderComponent bc = new BoxColliderComponent(this, obj);
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
                _SpatialHash.RegisterObject(coll.GameObject);
            }
        }

        private void CheckCollisions()
        {
            foreach (var obj in _activeColliders)
            {
                foreach (var item in _SpatialHash.GetNearby(obj.GameObject))
                {   
                    if (obj.GetType() == typeof(BoxColliderComponent) && (item.GetComponent<BoxColliderComponent>() != null))
                    {
                            if (Collision((BoxColliderComponent)obj, item.GetComponent<BoxColliderComponent>()) && obj.GameObject.Active && item.Active)
                            {
                               EventSystem.Instance.RaiseEvent("Collision", obj.GameObject, item);
                            }
                    }
                    
                    if(obj.Type == Type.CIRCLE_COLLIDER && (item.GetComponent(Type.BOX_COLLIDER) != null))
                    {
                        if (Collision(item.GetComponent<BoxColliderComponent>(), (CircleColliderComponent)obj) && obj.GameObject.Active && item.Active)
                        {
                            EventSystem.Instance.RaiseEvent("Collision", obj.GameObject, item);
                        }
                    }
                    
                }
            }
        }
   


    private bool Collision(BoxColliderComponent box, CircleColliderComponent circle)
        {
            double DeltaX = circle.GameObject.Transform.Position.X - Math.Max(box.GameObject.Transform.Position.X, 
                            Math.Min(circle.GameObject.Transform.Position.X, box.GameObject.Transform.Position.X - box.GameObject.Transform.Size.Width/2));

            double DeltaY = circle.GameObject.Transform.Position.Y - Math.Max(box.GameObject.Transform.Position.Y,
                Math.Min(circle.GameObject.Transform.Position.Y, box.GameObject.Transform.Position.Y - box.GameObject.Transform.Size.Height/2));
            
            return (DeltaX * DeltaX + DeltaY * DeltaY) < (circle.Radius * circle.Radius);
        }

        private bool Collision(BoxColliderComponent box_1, BoxColliderComponent box_2)
        {
            if (box_1.GameObject.Transform.Position.X < box_2.GameObject.Transform.Position.X + box_2.GameObject.Transform.Size.Width &&
                box_1.GameObject.Transform.Position.X + box_1.GameObject.Transform.Size.Width > box_2.GameObject.Transform.Position.X &&
                box_1.GameObject.Transform.Position.Y < box_2.GameObject.Transform.Position.Y + box_2.GameObject.Transform.Size.Height &&
                box_1.GameObject.Transform.Position.Y + box_1.GameObject.Transform.Size.Height > box_2.GameObject.Transform.Position.Y)
            {
                return true;
            }
            return false;
        }

        public bool Overlaps(GameObject obj, GameObject other)
        {
            double minX = obj.Transform.Position.X - obj.Transform.Size.Width / 2;
            double maxX = obj.Transform.Position.X + obj.Transform.Size.Width / 2;
            double maxY = obj.Transform.Position.Y - obj.Transform.Size.Height / 2;
            double minY = obj.Transform.Position.Y + obj.Transform.Size.Height / 2;

            double other_MinX = other.Transform.Position.X - other.Transform.Size.Width / 2;
            double other_MaxX = other.Transform.Position.X + other.Transform.Size.Width / 2;
            double other_MaxY = other.Transform.Position.Y - other.Transform.Size.Height / 2;
            double other_MinY = other.Transform.Position.Y + other.Transform.Size.Height / 2;

            return (maxX > other_MinX &&
                    minX < other_MaxX &&
                    maxY > other_MinY &&
                    minY < other_MaxY);
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
            
        }
    }
}
