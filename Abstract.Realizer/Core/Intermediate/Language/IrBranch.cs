using System.Text;
using Abstract.Realizer.Builder.Language;

namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrBranch(BlockBuilder to) : IrNode
{
    public readonly BlockBuilder To = to;

    public override string ToString() => $"(branch $\"{To.Name}\")";

}
