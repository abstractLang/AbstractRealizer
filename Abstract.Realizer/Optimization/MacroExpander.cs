using Abstract.Realizer.Builder.ProgramMembers;
using Abstract.Realizer.Core.Configuration.LangOutput;
using Abstract.Realizer.Core.Intermediate.Language;

namespace Abstract.Realizer.Optimization;

public class MacroExpander
{
    internal static void ExpandFunctionMacros(FunctionBuilder function, ILanguageOutputConfiguration config)
    {
        var intermediateRoot = function._intermediateRoot!;

        List<IrMacro> macros = [];
        foreach (var i in intermediateRoot.content) if (i is IrMacro @m) macros.Add(m);

        var localsIdx = 0;
        
        foreach (var macro in macros)
        {
            if (macro is IrMacroDefineLocal @macroDefineLocal)
            {
                intermediateRoot.content.Remove(macroDefineLocal);
                intermediateRoot.content.Insert(localsIdx++, @macroDefineLocal);
            }
        }
    }
}