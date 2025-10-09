using System.Text;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class VirtualFunctionBuilder: FunctionBuilder
{
    public uint Index = 0;

    internal VirtualFunctionBuilder(INamespaceOrStructureBuilder parent, string name, uint index, bool annonymous)
        : base(parent, name, annonymous)
    {
        Index = index;
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"(func (virtual {Index}) \"{Symbol}\"");
        foreach (var (name, type) in Parameters) sb.Append($" (param \"{name}\" {type})");
        
        if (_intermediateRoot != null) sb.Append($"\n{_intermediateRoot.ToString().TabAllLines()}");
        else if (BytecodeBuilder != null) sb.Append("\n" + BytecodeBuilder.ToString().TabAllLines());
        else sb.AppendLine("(no body)");
        
        sb.Length -= Environment.NewLine.Length;
        sb.Append(')');
        
        return sb.ToString();
    }
}
