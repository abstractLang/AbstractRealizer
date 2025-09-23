using System.Text;

namespace Abstract.Realizer.Builder.Language.Beta;

public class BetaBytecodeBuilder : BytecodeBuilder
{
    private List<IBetaInstruction> _instructions = [];
    public List<IBetaInstruction> InstructionsList => _instructions;
    public InstructionWriter Writer => new(this);


    public override string ToString()
    {
        var sb = new StringBuilder();

        foreach (var i in _instructions) sb.AppendLine(i.ToString());

        return sb.ToString();
    }
    

    public struct InstructionWriter
    {
        private BetaBytecodeBuilder _parentBuilder;
        internal InstructionWriter(BetaBytecodeBuilder builder) => _parentBuilder = builder;

        private InstructionWriter AddAndReturn(IBetaInstruction value)
        {
            _parentBuilder._instructions.Add(value);
            return this;
        }
    }
}
