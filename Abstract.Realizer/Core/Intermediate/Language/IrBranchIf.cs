using System.Text;
using Abstract.Realizer.Builder.Language;

namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrBranchIf(IrValue c, BlockBuilder iftrue, BlockBuilder iffalse) : IrNode
{
    public readonly IrValue Condition = c;
    public readonly BlockBuilder IfTrue = iftrue;
    public readonly BlockBuilder IfFalse = iffalse;

    public override string ToString() => $"(branch.if {Condition} $\"{IfTrue.Name}\" $\"{IfTrue.Name}\")";

}
