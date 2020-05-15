using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    public struct Vector2D
    {
        #region Attributes and Properties
        
        public double X { get; set; }
        public double Y { get; set; }

        public static Vector2D UP { get { return new Vector2D(0, 1); } }
        public static Vector2D DOWN { get { return new Vector2D(0, -1); } }
        public static Vector2D LEFT { get { return new Vector2D(-1, 0); } }
        public static Vector2D RIGHT { get { return new Vector2D(1, 0); } }
        public static Vector2D ZERO { get { return new Vector2D(0, 0); } }
        #endregion

        #region Constructors
        public Vector2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        #endregion

        #region Methodes
        public override bool Equals(object obj)
        {
            if (obj is Vector2D)
            {
                Vector2D v = (Vector2D)obj;
                if (v.X == X && v.Y == Y)
                    return obj.GetType().Equals(this.GetType());
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{{X={0}, Y={1}}}", X, Y);
        }

        public double Norm()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public static float Distance(Vector2D u, Vector2D v)
        {
            return (float)Math.Sqrt(Math.Pow(u.X - v.X, 2) + Math.Pow(u.Y - v.Y, 2));
        }
        
        /*
        public static double GetAngle(Vector2D u, Vector2D v)
        {
            return MathF.Acos((float)((u * v) / (u.Norm() * v.Norm()))) * (180/Math.PI);
        }
        */
        public static double GetAngle(Vector2D u, Vector2D v)
        {
            double dot = u * v;
            double det = u.X * v.Y - u.Y * v.X;
            double angle = MathF.Atan2((float)det, (float)dot) * (180 / Math.PI);
            if (angle >= 0)
                return angle;
            else
                return 360 + angle;
            //return MathF.Acos((float)((u * v) / (u.Norm() * v.Norm()))) * (180 / Math.PI);
        }
        #endregion

        #region Operators
        public static bool operator ==(Vector2D u, Vector2D v)
        {
            if (u.X == v.X && u.Y == v.Y)
                return true;
            else
                return false;
        }

        public static bool operator !=(Vector2D u, Vector2D v)
        {
            return u != v;
        }

        public static Vector2D operator +(Vector2D u, Vector2D v)
        {
            return new Vector2D(u.X + v.X, u.Y + v.Y);
        }

        public static Vector2D operator -(Vector2D u, Vector2D v)
        {
            return new Vector2D(u.X - v.X, u.Y - v.Y);
        }

        public static Vector2D operator *(Vector2D u, double a)
        {
            return new Vector2D(a * u.X, a * u.Y);
        }

        public static Vector2D operator /(Vector2D u, double a)
        {
            return new Vector2D(u.X / a, u.Y / a);
        }

        public static Vector2D operator -(Vector2D u)
        {
            return new Vector2D(-u.X, -u.Y);
        }

        public static double operator *(Vector2D u, Vector2D v)
        {
            return (u.X * v.X) + (u.Y * v.Y);
        }
        #endregion

    }
}
