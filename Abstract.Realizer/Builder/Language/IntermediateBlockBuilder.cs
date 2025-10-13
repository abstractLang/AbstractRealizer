using Abstract.Realizer.Builder.ProgramMembers;
using Abstract.Realizer.Core.Intermediate.Language;

namespace Abstract.Realizer.Builder.Language;

internal class IntermediateBlockBuilder(FunctionBuilder p, string n, uint idx) : BlockBuilder(p, n, idx)
{
    public readonly IrRoot Root = new IrRoot();
    public override string DumpInstructionsToString() => Root.ToString();
}
