using Abstract.Realizer.Builder.References;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class StaticFieldBuilder: FieldBuilder
{
    public TypeReference? Type = null;
    
    
    internal StaticFieldBuilder(INamespaceOrStructureBuilder parent, string name, bool annonymous)
        : base(parent, name, annonymous) {}
    
    
    public override string ToString() => $"(field static \"{Symbol}\" {Type?.ToString() ?? "<nil>"})";
    public override string ToReadableReference() => $"\"{string.Join('.', GlobalIdentifier)}\"";
}
