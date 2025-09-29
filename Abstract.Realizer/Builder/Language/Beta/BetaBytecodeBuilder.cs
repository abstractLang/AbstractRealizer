using System.Numerics;
using System.Text;
using Abstract.Realizer.Builder.ProgramMembers;
using Abstract.Realizer.Builder.References;

namespace Abstract.Realizer.Builder.Language.Beta;

public class BetaBytecodeBuilder : BytecodeBuilder
{
    private List<IBetaInstruction> _instructions = [];
    public List<IBetaInstruction> InstructionsList => _instructions;
    public InstructionWriter Writer => new(this);

    
    public override string ToString()
    {
        var sb = new StringBuilder();

        foreach (var i in _instructions)
        {
            sb.Append(i);
            if (i is not IBetaFlag) sb.AppendLine();
        }

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
        
        
        public InstructionWriter Nop() => AddAndReturn(new InstNop());
        public InstructionWriter Invalid() => AddAndReturn(new InstInvalid());
        public InstructionWriter Pop() => AddAndReturn(new InstPop());
        public InstructionWriter Dup() => AddAndReturn(new InstDup());
        public InstructionWriter Swap() => AddAndReturn(new InstSwap());
        public InstructionWriter Call(BaseFunctionBuilder func) => AddAndReturn(new InstCall(func));
        public InstructionWriter CallVirt() => AddAndReturn(new InstCallVirt());
        public InstructionWriter Ret() => AddAndReturn(new InstRet());
        public InstructionWriter Break() => AddAndReturn(new InstBreak());

        
        public InstructionWriter LdConstI(byte size, BigInteger value) => AddAndReturn(new InstLdConstI(size, value));
        public InstructionWriter LdConstIptr(BigInteger value) => AddAndReturn(new InstLdConstIptr(value));
        public InstructionWriter LdLocal(short index) => AddAndReturn(new InstLdLocal(index));
        public InstructionWriter LdField(FieldBuilder field) => AddAndReturn(new InstLdField(field));
        
        
        public InstructionWriter StLocal(short index) => AddAndReturn(new InstStLocal(index));
        public InstructionWriter StField(FieldBuilder field) => AddAndReturn(new InstStField(field));


        public InstructionWriter Add() => AddAndReturn(new InstAdd());
        public InstructionWriter Sub() => AddAndReturn(new InstSub());
        public InstructionWriter Mul() => AddAndReturn(new InstMul());
        public InstructionWriter Div() => AddAndReturn(new InstDiv());
        
        
        public InstructionWriter Extend() => AddAndReturn(new InstExtend());
        public InstructionWriter Trunc() => AddAndReturn(new InstTrunc());
        
        
        public InstructionWriter MStackEnter(StructureBuilder stackFrame) => AddAndReturn(new InstMStackEnter(stackFrame));
        public InstructionWriter MStackLeave() => AddAndReturn(new InstMStackLeave());
        
        
        public InstructionWriter TypeInt(bool signed, byte? size) => AddAndReturn(new FlagIntTyped(signed, size));
        
        
        public InstructionWriter MacroDefineLocal(TypeReference tref) => AddAndReturn(new MacroDefineLocal(tref));
    }
}
