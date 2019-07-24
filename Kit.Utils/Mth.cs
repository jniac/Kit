using System;

namespace Kit.Utils
{
    public static class Mth
    {
        public static float Clamp(float x) => x < 0 ? 0 : x > 1 ? 1 : x;

        public static float Clamp(float x, float min, float max) => x < min ? min : x > max ? max : x;

        public static int Clamp(int x) => x < 0 ? 0 : x > 1 ? 1 : x;

        public static int Clamp(int x, int min, int max) => x < min ? min : x > max ? max : x;
    }
}
