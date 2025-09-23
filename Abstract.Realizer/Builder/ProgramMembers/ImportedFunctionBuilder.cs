using System.Text;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class ImportedFunctionBuilder: BaseFunctionBuilder
{
    public string? Symbol { get; set; }
   

    internal ImportedFunctionBuilder(INamespaceOrStructureBuilder parent, string name) : base(parent, name) { }
    internal ImportedFunctionBuilder(INamespaceOrStructureBuilder parent, ImportedFunctionBuilder tocopy) : this(parent, tocopy.Name)
    {
        Symbol = tocopy.Symbol;
    }
    
    
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"(func \"{Name}\"");
        foreach (var (name, type) in parameters) sb.Append($" (param \"{name}\" {type})");

        if (Symbol != null) sb.Append($" (import \"{Symbol}\")");
        else sb.Append($" (import nullptr)");
        sb.Append(')');
        
        return sb.ToString();
    }
}
