using System.Text;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class ModuleBuilder: NamespaceBuilder
{

    internal ModuleBuilder(string name) : base(null!, name) { }
    internal ModuleBuilder(ModuleBuilder tocopy) : this(tocopy.Name) { }
    
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"(module \"{Name}\"");
        foreach (var i in namespaces) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in functions) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in structures) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in typedefs) sb.AppendLine(i.ToString().TabAllLines());
        sb.Length -= Environment.NewLine.Length;
        sb.Append(')');
        
        return sb.ToString();
    }
}
