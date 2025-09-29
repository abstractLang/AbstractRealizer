using Abstract.Realizer.Builder.References;

namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrMacroDefineLocal(TypeReference tref): IrMacro
{
    public readonly TypeReference Type = tref;
    public override string ToString() => $"$DEFINE_LOCAL {Type}";
}
