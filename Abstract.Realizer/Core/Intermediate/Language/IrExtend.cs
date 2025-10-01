using Abstract.Realizer.Builder.References;
using Abstract.Realizer.Core.Intermediate.Language;
using Abstract.Realizer.Core.Intermediate.Types;

namespace Abstract.Realizer.Builder.ProgramMembers;

internal class IrExtend(IntegerType to, IrValue val) : IrValue
{
    public readonly IntegerType ToType = to;
    public readonly IrValue Value = val;

    public override string ToString() => $"(extend {ToType} {Value})";
}
