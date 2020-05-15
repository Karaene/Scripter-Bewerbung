using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class Event : IRenderable
    {
        Paddle paddle;
        Ball ball;
        Brick brick;
        Random rnd = new Random();
        private Rectangle eventRect;
        private Transform transform;
        private Dimension defaultSize = new Dimension(24, 24);
        private static Color color = new Color(056, 056, 056);
        private const float SPEED = 2f;
        bool eventStatus = false;
        bool eventFinished = false;
        Timer myTimer;
        Game game;
        bool paddleEventPlus = false;
        bool paddleEventMinus = false;
        bool pointEvent = false;
        bool damageEvent = false;
        bool isHit = false;
        Boss boss;

        public bool EventStarted
        {
            get { return eventStatus; }
        }
        public bool EventFinished
        {
            get { return eventFinished; }
        }


        public Event(Ball ball, Paddle pad, Brick brick, Game game)
        {
            this.ball = ball;
            this.paddle = pad;
            this.brick = brick;
            this.game = game;
            myTimer = new Timer();
            rnd = new Random();
        }

        public Event(Ball ball, Paddle pad, Boss boss, Game game)
        {
            this.ball = ball;
            this.paddle = pad;
            this.boss = boss;
            this.game = game;
            myTimer = new Timer();
            rnd = new Random();
        }

        private void RandomFunction()
        {
            int randomNumber = rnd.Next(0, 100);
            //60% Buff chance 40 % Debuff chance
            if (randomNumber < 75)
            {
                //Buffs
                if (0 <= randomNumber && randomNumber < 25)
                {
                    IncreasePaddleWidth();
                    paddleEventPlus = true;
                }
                if (25 <= randomNumber && randomNumber < 50)
                {
                    AddPointMultiplier();
                    pointEvent = true;
                }
                if( 50<= randomNumber)
                {
                    IncreaseLifepoints();
                }
            }

            else
            {
                DecreasePaddleWidth();
                paddleEventMinus = true;
            }

        }
        void RandomFunctionBoss()
        {
            int randomNumber = rnd.Next(0, 100);
                if (0 <= randomNumber && randomNumber < 25)
                {
                    IncreasePaddleWidth();
                    paddleEventPlus = true;
                }
                if (25 <= randomNumber && randomNumber < 50)
                {
                    AddPointMultiplier();
                    pointEvent = true;
                }
                if(50 <= randomNumber && randomNumber < 75)
                {
                    IncreaseBossDamage();
                    damageEvent = true;
                }
                if (75 <= randomNumber)
                {
                    IncreaseLifepoints();
                }
        }

        public void CreateEvent()
        {
            if (isHit == true)
            {
                if (eventStatus == false)
                {
                    Console.WriteLine("Started");
                    myTimer.stop();
                    myTimer.start();
                    eventStatus = true;
                    if (brick != null)
                    {
                        RandomFunction();
                    }
                    else
                    {
                        RandomFunctionBoss();
                    }
                }
                if ((myTimer.get_ticks() / 1000f) > 5)
                {
                    Console.WriteLine("Ended");
                    Reset();
                }
            }

        }

        private bool OutOfBounds(int sizeY)
        {
            if ((eventRect.Y > sizeY || eventRect.Y < 0) && isHit == false)
            {
                return true;
            }
            return false;
        }

        private bool EventCollisionDetection()
        {
            if (eventRect.X + eventRect.Width > paddle.Transform.Position.X && eventRect.X < paddle.Transform.Position.X + paddle.Transform.Dimension.Width &&
                eventRect.Y + eventRect.Height > paddle.Transform.Position.Y && eventRect.Y < paddle.Transform.Position.Y + paddle.Transform.Dimension.Height)
            {
                return true;
            }
            return false;
        }

        public void CheckCollision()
        {
            if (EventCollisionDetection() == true)
            {
                isHit = true;
            }
        }

        public void CheckOutOfBounds(int sizeY)
        {
            if (OutOfBounds(sizeY) == true)
            {
                eventFinished = true;
            }
        }

        public void FallDown()
        {
            if (eventStatus == false)
            {
                CreateRect();
                if (paddle.PlacedPosition == Paddle.Placed.TOP)
                {
                    Vector2D direction = new Vector2D(0, -1);
                    SetDirection(direction);
                }
                else
                {
                    Vector2D direction = new Vector2D(0, 1);
                    SetDirection(direction);
                }
            }
        }

        public void FallDownFromBoss()
        {
            if(eventStatus == false && game.bossSpawned)
            {
                CreateRectBoss();
                if (paddle.PlacedPosition == Paddle.Placed.TOP)
                {
                    Vector2D direction = new Vector2D(0, -1);
                    SetDirection(direction);
                }
                else
                {
                    Vector2D direction = new Vector2D(0, 1);
                    SetDirection(direction);
                }
            }
        }

        private void CreateRectBoss()
        {
            double xpos = boss.Transform.Position.X + boss.Transform.Dimension.Width / 2 - defaultSize.Width/2;
            double ypos = boss.Transform.Position.Y + boss.Transform.Dimension.Height / 2 - defaultSize.Height / 2;
            transform = new Transform(new Vector2D(xpos, ypos), defaultSize);
            eventRect = new Rectangle(transform.Position, transform.Dimension, color);
        }

        private void IncreasePaddleWidth()
        {
            //paddle.paddleRect.Width += 40;
            paddle.Transform.Dimension= new Dimension(paddle.Transform.Dimension.Width+ 40, paddle.Transform.Dimension.Height);
        }
        private void DecreasePaddleWidth()
        {
            //paddle.paddleRect.Width -= 40;
            paddle.Transform.Dimension = new Dimension(paddle.Transform.Dimension.Width - 40, paddle.Transform.Dimension.Height);
        }

        private void IncreaseLifepoints()
        {
            game.playerLifes += 1;
        }
        private void AddPointMultiplier()
        {
            game.PointMultiplier += 5;
        }
        private void ResetPointMultiplier()
        {
            game.PointMultiplier -= 5;
        }
        private void IncreaseBossDamage()
        {
            ball.damage += 5;
        }
        private void DecreaseBossDamge()
        {
            ball.damage -= 5;
        }
        private void Reset()
        {
            if (paddleEventPlus == true)
            {
                DecreasePaddleWidth();

            }
            else if (pointEvent == true)
            {
                ResetPointMultiplier();

            }
            else if (paddleEventMinus == true)
            {
                IncreasePaddleWidth();
            } else if (damageEvent == true)
            {
                DecreaseBossDamge();
            }

            eventStatus = false;
            eventFinished = true;
        }

        private void CreateRect()
        {
            
            if (paddle.PlacedPosition == Paddle.Placed.TOP)
            {
                double xpos = brick.Transform.Position.X + brick.Transform.Dimension.Width / 2;
                double ypos = brick.Transform.Position.Y;
                transform = new Transform(new Vector2D(xpos, ypos), defaultSize);
                eventRect = new Rectangle(transform.Position, transform.Dimension, color);
            }
            else
            {
          
                double xpos = brick.Transform.Position.X + brick.Transform.Dimension.Width / 2;
                double ypos = brick.Transform.Position.Y + brick.Transform.Dimension.Height;
                transform = new Transform(new Vector2D(xpos, ypos), defaultSize);
                eventRect = new Rectangle(transform.Position, transform.Dimension, color);
            }
        }

        private void SetDirection(Vector2D direction)
        {
            double length = direction.Norm();
            transform.Direction = new Vector2D(SPEED * (direction.X / length), SPEED * direction.Y / length);
        }
        public void Update()
        {
            transform.Position += new Vector2D(transform.Direction.X, transform.Direction.Y);
            eventRect.X = (int)transform.Position.X;
            eventRect.Y = (int)transform.Position.Y;
        }

        public void Render(IntPtr _Renderer)
        {
            eventRect.Render(_Renderer);
        }

    }
}
