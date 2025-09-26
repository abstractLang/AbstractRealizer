using System.Numerics;

namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrInteger(byte? size, BigInteger value): IrValue
{
    public readonly byte? Size = size;
    public readonly BigInteger Value = value;

    public override string ToString() => "(const " + (Size.HasValue ? Size.Value : "ptr")+ $" {Value})";
}