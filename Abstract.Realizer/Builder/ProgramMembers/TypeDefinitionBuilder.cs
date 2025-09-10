namespace Abstract.Realizer.Builder.ProgramMembers;

public class TypeDefinitionBuilder(string name): TypeBuilder(name)
{
    public override string ToString() => $"(typedef {Name})";
}
