using Abstract.Realizer.Builder.ProgramMembers;

namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrNewObj(StructureBuilder type) : IrValue
{
    public readonly StructureBuilder Type = type;
    public override string ToString() => $"(new {type.ToReadableReference()})";
}
