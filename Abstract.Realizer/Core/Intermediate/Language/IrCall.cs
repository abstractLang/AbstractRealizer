using System.Text;
using Abstract.Realizer.Builder.ProgramMembers;

namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrCall(BaseFunctionBuilder f, IrValue[] arguments) : IrValue
{
    public readonly BaseFunctionBuilder Function = f;
    public readonly IrValue[] Arguments = arguments;

    public override string ToString()
    {
        var sb = new StringBuilder();
        
        sb.Append($"(call {Function.ToReadableReference()}");
        foreach (var i in Arguments) sb.Append($"\n{i}");
        sb.Append(')');

        return sb.ToString();
    }
}
