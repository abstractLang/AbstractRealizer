namespace Abstract.Realizer.Builder.ProgramMembers;

public class StructureBuilder(string name): TypeBuilder(name)
{
    public override string ToString() => $"(structure {Name})";
}
