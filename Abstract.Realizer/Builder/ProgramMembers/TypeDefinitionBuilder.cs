namespace Abstract.Realizer.Builder.ProgramMembers;

public class TypeDefinitionBuilder(INamespaceOrStructureBuilder parent, string name): TypeBuilder(parent, name)
{
    public override string ToString() => $"(typedef {Name})";
}
