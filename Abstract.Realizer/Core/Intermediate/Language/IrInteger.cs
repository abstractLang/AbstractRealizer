using System.Numerics;

namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrInteger(int? bits, BigInteger value): IrValue
{
    public readonly int? Bits = bits;
    public readonly BigInteger Value = value;

    public override string ToString() => "(const " + (Bits.HasValue ? Bits.Value : "ptr")+ $") {Value}";
}