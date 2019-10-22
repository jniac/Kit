using System;

namespace Kit
{
    public static class Mth
    {
        public static float Clamp(float x) => x < 0 ? 0 : x > 1 ? 1 : x;

        public static void Clamp(ref float x) => x = x < 0 ? 0 : x > 1 ? 1 : x;

        public static float Clamp(float x, float min, float max) => x < min ? min : x > max ? max : x;

        public static int Clamp(int x) => x < 0 ? 0 : x > 1 ? 1 : x;

        public static int Clamp(int x, int min, int max) => x < min ? min : x > max ? max : x;

        public static float Mix(float a, float b, float t, bool clamp = true) => a + (b - a) * (clamp ? Clamp(t) : t);

        public static float Ratio(float x, float min, float max, bool clamp = true)
        {
            x = (x - min) / (max - min);

            if (clamp)
                Clamp(ref x);

            return x;
        }

        // https://www.desmos.com/calculator/5qrnsfomsl
        public static float Limited(float x, float max) => (max * x) / (max + x);
    }
}
