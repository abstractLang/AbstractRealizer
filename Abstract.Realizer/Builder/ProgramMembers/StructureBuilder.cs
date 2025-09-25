using System.Text;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class StructureBuilder: TypeBuilder, INamespaceOrStructureBuilder
{
    public List<InstanceFieldBuilder> Fields = [];
    public List<BaseFunctionBuilder> Functions = [];


    public uint? Length = null;
    public uint? Alignment = null;
    
    
    internal StructureBuilder(INamespaceOrStructureBuilder parent, string name): base(parent, name) {}
    
    
    public InstanceFieldBuilder AddField(string fn)
    {
        var newField = new InstanceFieldBuilder(this, fn);
        Fields.Add(newField);
        return newField;
    }
    public FunctionBuilder AddFunction(string fn)
    {
        var newFunction = new FunctionBuilder(this, fn);
        Functions.Add(newFunction);
        return newFunction;
    }
    
    
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"(struct \"{Name}\"");
        
        if (Length != null || Alignment != null) sb.Append('\t');
        if (Length != null) sb.Append($"(length {Length}) ");
        if (Alignment != null) sb.Append($"(alignment {Alignment})");
        if (Length != null || Alignment != null) sb.AppendLine();
        
        foreach (var i in Fields) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in Functions) sb.AppendLine(i.ToString().TabAllLines());
        sb.Length -= Environment.NewLine.Length;
        sb.Append(')');
        
        return sb.ToString();
    }
    public override string ToReadableReference() => '"' + string.Join('.', GlobalIdentifier) + '"';
}
