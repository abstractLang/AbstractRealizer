using System.Reflection;
using Abstract.Realizer.Core.Intermediate.Types;

namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrSliceBytes(byte[] data) : IrValue
{
    public RealizerType ItemType => new IntegerType(false, 8);
    public readonly byte[] Values = data;

    public override string ToString() => $"(slice {string.Concat(Values.Select(e => e.ToString()))})";
}
