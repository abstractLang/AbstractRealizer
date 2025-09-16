using System.Text;
using Abstract.Realizer.Builder.Language;
using Abstract.Realizer.Builder.Language.Omega;
using Abstract.Realizer.Builder.References;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class ImportedFunctionBuilder(INamespaceOrStructureBuilder parent, string name): BaseFunctionBuilder(parent, name)
{
    public string? Symbol { get; set; }
   
    
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"(func \"{Name}\"");
        foreach (var (name, type) in parameters) sb.Append($" (param \"{name}\" {type})");

        if (Symbol != null) sb.Append($" (import \"{Symbol}\")");
        else sb.Append($" (import nullptr)");
        
        sb.Length -= Environment.NewLine.Length;
        sb.Append(')');
        
        return sb.ToString();
    }
    
}
