using System;
namespace Kit
{
    public class Random
    {
        public static Random R = new Random();

        System.Random random;

        const float INT_MAX_VALUE = int.MaxValue;

        public Random(int seed = 12345)
        {
            random = new System.Random(seed);
        }

        public float Float() => random.Next() / INT_MAX_VALUE;
        public float Float(float max) => max * Float();
        public float Float(float min, float max) => min + (max - min) * Float();
    }
}
