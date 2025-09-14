using System.Text;
using Abstract.Realizer.Builder.Language;
using Abstract.Realizer.Builder.Language.Omega;
using Abstract.Realizer.Builder.References;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class FunctionBuilder(INamespaceOrStructureBuilder parent, string name): ProgramMemberBuilder(parent, name)
{
    private List<(string, TypeReference)> parameters = [];
    private List<TypeReference> locals = [];

    public BytecodeBuilder? BytecodeBuilder { get; private set;  } = null!;

    public int AddParameter(string name, TypeReference typeReference)
    {
        parameters.Add((name, typeReference));
        return parameters.Count - 1;
    }
    public int AddLocal(TypeReference typeReference)
    {
        locals.Add(typeReference);
        return locals.Count - 1;
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

        sb.Append($"(function \"{Name}\"");
        foreach (var (name, type) in parameters) sb.Append($" (param \"{name}\" {type})");
        sb.AppendLine();
        for (var i = 0; i < locals.Count; i++) sb.AppendLine($"\t(local ${i:d2} {locals[i]})");

        if (BytecodeBuilder != null) sb.Append(BytecodeBuilder.ToString().TabAllLines().TabAllLines());
        
        sb.Length -= Environment.NewLine.Length;
        sb.Append(')');
        
        return sb.ToString();
    }

    public string ToReadableReference()
    {
        var sb = new StringBuilder();

        sb.Append('"');
        sb.AppendJoin('.', GlobalIdentifier);
        sb.Append('"');
        sb.Append('(');
        sb.AppendJoin(", ", parameters.Select(e => e.Item2.ToString()));
        sb.Append(')');
        
        return sb.ToString();
    }
}
