using Abstract.Realizer.Core.Intermediate.Language;

namespace Abstract.Realizer.Builder.ProgramMembers;

internal class IrTrunc(byte len, IrValue val) : IrValue
{
    public readonly byte NewLength = len;
    public readonly IrValue Value = val;

    public override string ToString() => $"(trunc {NewLength} {Value})";
}
