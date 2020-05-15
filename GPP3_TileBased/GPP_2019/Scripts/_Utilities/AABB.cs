using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    public struct AABB
    {
        public double Min_X { get; set; }
        public double Min_Y { get; set; }
        public double Max_X { get; set; }
        public double Max_Y { get; set; }

        public double SurfaceArea { get; set; }

        public AABB(double minX, double minY, double maxX, double maxY)
        {
            Min_X = minX;
            Min_Y = minY;
            Max_X = maxX;
            Max_Y = maxY;

            SurfaceArea = 0;
            SurfaceArea = CalculateSurface();
        }

        private double CalculateSurface()
        {
            return GetWidth() * GetHeight();
        }

        public bool Overlaps(AABB other)
        {
            Console.WriteLine("Check Overlap");
            return (Max_X > other.Min_X &&
                    Min_X < other.Max_X &&
                    Max_Y > other.Min_Y &&
                    Min_Y < other.Max_Y); 
        }

        public bool contains(AABB other)
        {
            return (other.Min_X >= Min_X &&
                    other.Max_X <= Max_X &&
                    other.Min_Y >= Min_Y &&
                    other.Max_Y <= Max_X);
        }

        private double GetWidth() { return Max_X - Min_X; }
        private double GetHeight() { return Max_Y - Min_Y; }

    }
}
