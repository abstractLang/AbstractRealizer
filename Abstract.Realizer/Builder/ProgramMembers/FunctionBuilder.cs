using System.Text;
using Abstract.Realizer.Builder.Language;
using Abstract.Realizer.Builder.Language.Omega;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class FunctionBuilder: BaseFunctionBuilder
{
    public BytecodeBuilder? BytecodeBuilder { get; private set;  }
  
    
    internal FunctionBuilder(INamespaceOrStructureBuilder parent, string name) : base(parent, name) { }
    internal FunctionBuilder(INamespaceOrStructureBuilder parent, FunctionBuilder tocopy) : this(parent, tocopy.Name)
    {
        BytecodeBuilder = (BytecodeBuilder?)tocopy.BytecodeBuilder?.Clone();
    }
    
    
    
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

        sb.Append($"(func \"{Name}\"");
        foreach (var (name, type) in parameters) sb.Append($" (param \"{name}\" {type})");
        sb.AppendLine();
        for (var i = 0; i < locals.Count; i++) sb.AppendLine($"\t(local ${i:d2} {locals[i]})");

        if (BytecodeBuilder != null) sb.Append(BytecodeBuilder.ToString().TabAllLines().TabAllLines());
        
        sb.Length -= Environment.NewLine.Length;
        sb.Append(')');
        
        return sb.ToString();
    }
    
}
