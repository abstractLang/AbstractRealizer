using System.Text;

namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrIf(IrValue c) : IrNode
{
    public readonly IrValue Condition = c;
    public readonly List<IrNode> Then = [];
    public readonly List<IrNode> Else = [];

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"(if {Condition}");
        foreach (var i in Then) sb.Append($"{i}".TabAllLines());
        if (Then.Count > 0) sb.Length -= Environment.NewLine.Length;
        if (Else.Count > 0)
        {
            sb.AppendLine($"(else");
            foreach (var i in Else) sb.Append($"{i}".TabAllLines());
            sb.Length -= Environment.NewLine.Length;
            sb.Append(')');
        }
        sb.Append(')');
        
        return sb.ToString();
    }
}
