using System;

namespace Kit
{
    // quite well referenced:
    // https://gist.github.com/blixt/f17b47c62508be59987b
    // uniform?
    // https://www.khanacademy.org/computer-programming/prng-test/5500564014432256
    public class PRNG
    {
        uint seed;
        public uint Seed 
        {
            get => seed;
            set => SetSeed(value);              
        }

        public PRNG()
        {
            seed = (uint)DateTime.Now.Millisecond * 16807 % 2147483647;
        }

        public PRNG(uint seed)
        {
            SetSeed(seed);
        }

        public void SetSeed(uint seed)
        {
            if (seed == 0)
                throw new Exception("oups, seed must be greater than zero");

            this.seed = seed % 2147483647;
        }

        public uint Next() =>
            seed = seed * 16807 % 2147483647;



        public float Float() => 
            Next() / 2147483647f;

        public float Float(float max) =>
            max * Float();
            
        public float Float(float min, float max) =>
            min + (max - min) * Float();



        public int Int(int max) =>
            (int)Float(max);



        public bool Chance(float esperance) =>
            Float() < esperance;
    }

    public static class PRNGS
    {
        static PRNG prng = new PRNG();

        public static float Float() => prng.Float();
        public static float Float(float max) => prng.Float(max);
        public static float Float(float min, float max) => prng.Float(min, max);
        public static bool Chance(float esperance) => prng.Chance(esperance);

    }
}
