using System.Text;
using Abstract.Realizer.Builder.References;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class StructureBuilder(NamespaceBuilder parent, string name): TypeBuilder(parent, name),
    INamespaceOrStructureBuilder
{
    protected List<FieldBuilder> fields = [];
    protected List<FunctionBuilder> functions = [];
    
    public FieldBuilder AddField(string fn)
    {
        var newField = new FieldBuilder(this, fn);
        fields.Add(newField);
        return newField;
    }
    
    public FunctionBuilder AddFunction(string fn)
    {
        var newFunction = new FunctionBuilder(this, fn);
        functions.Add(newFunction);
        return newFunction;
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"(struct \"{name}\"");
        foreach (var i in fields) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in functions) sb.AppendLine(i.ToString().TabAllLines());
        sb.Length -= Environment.NewLine.Length;
        sb.Append(')');
        
        return sb.ToString();
    }
}
