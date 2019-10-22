using System;
namespace Kit
{
    public enum SelectBoundMode { NONE, CLAMP, LOOP, LOOP_OVER_NONE }

    public partial class Select<T>
    {
        static int Bounded(int index, int count, SelectBoundMode mode)
        {
            switch (mode)
            {
                case SelectBoundMode.LOOP_OVER_NONE:

                    index %= count + 1;

                    if (index < 0)
                        index += count + 1;

                    return index == count ? -1 : index;

                case SelectBoundMode.LOOP:

                    index %= count;

                    return index < 0 ? index + count : index;

                case SelectBoundMode.CLAMP:

                    return index < 0 ? 0 : index >= count ? count - 1 : index;

                default:

                    return index < 0 ? -1 : index >= count ? count - 1 : index;

            }
        }

    }
}
