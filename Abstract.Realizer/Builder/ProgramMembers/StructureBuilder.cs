using System.Text;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class StructureBuilder: TypeBuilder, INamespaceOrStructureBuilder
{
    protected List<FieldBuilder> fields = [];
    protected List<BaseFunctionBuilder> functions = [];
    
    
    internal StructureBuilder(INamespaceOrStructureBuilder parent, string name): base(parent, name) {}
    internal StructureBuilder(INamespaceOrStructureBuilder parent, StructureBuilder tocopy) : this(parent, tocopy.Name)
    {
        foreach (var i in tocopy.fields) fields.Add(new(this, i));
        foreach (var i in tocopy.functions)
        {
            switch (i)
            {
                case FunctionBuilder f: functions.Add(new FunctionBuilder(this, f)); break;
                case ImportedFunctionBuilder f: functions.Add(new ImportedFunctionBuilder(this, f)); break;
            }
        } 
    }
    
    
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

        sb.AppendLine($"(struct \"{Name}\"");
        foreach (var i in fields) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in functions) sb.AppendLine(i.ToString().TabAllLines());
        sb.Length -= Environment.NewLine.Length;
        sb.Append(')');
        
        return sb.ToString();
    }
    
    public override string ToReadableReference() => '"' + string.Join('.', GlobalIdentifier) + '"';
}
