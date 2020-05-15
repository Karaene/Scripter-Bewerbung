using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class HealthSystem : ISystem
    {
        private HealthSystem() { }
        private static HealthSystem instance = null;
        public static HealthSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HealthSystem();
                }
                return instance;
            }
        }

        private bool takingDamage = false;
        private LTimer timer = new LTimer();
        private uint startTick = 0;
        private const int DELAY_MS = 750;

        public void Init()
        {
            EventSystem.Instance.AddListener("TakeDamage", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    if (PlayerControl.Instance.Player.GameObject.GetComponent<HealthComponent>().Health > 0 && !takingDamage)
                    {
                        takingDamage = true;
                        timer.Start();
                        PlayerControl.Instance.Player.GameObject.GetComponent<HealthComponent>().Health -= (int)parameters[1];
                        if (PlayerControl.Instance.Player.GameObject.GetComponent<HealthComponent>().Health < 0)
                        {
                            PlayerControl.Instance.Player.GameObject.GetComponent<HealthComponent>().Health = 0;
                        }
                    }
                }
            });

            EventSystem.Instance.AddListener("IncreaseHealth", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    PlayerControl.Instance.Player.GameObject.GetComponent<HealthComponent>().Health = (int)parameters[0];
                    Console.WriteLine("Health: " + (int)parameters[0]);
                }
            });
            
        }


        public HealthComponent CreateHealthComponent(int health)
        {
            HealthComponent healthComponent = new HealthComponent(this, health);
            return healthComponent;
        }

        public void SetHealth(HealthComponent healthComponent, int health)
        {
            healthComponent.Health = health;
        }

        public void Update()
        {
            //Console.WriteLine("Player Health: " + PlayerControl.Instance.Player.GameObject.GetComponent<HealthComponent>().Health);
            

            if (PlayerControl.Instance.Player.GameObject.GetComponent<HealthComponent>().Health <= 0)
            {
                PlayerControl.Instance.Player.GameObject.Active = false;
                EventSystem.Instance.RaiseEvent("GameOver");
                //Console.WriteLine("GameOver");
            }

            GameObjectSystem.Instance.GetGameObject("health").GetComponent<TextComponent>().Text.SetText("" + PlayerControl.Instance.Player.GameObject.GetComponent<HealthComponent>().Health);

            if (takingDamage)
            {
                if (timer.GetTicks() > startTick + DELAY_MS)
                {
                    timer.Stop();
                    takingDamage = false;
                }
            }
        }
    }
}
