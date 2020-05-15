using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace BrickBreakerConsoleApp
{
    class Sphere
    {
        /*
        double posX;
        double posY;
        double dirX;
        double dirY;
        */
        Transform transform;

        int radius;
        Sprite sprite;


        public Sphere(Vector2D pos, int radius)
        {
            transform = new Transform();
            this.transform.Position = pos;
            this.radius = radius;
        }

        public void SetPosX(double posX)
        {
            this.transform.Position = new Vector2D(this.transform.Position.Y, posX);
        }

        public void SetPosY(double posY)
        {
            this.transform.Position = new Vector2D(posY, this.transform.Position.X);
        }

        public double GetPosX()
        {
            return this.transform.Position.X;
        }

        public double GetPosY()
        {
            return this.transform.Position.Y;
        }

        public int GetRadius()
        {
            return radius;
        }

        /*
        public void SetDirection(float dirx, float diry)
        {
            // Normalize the direction vector and multiply with BALL_SPEED
            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            this.dirX = MOVE_SPEED * (dirX / length);
            this.dirY = MOVE_SPEED * (dirY / length);


            
        }
        */

        public void DrawSphere(IntPtr renderer)
        {
            sdl_ellipse(renderer, (int)this.transform.Position.X, (int)this.transform.Position.Y, radius, radius);
        }

        private void sdl_ellipse(IntPtr r, int x0, int y0, int radiusX, int radiusY)
        {
            SDL_SetRenderDrawColor(r, 255, 0, 0, 0);
            float pi = 3.14159265358979323846264338327950288419716939937510f;
            float pih = pi / 2.0f; //half of pi

            //drew  28 lines with   4x4  circle with precision of 150 0ms
            //drew 132 lines with  25x14 circle with precision of 150 0ms
            //drew 152 lines with 100x50 circle with precision of 150 3ms
            const int prec = 27; // precision value; value of 1 will draw a diamond, 27 makes pretty smooth circles.
            float theta = 0;     // angle that will be increased each loop

            //starting point
            int x = (int)((float)radiusX * Math.Cos(theta));//start point
            int y = (int)((float)radiusY * Math.Sin(theta));//start point
            int x1 = x;
            int y1 = y;

            //repeat until theta >= 90;
            float step = pih / (float)prec; // amount to add to theta each time (degrees)
            for (theta = step; theta <= pih; theta += step)//step through only a 90 arc (1 quadrant)
            {
                //get new point location
                x1 = (int)((float)radiusX * Math.Cos(theta) + 0.5); //new point (+.5 is a quick rounding method)
                y1 = (int)((float)radiusY * Math.Sin(theta) + 0.5); //new point (+.5 is a quick rounding method)

                //draw line from previous point to new point, ONLY if point incremented
                if ((x != x1) || (y != y1))//only draw if coordinate changed
                {
                    SDL_RenderDrawLine(r, x0 + x, y0 - y, x0 + x1, y0 - y1);//quadrant TR
                    SDL_RenderDrawLine(r, x0 - x, y0 - y, x0 - x1, y0 - y1);//quadrant TL
                    SDL_RenderDrawLine(r, x0 - x, y0 + y, x0 - x1, y0 + y1);//quadrant BL
                    SDL_RenderDrawLine(r, x0 + x, y0 + y, x0 + x1, y0 + y1);//quadrant BR
                }
                //save previous points
                x = x1;//save new previous point
                y = y1;//save new previous point
            }
            //arc did not finish because of rounding, so finish the arc
            if (x != 0)
            {
                x = 0;
                SDL_RenderDrawLine(r, x0 + x, y0 - y, x0 + x1, y0 - y1);//quadrant TR
                SDL_RenderDrawLine(r, x0 - x, y0 - y, x0 - x1, y0 - y1);//quadrant TL
                SDL_RenderDrawLine(r, x0 - x, y0 + y, x0 - x1, y0 + y1);//quadrant BL
                SDL_RenderDrawLine(r, x0 + x, y0 + y, x0 + x1, y0 + y1);//quadrant BR
            }

            SDL_SetRenderDrawColor(r, 255, 255, 255, 0);
        }

        public void Update(Vector2D pos)
        {
            transform.Position = pos;

        }
    }
}
