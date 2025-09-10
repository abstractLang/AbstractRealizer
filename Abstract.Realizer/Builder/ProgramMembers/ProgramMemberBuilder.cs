namespace Abstract.Realizer.Builder.ProgramMembers;

public abstract class ProgramMemberBuilder
{
    public readonly NamespaceBuilder? Parent;
    public readonly string Name;

    public string[] GlobalIdentifier => Parent == null
        ? [Name]
        : [..Parent.GlobalIdentifier.Where(e=>!string.IsNullOrEmpty(e)), Name];
    
    internal ProgramMemberBuilder(NamespaceBuilder parent, string name)
    {
        Parent = parent;
        Name = name;
    }
}
