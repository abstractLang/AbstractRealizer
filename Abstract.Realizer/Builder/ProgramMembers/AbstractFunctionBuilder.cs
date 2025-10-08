using System.Text;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class AbstractFunctionBuilder: BaseFunctionBuilder
{
    internal uint? Index = 0;
    
    internal AbstractFunctionBuilder(INamespaceOrStructureBuilder parent, string name, bool annonymous)
        : base(parent, name, annonymous) {}
    
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"(func virtual" + (Index.HasValue ? $" index={Index.Value}" : "") + $" \"{Symbol}\"");
        foreach (var (name, type) in Parameters) sb.Append($" (param \"{name}\" {type})");
        sb.Append("(no body))");
        
        return sb.ToString();
    }
}