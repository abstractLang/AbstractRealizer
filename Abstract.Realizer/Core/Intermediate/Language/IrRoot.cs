using System.Text;

namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrRoot: IrNode
{
    public List<IrNode> content = [];

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var item in content) sb.AppendLine(item.ToString());
        if (sb.Length > Environment.NewLine.Length) sb.Length -= Environment.NewLine.Length;
        return sb.ToString();
    }
}