using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
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
        
        public void Init()
        {
            EventSystem.Instance.AddListener("Collision", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    GameObject go_1 = (GameObject)parameters[0];
                    GameObject go_2 = (GameObject)parameters[1];

                    if ((go_1.GetComponent<PlayerComponent>() != null) && (go_2.GetComponent<AiComponent>() != null))
                    {
                        Console.WriteLine("EnemyAttack!");
                        EventSystem.Instance.RaiseEvent("TakeDamage", go_1, go_2.GetComponent<AttackComponent>().Damage);
                    }
                    /*
                    if((go_1.GetComponent<AiComponent>() != null) && (go_2.GetComponent<ObstacleComponent>() != null))
                    {
                        go_1.GetComponent<BoxColliderComponent>().HitCollider = true;
                        go_1.GetComponent<BoxColliderComponent>().Point = go_2.Transform.Position;
                        //Console.WriteLine("Collided with wall");
                    }
                    */
                    if ((go_1.GetComponent<BulletComponent>() != null) && go_2.GetComponent<AiComponent>() != null && go_1.GetComponent<BulletComponent>().Friendly)
                    {
                        go_1.Active = false;
                        go_2.Active = false;
                        go_2.GetComponent<BoxColliderComponent>().HitCollider = false;
                        EventSystem.Instance.RaiseEvent("ShotEnemy");
                        Console.WriteLine("ShotEnemy: " + go_2.Id);
                    }

                    if ((go_1.GetComponent<BulletComponent>() != null) && go_2.Id == "enemyShooting" && go_1.GetComponent<BulletComponent>().Friendly)
                    {
                        go_1.Active = false;
                        go_2.Active = false;
                        go_2.GetComponent<BoxColliderComponent>().HitCollider = false;
                        EventSystem.Instance.RaiseEvent("ShotEnemy");
                        Console.WriteLine("ShotEnemy: " + go_2.Id);
                    }

                    if ((go_1.GetComponent<BulletComponent>() != null) && go_2.GetComponent<PlayerComponent>() != null && !go_1.GetComponent<BulletComponent>().Friendly)
                    {
                        go_1.Active = false;
                        //go_2.Active = false;
                        //go_2.GetComponent<BoxColliderComponent>().HitCollider = false;
                        Console.WriteLine("AUA!!!");
                        EventSystem.Instance.RaiseEvent("TakeDamage", go_2, go_1.GetComponent<BulletComponent>().Damage);
                    }

                    if ((go_1.GetComponent<PickUpComponent>() != null) && (go_2.GetComponent<PlayerComponent>() != null))
                    {
                        Console.WriteLine("Collect " + go_1.Id);
                        go_1.Active = false;

                        if (go_1.GetComponent<PickUpComponent>().PickUpType == PickUpType.COIN)
                        {
                            EventSystem.Instance.RaiseEvent("EarnCoin");
                        }

                        if (go_1.GetComponent<PickUpComponent>().PickUpType == PickUpType.ITEM)
                        {
                            EventSystem.Instance.RaiseEvent("AddItem", go_2, go_1);
                            EventSystem.Instance.RaiseEvent("ChangeToWeapon", go_2, go_2.GetComponent<InventoryComponent>().Items.Count-1);
                        }
                        
                    }
                }
            });
        }
    }
}
