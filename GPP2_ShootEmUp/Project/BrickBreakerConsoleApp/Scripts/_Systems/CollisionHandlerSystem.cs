using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class CollisionHandlerSystem : ISystem
    {
        private CollisionHandlerSystem() { }
        private static CollisionHandlerSystem instance = null;
        public static CollisionHandlerSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CollisionHandlerSystem();
                }
                return instance;
            }
        }
        
        public void AddListeners()
        {
            EventSystem.Instance.AddListener("Collision", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    GameObject go_1 = (GameObject)parameters[0];
                    GameObject go_2 = (GameObject)parameters[1];
                    
                    if ((go_1.GetComponent<PlayerComponent>() != null) && (go_2.GetComponent<AiComponent>() != null))
                    {
                        EventSystem.Instance.RaiseEvent("TakeDamage", go_1, go_2.GetComponent<AttackComponent>().Damage);
                    }
                    if((go_1.GetComponent<AiComponent>() != null) && (go_2.GetComponent<ObstacleComponent>() != null))
                    {
                        go_1.GetComponent<BoxColliderComponent>().HitCollider = true;
                        go_1.GetComponent<BoxColliderComponent>().Point = go_2.Transform.Position;
                      //  Console.WriteLine("Collided with wall");
                    }
                    if ((go_1.GetComponent<BulletComponent>() != null) && go_2.GetComponent<AiComponent>() != null && go_1.GetComponent<BulletComponent>().Friendly)
                    {
                        go_1.Active = false;
                        go_2.Active = false;
                        go_2.GetComponent<BoxColliderComponent>().HitCollider = false;
                        EventSystem.Instance.RaiseEvent("ShotEnemy");
                        //Console.WriteLine("ShotEnemy: " + go_2.Id);
                    }
                    if ((go_1.GetComponent<BulletComponent>() != null) && go_2.Id == "enemyShooting" && go_1.GetComponent<BulletComponent>().Friendly)
                    {
                        go_1.Active = false;
                        go_2.Active = false;
                        go_2.GetComponent<BoxColliderComponent>().HitCollider = false;
                        EventSystem.Instance.RaiseEvent("ShotEnemy");
                       // Console.WriteLine("ShotEnemy: " + go_2.Id);
                    }
                    if ((go_1.GetComponent<BulletComponent>() != null) && go_2.GetComponent<PlayerComponent>() != null && !go_1.GetComponent<BulletComponent>().Friendly)
                    {
                        go_1.Active = false;
                        //go_2.Active = false;
                        //go_2.GetComponent<BoxColliderComponent>().HitCollider = false;
                      //  Console.WriteLine("AUA!!!");
                        EventSystem.Instance.RaiseEvent("TakeDamage", go_2, go_1.GetComponent<BulletComponent>().Damage);
                    }
                }
            });
        }
    }
}
