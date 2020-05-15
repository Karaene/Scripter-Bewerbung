using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    public static class MathHelper
    {
        public static float Clamp(float x, float aMin, float bMax)
        {
            return (float)Math.Max(aMin, Math.Min(bMax, x));
        }

        public static float Frac(float v)
        {
            return (float)(v - Math.Floor(v));
        }

        public static float Saturate(float v)
        {
            return (float)Math.Max(0, Math.Min(1, v));
        }

        public static float SaturateSigned(float v)
        {
            return (float)Math.Max(-1, Math.Min(1, v));
        }

        public static float Lerp(float from, float to, float weight)
        {
            return from + weight * (to - from);
        }

        public static float Smerp(float from, float to, float t)
        {
            float weight = t * t * (3.0f - (2.0f * t));
            return from + weight * (to - from);
        }

        public static float Towards(float from, float to, float max)
        {
            if (max <= 0)
            {
                return from;
            }
            return from + Math.Sign(to - from) * Math.Min(Math.Abs(to - from), max);
        }

        public static float Step(float edge, float v)
        {
            return (v >= edge) ? 1 : 0;
        }

        public static float SmoothStep(float edge0, float edge1, float v)
        {
            float t = Saturate((v - edge0) / (edge1 - edge0));
            return t * t * (3.0f - (2.0f * t));
        }

        public static float Linstep(float edge0, float edge1, float v)
        {
            return Saturate((v - edge0) / (edge1 - edge0));
        }
    }
}
