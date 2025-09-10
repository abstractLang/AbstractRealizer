namespace Abstract.Realizer.Builder.ProgramMembers;

public class StructureBuilder(NamespaceBuilder parent, string name): TypeBuilder(parent, name)
{
    public override string ToString() => $"(structure {Name})";
}
