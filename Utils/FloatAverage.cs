using System;
namespace Kit
{
    public class FloatAverage
    {
        int index = 0;
        int length = 10;
        float[] values;

        float sum;

        public FloatAverage(int length)
        {
            this.length = length;
            values = new float[length];
            sum = 0;
        }

        public FloatAverage() : this(10) { }

        public float Average => sum / length;

        public void SetCurrentValue(float value)
        {
            sum += -values[index] + value;
            values[index] = value;

            index = index + 1 < length ? index + 1 : 0;
        }

        public float CurrentValue
        {
            get => values[index];
            set => SetCurrentValue(value);
        }
    }
}
