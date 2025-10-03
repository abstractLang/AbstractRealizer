using Abstract.Realizer.Core.Intermediate.Types;

namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrSlice(RealizerType ty, IrValue[] values) : IrValue
{
    public readonly RealizerType ItemType = ty;
    public readonly IrValue[] Values = values;

    public override string ToString()
        => $"(slice {ItemType} [{string.Concat(values.Select(( e => e.ToString())))}])";
}
