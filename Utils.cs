using Microsoft.Xna.Framework;

namespace Hacknet.Effects
{
    public static class Utils
    {
        public static Color AddativeRed = new Color(255, 0, 0, 128);
        
        public static float QuadraticOutCurve(float t)
        {
            return t * (2.0f - t);
        }
    }
}