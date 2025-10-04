using System.Text;
using Abstract.Realizer.Builder.References;

namespace Abstract.Realizer.Builder.ProgramMembers;

public abstract class BaseFunctionBuilder: ProgramMemberBuilder
{
    public List<(string name, TypeReference type)> Parameters = [];
    public TypeReference? ReturnType = null;

    internal BaseFunctionBuilder(INamespaceOrStructureBuilder parent, string name, bool annonymous)
        : base(parent, name, annonymous) { }
    
    
    public int AddParameter(string name, TypeReference typeReference)
    {
        Parameters.Add((name, typeReference));
        return Parameters.Count - 1;
    }
    
    public override string ToReadableReference()
    {
        var sb = new StringBuilder();

        sb.Append('"');
        sb.AppendJoin('.', GlobalIdentifier);
        sb.Append('"');
        sb.Append('(');
        sb.AppendJoin(", ", Parameters.Select(e => e.Item2.ToString()));
        sb.Append(')');
        
        return sb.ToString();
    }
}