using Abstract.Realizer.Builder.ProgramMembers;

namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrNewObj(TypeBuilder type) : IrValue
{
    public readonly TypeBuilder Type = type;

    public override string ToString() => $"(new {type.ToReadableReference()})";
}
