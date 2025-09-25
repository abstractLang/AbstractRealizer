using Abstract.Realizer.Core.Intermediate.Language;

namespace Abstract.Realizer.Builder.ProgramMembers;

internal class IrExtend(byte len, IrValue val) : IrValue
{
    public readonly byte NewLength = len;
    public readonly IrValue Value = val;

    public override string ToString() => $"(extend {NewLength} {Value})";
}
