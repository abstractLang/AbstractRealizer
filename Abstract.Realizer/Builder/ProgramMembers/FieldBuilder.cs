using Abstract.Realizer.Builder.References;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class FieldBuilder(INamespaceOrStructureBuilder parent, string name): ProgramMemberBuilder(parent, name)
{
    public TypeReference? Type = null;
    public override string ToString() => $"(field \"{Name}\" {Type?.ToString() ?? "<nil>"})";
}
