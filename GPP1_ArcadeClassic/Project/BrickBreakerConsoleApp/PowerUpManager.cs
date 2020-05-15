using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace BrickBreakerConsoleApp
{
    class PowerUpManager
    {
        public bool EventStarted { get; }
        public bool EventFinished { get; }
        static uint lastTime = 0, currentTime;

        private const int BIGGER_PADDLE_SECONDS = 5;
        private const int BIGGER_POINTS_MULTIPLIER_SECONDS = 5;
        private const int CHANGED_BALL_SPEED_SECONDS = 5;

        public static void RandomPowerUp()
        {
            Random rnd = new Random();
            int randomNumber = rnd.Next(0, 100);

            if (randomNumber < 60)
            {

            }
        }

        public static void UpdateBuffs(Paddle paddle_Bottom, Paddle paddle_Top, Ball ball)
        {
            if (paddle_Bottom.paddleSizeBuff)
            {
                currentTime = SDL_GetTicks();
                if (currentTime > lastTime + BIGGER_PADDLE_SECONDS * 1000)
                {
                    Console.WriteLine("Another 5 Seconds gone!");
                    ResetPaddle(paddle_Bottom);
                    paddle_Bottom.paddleSizeBuff = false;
                    lastTime = currentTime;
                }
            }
            if (paddle_Top.paddleSizeBuff)
            {
                currentTime = SDL_GetTicks();
                if (currentTime > lastTime + BIGGER_PADDLE_SECONDS * 1000)
                {
                    Console.WriteLine("Another 5 Seconds gone!");
                    ResetPaddle(paddle_Top);
                    paddle_Top.paddleSizeBuff = false;
                    lastTime = currentTime;
                }
            }
        }

        public static void BiggerPaddle(Paddle paddle)
        {
            int paddleWidth = (int)paddle.Transform.Dimension.Width;
            ChangePaddleWidth(paddle, paddleWidth + 80);
            paddle.Transform.Position = new Vector2D(paddle.Transform.Position.X - 40, paddle.Transform.Position.Y);
            paddle.paddleSizeBuff = true;
        }

        public static void ResetPaddle(Paddle paddle)
        {
            ChangePaddleWidth(paddle, Game.PADDLE_WIDTH);
            paddle.Transform.Position = new Vector2D(paddle.Transform.Position.X + 40, paddle.Transform.Position.Y);
        }

        public static void ChangePaddleWidth(Paddle paddle, int width)
        {
            paddle.Transform.Dimension = new Dimension(width, paddle.Transform.Dimension.Height);
        }

        public static void ChangePointsMultiplier(int currentMultiplier, int newMultiplier)
        {
            currentMultiplier = newMultiplier;
        }
        
        public static void ChangeBallSpeed(Ball ball, double speed)
        {
            ball.Speed = speed;
        }
    }
}
