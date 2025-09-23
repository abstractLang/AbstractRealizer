namespace Abstract.Realizer.Builder.ProgramMembers;

public class TypeDefinitionBuilder: TypeBuilder
{
    
    internal TypeDefinitionBuilder(INamespaceOrStructureBuilder parent, string name) : base(parent, name) {}

    internal TypeDefinitionBuilder(INamespaceOrStructureBuilder parent, TypeBuilder tocopy) : this(parent, tocopy.Name)
    {
        
    }
    
    
    public override string ToString() => $"(typedef {Name})";
    public override string ToReadableReference() => '"' + string.Join('.', GlobalIdentifier) + '"';
}
