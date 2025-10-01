using Abstract.Realizer.Core.Intermediate.Language;
using Abstract.Realizer.Core.Intermediate.Types;

namespace Abstract.Realizer.Builder.ProgramMembers;

internal class IrTrunc(IntegerType to, IrValue val) : IrValue
{
    public readonly IntegerType ToType = to;
    public readonly IrValue Value = val;

    public override string ToString() => $"(trunc {ToType} {Value})";
}
