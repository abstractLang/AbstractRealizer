using System.Text;

namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrRoot
{
    public List<IrNode> content = [];

    public override string ToString()
    {
        var sb = new StringBuilder();

        foreach (var item in content)
            sb.AppendLine(item.ToString());
        
        return sb.ToString();
    }
}