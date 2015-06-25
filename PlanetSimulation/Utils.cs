using System;
using SFML.Window;

namespace PlanetSimulation
{
    static class Utils
    {
        public static double Distance(Vector2f v, Vector2f u)
        {
            return Length(v - u);
        }

        public static double Length(Vector2f v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }
    }
}
