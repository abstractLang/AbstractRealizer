using Abstract.Realizer.Builder.ProgramMembers;
using Abstract.Realizer.Core.Configuration.LangOutput;
using Abstract.Realizer.Core.Intermediate.Language;

namespace Abstract.Realizer.Optimization;

internal static class FunctionNormalizer
{
    internal static void NormalizeFunction(FunctionBuilder function, IrRoot intermediate, BetaOutputConfiguration config)
    {
        
        
        
    }

    internal static bool NeedsMemStack(FunctionBuilder function, IrRoot intermediate, BetaOutputConfiguration config)
    {
        if (config.UseMemoryStack) return false;
        if (config.LocalStores == BetaDataKinds.All) return false;
        
        //return ((config.LocalStores & BetaDataKinds.Primitives) == 0 && function.)
        return false;
    }
}
