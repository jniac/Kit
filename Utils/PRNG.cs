using System;

namespace Kit.Utils
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
    }
}
