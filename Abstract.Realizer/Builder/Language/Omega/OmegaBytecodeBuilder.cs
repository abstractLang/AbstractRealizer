using System.Numerics;
using System.Text;
using Abstract.Realizer.Builder.ProgramMembers;
using TypeBuilder = Abstract.Realizer.Builder.ProgramMembers.TypeBuilder;

namespace Abstract.Realizer.Builder.Language.Omega;

public class OmegaBytecodeBuilder: BytecodeBuilder
{
    private List<IOmegaInstruction> _instructions = [];
    public List<IOmegaInstruction> InstructionsList => _instructions;
    public InstructionWriter Writer => new(this);
    
    
    public override object Clone()
    {
        var clone = new OmegaBytecodeBuilder();
        clone._instructions = _instructions.ToList();
        return clone;
    }
    public override string ToString()
    {
        var sb = new StringBuilder();
        var q = new Queue<IOmegaInstruction>(_instructions);
        while (q.Count > 0) WriteInstruction(sb, q);

        sb.AppendLine();
        foreach (var i in _instructions) sb.AppendLine(";; " + i);
        
        return sb.ToString();
    }

    
    private void WriteInstruction(StringBuilder sb, Queue<IOmegaInstruction> instQueue, bool recursive = false)
    {
        var a = instQueue.Peek();
        switch (a)
        {
            case InstNop:
                instQueue.Dequeue();
                sb.Append("nop");
                break;

            case InstRet @r:
                instQueue.Dequeue();
                sb.Append("ret");
                if (r.value) sb.Append(" " + WriteInstructionValue(instQueue).TabAllLines()[1..]);
                break;
            
            case InstStLocal @stlocal:
            {
                instQueue.Dequeue();
                if (stlocal.index < 0) sb.Append($"(arg ${(-stlocal.index) - 1}) = ");
                else sb.Append($"(local {stlocal.index}) = ");
                sb.Append(WriteInstructionValue(instQueue).TabAllLines().TrimStart('\t'));
            } break;
            
            case InstStStaticField @stfld:
            {
                instQueue.Dequeue();
                sb.Append($"(field {stfld.StaticField.ToReadableReference()}) = ");
                sb.Append(WriteInstructionValue(instQueue).TabAllLines().TrimStart('\t'));
            } break;
            
            case InstStField @stfld:
            {
                instQueue.Dequeue();
                sb.Append($"(field {stfld.StaticField.ToReadableReference()}) = ");
                sb.Append(WriteInstructionValue(instQueue).TabAllLines().TrimStart('\t'));
            } break;
            
            default:
                sb.Append(WriteInstructionValue(instQueue).TrimStart('\t'));
                break;
        }
        if (!recursive) sb.AppendLine();
    }
    private string WriteInstructionValue(Queue<IOmegaInstruction> instQueue)
    {
        if (instQueue.Count == 0) return "<eof>";
        var sb = new StringBuilder();
        
        var a = instQueue.Dequeue();
        switch (a)
        {
            case InstNot: sb.Append($"(not\n\t{WriteInstructionValue(instQueue)})"); break;
            case InstAdd: sb.Append($"(add" +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()}" +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            case InstMul @m: sb.Append($"(mul." + (m.signed ? 's' : 'u') +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()}" +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            
            case InstSigcast @s: sb.Append("(sigcast." + (s.segned ? 's' : 'u') + $" {WriteInstructionValue(instQueue)})"); break;
            case InstTrunc @t: sb.Append($"(trunc {t.len} {WriteInstructionValue(instQueue)})"); break;
            case InstExtend @e: sb.Append($"(extend {e.len} {WriteInstructionValue(instQueue)})"); break;
            
            case InstLdConstI @ldconsti: sb.Append($"(const {ldconsti.len} 0x{ldconsti.value:x})"); break;
            case InstLdConstI1 @ldc: sb.Append("(const 1 " + (ldc.value ? "true" : "false" + ")")); break;
            case InstLdConstIptr @ldc: sb.Append($"(const ptr 0x{ldc.value:x})"); break;

            case InstLdLocal @ldl:
                if (ldl.local < 0) sb.Append($"(arg {(-ldl.local)-1})");
                else sb.Append($"(local {ldl.local})");
                break;
            case InstLdStaticField @ldf: sb.Append($"(field {ldf.StaticField.ToReadableReference()})"); break;
            case InstLdField @acf: sb.Append($"(field {acf.StaticField.ToReadableReference()})"); break;
            
            case InstCall @call:
            {
                sb.Append($"(call {call.function.ToReadableReference()} (");
                foreach (var i in call.function.Parameters)
                    sb.Append("\n" + WriteInstructionValue(instQueue).TabAllLines());
                sb.Append("))");
            } break;

            case InstLdNewObject @newobj:
                sb.Append($"newobj({newobj.type.ToReadableReference()})");
                break;
            
            default: throw new NotImplementedException();
        }

        if (instQueue.Count > 0 && instQueue.Peek() 
            is InstLdField
            or InstStField)
        {
            sb.Append("->");
            WriteInstruction(sb, instQueue, true);
        }

        return sb.ToString();
    }
    

    public struct InstructionWriter
    {
        private OmegaBytecodeBuilder _parentBuilder;
        internal InstructionWriter(OmegaBytecodeBuilder builder) => _parentBuilder = builder;

        private InstructionWriter AddAndReturn(IOmegaInstruction value)
        {
            _parentBuilder._instructions.Add(value);
            return this;
        }

        public InstructionWriter Nop() => AddAndReturn(new InstNop());
        public InstructionWriter Invalid() => AddAndReturn(new InstInvalid());
        public InstructionWriter Call(BaseFunctionBuilder r) => AddAndReturn(new InstCall(r));
        public InstructionWriter CallVirt() => AddAndReturn(new InstCallvirt());
        public InstructionWriter Ret(bool value) => AddAndReturn(new InstRet(value));
        
        public InstructionWriter Add() => AddAndReturn(new InstAdd());
        public InstructionWriter Sub() => AddAndReturn(new InstSub());
        public InstructionWriter Mul(bool signed) => AddAndReturn(new InstMul(signed));
        public InstructionWriter Div(bool signed) => AddAndReturn(new InstDiv(signed));
        public InstructionWriter Rem(bool signed) => AddAndReturn(new InstRem(signed));
        public InstructionWriter Neg() => AddAndReturn(new InstNeg());
        public InstructionWriter Not() => AddAndReturn(new InstNot());
        public InstructionWriter And() => AddAndReturn(new InstAnd());
        public InstructionWriter Or() => AddAndReturn(new InstOr());
        public InstructionWriter Xor() => AddAndReturn(new InstXor());
        public InstructionWriter Shr() => AddAndReturn(new InstShr());
        public InstructionWriter Shl() => AddAndReturn(new InstShl());
        public InstructionWriter Ror() => AddAndReturn(new InstRor());
        public InstructionWriter Rol() => AddAndReturn(new InstRol());
        
        public InstructionWriter Block(string label) => AddAndReturn(new InstBlock(label));
        public InstructionWriter Loop(string label) => AddAndReturn(new InstLoop(label));
        public InstructionWriter If() => AddAndReturn(new InstIf());
        public InstructionWriter Else() => AddAndReturn(new InstElse());
        public InstructionWriter Switch() => AddAndReturn(new InstSwitch());
        public InstructionWriter End() => AddAndReturn(new InstEnd());
        
        public InstructionWriter LdConstI1(bool value) => AddAndReturn(new InstLdConstI1(value));
        public InstructionWriter LdConstIptr(ulong value) => AddAndReturn(new InstLdConstIptr(value));
        public InstructionWriter LdConstI(byte size, BigInteger value) => AddAndReturn(new InstLdConstI(size, value));
        public InstructionWriter LdConstNull() => AddAndReturn(new InstLdConstNull());
        
        public InstructionWriter LdNewSlice() => AddAndReturn(new InstLdNewSlice());
        public InstructionWriter LdNewObject(TypeBuilder r) => AddAndReturn(new InstLdNewObject(r));
        
        public InstructionWriter LdLocal(short index) => AddAndReturn(new InstLdLocal(index));
        public InstructionWriter LdLocalRef(short index) => AddAndReturn(new InstLdLocalRef(index));
        
        public InstructionWriter LdField(StaticFieldBuilder r) => AddAndReturn(new InstLdStaticField(r));
        public InstructionWriter LdField(InstanceFieldBuilder r) => AddAndReturn(new InstLdField(r));
        public InstructionWriter LdFieldRef() => AddAndReturn(new InstLdFieldRef());
        //public InstructionWriter LdFieldRef() => AddAndReturn(new InstLdStaticField());
        public InstructionWriter LdFuncRef(FunctionBuilder funcref) => AddAndReturn(new InstLdFuncRef(funcref));
        public InstructionWriter LdTypeRef(TypeBuilder typeref) => AddAndReturn(new InstLdTypeRef(typeref));
        public InstructionWriter LdIndex() => AddAndReturn(new InstLdIndex());
        
        public InstructionWriter StLocal(short index) => AddAndReturn(new InstStLocal(index));
        public InstructionWriter StField(StaticFieldBuilder r) => AddAndReturn(new InstStStaticField(r));
        public InstructionWriter StField(InstanceFieldBuilder r) => AddAndReturn(new InstStField(r));
        public InstructionWriter StIndex() => AddAndReturn(new InstStIndex());
        
        public InstructionWriter Extend(byte size) => AddAndReturn(new InstExtend(size));
        public InstructionWriter Trunc(byte size) => AddAndReturn(new InstTrunc(size));
        public InstructionWriter Sigcast(bool signess) => AddAndReturn(new InstSigcast(signess));
        public InstructionWriter Bitcast() => AddAndReturn(new InstBitcast());
        
        public InstructionWriter MemCopy() => AddAndReturn(new InstMemCopy());
        public InstructionWriter MemFill() => AddAndReturn(new InstMemFill());
        public InstructionWriter MemEq() => AddAndReturn(new InstMemEq());
        
        public InstructionWriter AllowOvf() => AddAndReturn(new FlagAllowOvf());
        public InstructionWriter AllowNil() => AddAndReturn(new FlagAllowNil());
    }
}
