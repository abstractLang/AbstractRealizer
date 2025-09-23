using Abstract.Realizer.Builder.References;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class FieldBuilder: ProgramMemberBuilder
{
    public TypeReference? Type = null;
    
    
    internal FieldBuilder(INamespaceOrStructureBuilder parent, string name) : base(parent, name) {}
    internal FieldBuilder(INamespaceOrStructureBuilder parent, FieldBuilder tocopy) : this(parent, tocopy.Name)
    {
        Type = tocopy.Type;
    }
    
    
    public override string ToString() => $"(field \"{Name}\" {Type?.ToString() ?? "<nil>"})";

    public override string ToReadableReference() => $"\"{string.Join('.', GlobalIdentifier)}\"";
}
