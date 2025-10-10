using System.Text;
using Abstract.Realizer.Builder.Language;
using Abstract.Realizer.Builder.Language.Omega;
using Abstract.Realizer.Builder.References;
using Abstract.Realizer.Core.Intermediate.Language;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class FunctionBuilder: BaseFunctionBuilder
{
    public string? ExportSymbol = null;

    public bool IsInstance => Parameters.Count > 0 
                           && Parameters[0].name == "self"
                           && (Parameters[0].type is ReferenceTypeReference { Subtype: NodeTypeReference @ntr } 
                           && ntr.TypeReference == Parent);
    
    public BytecodeBuilder? BytecodeBuilder { get; internal set;  }
    internal IrRoot? _intermediateRoot = null;

    internal FunctionBuilder(INamespaceOrStructureBuilder parent, string name, bool annonymous)
        : base(parent, name, annonymous) {}
    
    
    public OmegaBytecodeBuilder GetOrCreateOmegaBuilder()
    {
        if (BytecodeBuilder is not null and not OmegaBytecodeBuilder)
            throw new Exception($"{BytecodeBuilder.GetType().Name} already instantiated!");

        BytecodeBuilder ??= new OmegaBytecodeBuilder();
        return (BytecodeBuilder as OmegaBytecodeBuilder)!;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"(func \"{Symbol}\"");
        foreach (var (name, type) in Parameters) sb.Append($" (param \"{name}\" {type})");
        if (ReturnType != null) sb.Append($" (ret {ReturnType})");
        
        if (_intermediateRoot != null) sb.Append($"\n{_intermediateRoot.ToString().TabAllLines()}");
        else if (BytecodeBuilder != null) sb.Append("\n" + BytecodeBuilder.ToString().TabAllLines());
        else sb.Append(" (no body)");

        sb.Append(')');
        return sb.ToString();
    }
    
}
