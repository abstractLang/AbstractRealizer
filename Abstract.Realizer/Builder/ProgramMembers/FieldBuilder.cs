namespace Abstract.Realizer.Builder.ProgramMembers;

public abstract class FieldBuilder: ProgramMemberBuilder
{
    internal FieldBuilder(INamespaceOrStructureBuilder parent, string name) : base(parent, name) {}
}