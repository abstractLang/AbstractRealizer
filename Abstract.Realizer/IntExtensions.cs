using System.Text;

namespace Abstract.Realizer;

internal static class IntExtensions
{
    public static byte AlignForward(this byte value, long alignment) => (byte)((value + alignment - 1) & ~(alignment - 1));
    public static int AlignForward(this int value, long alignment) => (int)((value + alignment - 1) & ~(alignment - 1));
    public static uint AlignForward(this uint value, long alignment) => (uint)((value + alignment - 1) & ~(alignment - 1));
}
