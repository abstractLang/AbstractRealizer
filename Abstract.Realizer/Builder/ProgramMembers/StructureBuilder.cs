using System.Text;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class StructureBuilder: TypeBuilder, INamespaceOrStructureBuilder
{
    public StructureBuilder? Extends = null;
    public List<InstanceFieldBuilder> Fields = [];
    public List<BaseFunctionBuilder> Functions = [];
    
    public uint? Length = null;
    public uint? Alignment = null;
    public uint? VtableLength = null;
    
    
    internal StructureBuilder(INamespaceOrStructureBuilder parent, string name, bool annonymouns)
        : base(parent, name, annonymouns) {}
    
    
    public InstanceFieldBuilder AddField(string symbol)
    {
        var newField = new InstanceFieldBuilder(this, symbol, false);
        Fields.Add(newField);
        return newField;
    }
    public FunctionBuilder AddFunction(string symbol)
    {
        var newFunction = new FunctionBuilder(this, symbol, false);
        Functions.Add(newFunction);
        return newFunction;
    }
    public AbstractFunctionBuilder AddAbstractFunction(string symbol)
    {
        var newFunction = new AbstractFunctionBuilder(this, symbol, false);
        Functions.Add(newFunction);
        return newFunction;
    }
    
    public InstanceFieldBuilder AddAnnonymousField(string symbol)
    {
        var newField = new InstanceFieldBuilder(this, symbol, true);
        Fields.Add(newField);
        return newField;
    }
    public FunctionBuilder AddAnnonymousFunction(string symbol)
    {
        var newFunction = new FunctionBuilder(this, symbol, true);
        Functions.Add(newFunction);
        return newFunction;
    }
    
    
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"(struct \"{Symbol}\"");
        if (Extends != null) sb.Append($" (extends {Extends.ToReadableReference()}");
        sb.AppendLine();
        
        if (Length != null || Alignment != null) sb.Append('\t');
        if (Length != null) sb.Append($"(length {Length}) ");
        if (Alignment != null) sb.Append($"(alignment {Alignment})");
        if (Length != null || Alignment != null) sb.AppendLine();
        if (VtableLength != null) sb.AppendLine($"\t(vtablelength {VtableLength.Value})");
        
        foreach (var i in Fields) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in Functions) sb.AppendLine(i.ToString().TabAllLines());
        sb.Length -= Environment.NewLine.Length;
        sb.Append(')');
        
        return sb.ToString();
    }
    public override string ToReadableReference() => '"' + string.Join('.', GlobalIdentifier) + '"';
}
