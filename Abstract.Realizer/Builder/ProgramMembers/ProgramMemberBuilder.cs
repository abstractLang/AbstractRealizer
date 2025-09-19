namespace Abstract.Realizer.Builder.ProgramMembers;

public abstract class ProgramMemberBuilder
{
    public readonly INamespaceOrStructureBuilder? Parent;
    public readonly string Name;

    public string[] GlobalIdentifier => Parent == null
        ? [Name]
        : [..Parent.GlobalIdentifier.Where(e=>!string.IsNullOrEmpty(e)), Name];
    
    internal ProgramMemberBuilder(INamespaceOrStructureBuilder parent, string name)
    {
        Parent = parent;
        Name = name;
    }

    public abstract string ToReadableReference();
}
