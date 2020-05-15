using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreakerConsoleApp
{
    class Collisions
    {
        public static bool Collision_Ball_Paddle(Ball ball, Paddle paddle)
        {
            if (CheckForCollision(ball, paddle))
            {
                Vector2D direction = new Vector2D();
                if (paddle.PlacedPosition == Paddle.Placed.TOP)
                {
                    direction = new Vector2D(paddle.GetReflection(ball), 1);
                   // Console.WriteLine("New Direction: " + direction);
                    ball.SetDirection(direction);
                    return true;
                }
                else if (paddle.PlacedPosition == Paddle.Placed.BOTTOM)
                {
                    direction = new Vector2D(paddle.GetReflection(ball), -1);
                   // Console.WriteLine("New Direction: " + direction);
                    ball.SetDirection(direction);
                    return true;
                }
                else
                {
                    Console.WriteLine("Collision Detection got fucked up!");
                    return false;
                }
            }
            return false;
        }

        public static bool Collision_Wall_Handler(Ball ball, PlayingField playingField)
        {
            if (ball.Transform.Position.X + ball.Transform.Dimension.Width >= playingField.Width || ball.Transform.Position.X <= 0 )
            {
               // Console.WriteLine("WallCollision detected");
                Vector2D newDirection = new Vector2D(ball.Transform.Direction.X * -1, ball.Transform.Direction.Y);
                ball.SetDirection(newDirection);
                return true;
            }
            return false;
        }
        
        public static bool CheckForCollision(ICollideable obj_1, ICollideable obj_2)
        {
            if (obj_1.Transform.Position.X + obj_1.Transform.Dimension.Width >= (int)obj_2.Transform.Position.X && obj_1.Transform.Position.X <= (int)obj_2.Transform.Position.X + (int)obj_2.Transform.Dimension.Width &&
                obj_1.Transform.Position.Y + obj_1.Transform.Dimension.Height >= (int)obj_2.Transform.Position.Y && obj_1.Transform.Position.Y <= (int)obj_2.Transform.Position.Y + (int)obj_2.Transform.Dimension.Height)
            {
               //Console.WriteLine("Collision detected");
                return true;
            }
            return false;
        }

        public static void Collision_Brick_Handler(Ball ball, ICollideable obj)
        {
            /*
            double ypos = obj.Transform.Position.Y + obj.Transform.Dimension.Height;
            double xpos = obj.Transform.Position.X + obj.Transform.Dimension.Width;
            Console.WriteLine("Ball position: " + ball.Transform.Position.X + "      y:   " + ball.Transform.Position.Y);
            Console.WriteLine("Brick corner Unten rechts: " + obj.Transform.Position.X + "      y:   " + obj.Transform.Position.Y);
           */
            Console.WriteLine("Collision detected");
            if (ball.Transform.Position.X + obj.Transform.Dimension.Width >= (int)obj.Transform.Position.X && obj.Transform.Position.X <= (int)obj.Transform.Position.X + (int)obj.Transform.Dimension.Width &&
        ball.Transform.Position.Y + ball.Transform.Dimension.Height >= (int)obj.Transform.Position.Y && obj.Transform.Position.Y <= (int)obj.Transform.Position.Y + (int)obj.Transform.Dimension.Height)
            {
                if (ball.Transform.Position.Y < obj.Transform.Position.Y  &&
                ball.Transform.Position.X < obj.Transform.Position.X + obj.Transform.Dimension.Width -20 &&
                ball.Transform.Position.X > obj.Transform.Position.X + 20) // 1
                {
                    // ball.Transform.Position = new Vector2D(ball.Transform.Position.X, ball.Transform.Position.Y - 0.03);// Move out of collision
                    Console.WriteLine("1");
                    Vector2D newDirection = new Vector2D(ball.Transform.Direction.X, ball.Transform.Direction.Y * -1);
                    ball.SetDirection(newDirection);
                    return;
                }

                if (ball.Transform.Position.Y < obj.Transform.Position.Y + obj.Transform.Dimension.Height &&
                    ball.Transform.Position.X + ball.Transform.Dimension.Width < obj.Transform.Position.X + obj.Transform.Dimension.Width -5 &&
                    ball.Transform.Position.X > obj.Transform.Position.X + 5) // 5
                {
                    Console.WriteLine("5");
                    //ball.Transform.Position = new Vector2D(ball.Transform.Position.X, ball.Transform.Position.Y + 0.02);// Move out of collision
                    Vector2D newDirection = new Vector2D(ball.Transform.Direction.X, ball.Transform.Direction.Y * -1 + 0.03);
                    ball.SetDirection(newDirection);
                    return;
                }

                //DOESNT WORK SOMEHOW BUT 3 ALSO WORKS FOR THE LEFT SIDE SO FAR SO IT DOESNT MATTER ANYWAYS

                /*
                if (ball.Transform.Position.X + ball.Transform.Dimension.Width < ball.Transform.Position.X
                    && ball.Transform.Position.Y < obj.Transform.Position.Y + brick.Transform.Dimension.Height
                    && ball.Transform.Position.Y > brick.Transform.Position.Y) // 7
                {
                    Console.WriteLine("7");
                    // ball.Transform.Position = new Vector2D(ball.Transform.Position.X - ball.Transform.Dimension.Width- 0.02 , ball.Transform.Position.Y );// Move out of collision
                    Vector2D newDirection = new Vector2D(ball.Transform.Direction.X * -1, ball.Transform.Direction.Y);
                    ball.SetDirection(newDirection);
                    return;
                }
                */

                //WORKS FOR BOTH SIDES
                //leave 5 pixels for the corners....you can try to make it smaller but works perfects for me (dania)
                if (ball.Transform.Position.X < obj.Transform.Position.X + obj.Transform.Dimension.Width &&
                    ball.Transform.Position.Y < obj.Transform.Position.Y + obj.Transform.Dimension.Height - 3 &&
                    ball.Transform.Position.Y > obj.Transform.Position.Y + 3) // 3
                {
                    Console.WriteLine("3");
                    Vector2D newDirection = new Vector2D(ball.Transform.Direction.X * -1, ball.Transform.Direction.Y);
                    ball.SetDirection(newDirection);
                    // double test = 0.02 * ball.Transform.Direction.Y;
                    //Console.WriteLine(test);
                    // ball.Transform.Position = new Vector2D(ball.Transform.Position.X + 0.02 * ball.Transform.Direction.X, ball.Transform.Position.Y *  + 0.02 * ball.Transform.Direction.Y);// Move out of collision
                    return;
                }
                /*
                if (ball.Transform.Position.X < obj.Transform.Position.X + obj.Transform.Dimension.Width &&
                      ball.Transform.Position.Y < obj.Transform.Position.Y + obj.Transform.Dimension.Height &&
                     ball.Transform.Position.Y > obj.Transform.Position.Y) // 3
                {
                    Console.WriteLine("corner collison");
                    //  ball.Transform.Position = new Vector2D(ball.Transform.Position.X + 0.02, ball.Transform.Position.Y);// Move out of collision
                    Vector2D newDirection = new Vector2D(ball.Transform.Direction.X, ball.Transform.Direction.Y * -1);
                   ball.Transform.Position = new Vector2D(ball.Transform.Position.X , ball.Transform.Position.Y + 2);// Move out of collision
                    ball.SetDirection(newDirection);
                    return;
                }
                */

                /*
                if (ball.Transform.Position.X < obj.Transform.Position.X
                    && ball.Transform.Position.Y < obj.Transform.Position.Y)
                {
                    Console.WriteLine("corner collison upper corner left");
                    Vector2D newDirection = new Vector2D(ball.Transform.Direction.X *-1, ball.Transform.Direction.Y * -1);
                    ball.Transform.Position = new Vector2D(ball.Transform.Position.X - 5, ball.Transform.Position.Y);// Move out of collision
                    ball.SetDirection(newDirection);
                    return;
                }
                */

                Console.WriteLine("corner");
                Vector2D dir = new Vector2D(ball.Transform.Direction.X *-1, ball.Transform.Direction.Y * -1);
                ball.SetDirection(dir);
                return;
            }
        }
    }
}
