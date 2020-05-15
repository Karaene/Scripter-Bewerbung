using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class HealthSystem : ISystem
    {
        private LTimer myTimer = new LTimer();
        private bool tookDamage = false;
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

        public void Init()
        {
            EventSystem.Instance.AddListener("TakeDamage", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    if (!tookDamage)
                    {
                        myTimer.Start();
                        tookDamage = true;
                    }

                   // Console.WriteLine(myTimer.GetTicks());
                    if ((myTimer.GetTicks() / 1000f) > 0.3)
                    {
                        tookDamage = false;
                    if (PlayerControl.Instance.Player.GameObject.GetComponent<HealthComponent>().Health > 0)
                    {
                        PlayerControl.Instance.Player.GameObject.GetComponent<HealthComponent>().Health -= (int)parameters[1];
                        if (PlayerControl.Instance.Player.GameObject.GetComponent<HealthComponent>().Health < 0)
                        {
                            PlayerControl.Instance.Player.GameObject.GetComponent<HealthComponent>().Health = 0;
                        }
                    }
                    }
                }
            });
        }

        public HealthComponent CreateHealthComponent(int health)
        {
            HealthComponent healthComponent = new HealthComponent(this ,health);
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
               // Console.WriteLine("GameOver");
            }

            GameObjectSystem.Instance.GetGameObject("health").GetComponent<TextComponent>().Text.SetText("" + PlayerControl.Instance.Player.GameObject.GetComponent<HealthComponent>().Health);
        }
    }
}
