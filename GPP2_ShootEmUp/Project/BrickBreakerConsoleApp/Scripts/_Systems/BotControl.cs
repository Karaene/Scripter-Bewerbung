using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class BotControl : ISystem
    {
        private List<AiComponent> _activeBots = new List<AiComponent>();
        private const int ALLOWED_ENEMY_DISTANCE = 42;
        private const int ALLOWED_ENEMY_PLAYER_DISTANCE = 32;

        private BotControl() { }
        private static BotControl instance = null;
        public static BotControl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BotControl();
                }
                return instance;
            }
        }

        public AiComponent CreateAiComponent()
        {
            AiComponent ai = new AiComponent(this);
            _activeBots.Add(ai);
            return ai;
        }

        public Vector2D GetDirection(Vector2D enemy_pos, Vector2D player)
        {
            Vector2D enemyPosScreen = new Vector2D(enemy_pos.X + CameraSystem.cameraRect.x, enemy_pos.Y + CameraSystem.cameraRect.y);
            Vector2D playerPosScreen = new Vector2D(player.X + CameraSystem.cameraRect.x, player.Y + CameraSystem.cameraRect.y);
            Vector2D direction = enemyPosScreen - playerPosScreen;
            double length = direction.Norm();
            return new Vector2D((direction.X / length), (direction.Y / length));
        }
        /*
        public bool AnybodyAlive()
        {
            foreach (var bot in _activeBots)
            {
                if (bot.GameObject.Active)
                    return true;
                break;
            }
            return false;
        }
        */
        public void Update(double deltaTime)
        {
            Vector2D v1, v2, v3, v4;

            foreach (var bot in _activeBots)
            {
                if (bot.GameObject.Active)
                {
                    v1 = Rule_1(bot);
                    v2 = Rule_2(bot);
                    v3 = Rule_3(bot);
                    v4 = Rule_4(bot);

                    bot.GameObject.Transform.Direction = GetDirection(PlayerControl.Instance.Player.GameObject.Transform.Position, bot.GameObject.Transform.Position);

                    Vector2D botScreenPosition = new Vector2D(bot.GameObject.Transform.Position.X + CameraSystem.cameraRect.x, bot.GameObject.Transform.Position.Y + CameraSystem.cameraRect.y);
                    Vector2D playerScreenPosition = new Vector2D(PlayerControl.Instance.Player.GameObject.Transform.Position.X + CameraSystem.cameraRect.x, PlayerControl.Instance.Player.GameObject.Transform.Position.Y + CameraSystem.cameraRect.y);

                    if ((Math.Abs(botScreenPosition.X - playerScreenPosition.X) > ALLOWED_ENEMY_PLAYER_DISTANCE ||
                        Math.Abs(botScreenPosition.Y - playerScreenPosition.Y) > ALLOWED_ENEMY_PLAYER_DISTANCE) && !bot.GameObject.GetComponent<BoxColliderComponent>().HitCollider)
                    {
                        if (bot.GameObject.GetComponent<VelocityComponent>().Velocity.X < 0) { bot.GameObject.GetComponent<VelocityComponent>().Velocity *= -1; }
                        bot.GameObject.Transform.Position += new Vector2D(bot.GameObject.Transform.Direction.X * bot.GameObject.GetComponent<VelocityComponent>().Velocity.X + v2.X,
                                                                          bot.GameObject.Transform.Direction.Y * bot.GameObject.GetComponent<VelocityComponent>().Velocity.Y + v2.Y);
                    }
                    else if (bot.GameObject.GetComponent<BoxColliderComponent>().HitCollider)
                    {
                        
                        if (bot.GameObject.GetComponent<BoxColliderComponent>().Point.X < bot.GameObject.Transform.Position.X &&
                            bot.GameObject.GetComponent<BoxColliderComponent>().Point.Y < bot.GameObject.Transform.Position.Y)
                        {
                            bot.GameObject.Transform.Position += new Vector2D(5.5, 5.5);
                            bot.GameObject.GetComponent<BoxColliderComponent>().HitCollider = false;
                        }


                        if (bot.GameObject.GetComponent<BoxColliderComponent>().Point.X >= bot.GameObject.Transform.Position.X &&
                            bot.GameObject.GetComponent<BoxColliderComponent>().Point.Y >= bot.GameObject.Transform.Position.Y)
                        {
                            bot.GameObject.Transform.Position += new Vector2D(-5.5, -5.5);
                            bot.GameObject.GetComponent<BoxColliderComponent>().HitCollider = false;
                        }


                        if (bot.GameObject.GetComponent<BoxColliderComponent>().Point.Y < bot.GameObject.Transform.Position.Y &&
                            bot.GameObject.GetComponent<BoxColliderComponent>().Point.X >= bot.GameObject.Transform.Position.X)
                        {
                            bot.GameObject.Transform.Position += new Vector2D(-5.5, +5.5);
                            bot.GameObject.GetComponent<BoxColliderComponent>().HitCollider = false;
                        }

                        if (bot.GameObject.GetComponent<BoxColliderComponent>().Point.Y >= bot.GameObject.Transform.Position.Y &&
                            bot.GameObject.GetComponent<BoxColliderComponent>().Point.X < bot.GameObject.Transform.Position.X)
                        {
                            bot.GameObject.Transform.Position += new Vector2D(+5.5, -5.5);
                            bot.GameObject.GetComponent<BoxColliderComponent>().HitCollider = false;
                        }


                        /*
                        bot.GameObject.Transform.Position += new Vector2D(bot.GameObject.GetComponent<VelocityComponent>().Velocity.X * -1, bot.GameObject.GetComponent<VelocityComponent>().Velocity.Y * -1);
                        bot.GameObject.GetComponent<BoxColliderComponent>().HitCollider = false;
                        */
                    }

                    /*
                    bot.GameObject.GetComponent<VelocityComponent>().Velocity = bot.GameObject.GetComponent<VelocityComponent>().Velocity + v2  + v4;
                    bot.GameObject.GetComponent<VelocityComponent>().Velocity = new Vector2D(bot.GameObject.GetComponent<VelocityComponent>().Velocity.X / 5, 
                                                                                                bot.GameObject.GetComponent<VelocityComponent>().Velocity.Y / 5);
                    bot.GameObject.Transform.Position += bot.GameObject.GetComponent<VelocityComponent>().Velocity;
                    */
                }
            }
            /*
            if (!AnybodyAlive())
            {
                Console.WriteLine("All enemies down");
                EventSystem.Instance.RaiseEvent("killedEverybody");
            }
            */
        }

        

        public Vector2D Rule_1(AiComponent refBot)
        {
            Vector2D percievedCenter = Vector2D.ZERO;

            foreach (var bot in _activeBots)
            {
                if(refBot != bot)
                {
                    percievedCenter = percievedCenter + bot.GameObject.Transform.Position;
                }
            }

            percievedCenter = percievedCenter / (_activeBots.Count - 1);

            return (percievedCenter - refBot.GameObject.Transform.Position) / 100;
        }

        public Vector2D Rule_2(AiComponent refBot)
        {
            Vector2D center = Vector2D.ZERO;

            foreach (var bot in _activeBots)
            {
                if (bot.GameObject.Active)
                {
                    float enemyDistance = Vector2D.Distance(refBot.GameObject.Transform.Position, bot.GameObject.Transform.Position);

                    if (bot != refBot && enemyDistance <= ALLOWED_ENEMY_DISTANCE)
                    {
                        center = center - (bot.GameObject.Transform.Position - refBot.GameObject.Transform.Position);
                    }
                }
            }

            return center / 16;
        }

        public Vector2D Rule_3(AiComponent refBot)
        {
            Vector2D percievedVelocity = Vector2D.ZERO;

            foreach (var bot in _activeBots)
            {
                if (bot != refBot)
                {
                    percievedVelocity = percievedVelocity + bot.GameObject.GetComponent<VelocityComponent>().Velocity;
                }
            }

            percievedVelocity = percievedVelocity / (_activeBots.Count - 1);

            return (percievedVelocity - refBot.GameObject.GetComponent<VelocityComponent>().Velocity) / 8f;
        }

        public Vector2D Rule_4(AiComponent refBot)
        {
            if (PlayerControl.Instance.Player != null)
            {
                Vector2D target = PlayerControl.Instance.Player.GameObject.Transform.Position;

                return ((target - refBot.GameObject.Transform.Position) / 20f);
            }
            else
            {
                return (Vector2D.ZERO);
            }
        }
    }
}
