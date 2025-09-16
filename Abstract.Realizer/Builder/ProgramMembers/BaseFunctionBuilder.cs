using System.Text;
using Abstract.Realizer.Builder.References;

namespace Abstract.Realizer.Builder.ProgramMembers;

public abstract class BaseFunctionBuilder(INamespaceOrStructureBuilder parent, string name): ProgramMemberBuilder(parent, name)
{
    protected List<(string, TypeReference)> parameters = [];
    protected List<TypeReference> locals = [];

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