using System.Numerics;
using System.Text;
using Abstract.Realizer.Builder.Language.Omega.Instructions;
using Abstract.Realizer.Builder.ProgramMembers;
using TypeBuilder = Abstract.Realizer.Builder.ProgramMembers.TypeBuilder;

namespace Abstract.Realizer.Builder.Language.Omega;

public class OmegaBytecodeBuilder: BytecodeBuilder
{
    private List<IOmegaInstruction> _instructions = [];
    public InstructionWriter Writer => new(this);
    
    
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
            case Inst__Nop:
                instQueue.Dequeue();
                sb.Append("nop");
                break;

            case Inst__Ret @r:
                instQueue.Dequeue();
                sb.Append("ret");
                if (r.value) sb.Append(" " + WriteInstructionValue(instQueue).TabAllLines()[1..]);
                break;
            
            case Inst__St_Local @stlocal:
            {
                instQueue.Dequeue();
                if (stlocal.index < 0) sb.Append($"(arg ${(-stlocal.index) - 1}) = ");
                else sb.Append($"(local {stlocal.index}) = ");
                sb.Append(WriteInstructionValue(instQueue).TabAllLines().TrimStart('\t'));
            } break;
            
            case Inst__St_Field @stfld:
            {
                instQueue.Dequeue();
                sb.Append($"(field {stfld.field.ToReadableReference()}) = ");
                sb.Append(WriteInstructionValue(instQueue).TabAllLines().TrimStart('\t'));
            } break;
            
            case Inst__St_Instance_Field @stfld:
            {
                instQueue.Dequeue();
                sb.Append($"(field {stfld.field.ToReadableReference()}) = ");
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
            case Inst__Not: sb.Append($"(not\n\t{WriteInstructionValue(instQueue)})"); break;
            case Inst__Add: sb.Append($"(add" +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()}" +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            case Inst__Mul @m: sb.Append($"(mul." + (m.signed ? 's' : 'u') +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()}" +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            
            case Inst__Sigcast @s: sb.Append("(sigcast." + (s.segned ? 's' : 'u') + $" {WriteInstructionValue(instQueue)})"); break;
            case Inst__Trunc @t: sb.Append($"(trunc {t.len} {WriteInstructionValue(instQueue)})"); break;
            case Inst__Extend @e: sb.Append($"(extend {e.len} {WriteInstructionValue(instQueue)})"); break;
            
            case Inst__Ld_Const_i @ldconsti: sb.Append($"(const {ldconsti.len} 0x{ldconsti.value:x})"); break;
            case Inst__Ld_Const_i1 @ldc: sb.Append("(const 1 " + (ldc.value ? "true" : "false" + ")")); break;
            case Inst__Ld_Const_i8 @ldc: sb.Append($"(const 8 0x{ldc.value:x})"); break;
            case Inst__Ld_Const_i16 @ldc: sb.Append($"(const 16 0x{ldc.value:x})"); break;
            case Inst__Ld_Const_i32 @ldc: sb.Append($"(const 32 0x{ldc.value:x})"); break;
            case Inst__Ld_Const_i64 @ldc: sb.Append($"(const 64 0x{ldc.value:x})"); break;
            case Inst__Ld_Const_i128 @ldc: sb.Append($"(const 128 0x{ldc.value:x})"); break;

            case Inst__Ld_Local @ldl:
                if (ldl.local < 0) sb.Append($"(arg {(-ldl.local)-1})");
                else sb.Append($"(local {ldl.local})");
                break;
            case Inst__Ld_Field @ldf: sb.Append($"(field {ldf.field.ToReadableReference()})"); break;
            case Inst__Ld_Instance_Field @acf: sb.Append($"(field {acf.field.ToReadableReference()})"); break;
            
            case Inst__Call @call:
            {
                sb.Append($"(call {call.function.ToReadableReference()} (");
                foreach (var i in call.function.Parameters)
                    sb.Append("\n" + WriteInstructionValue(instQueue).TabAllLines());
                sb.Append("))");
            } break;

            case Inst__Ld_New_Object @newobj:
                sb.Append($"newobj({newobj.type.ToReadableReference()})");
                break;
            
            default: throw new NotImplementedException();
        }

        if (instQueue.Count > 0 && instQueue.Peek() 
            is Inst__Ld_Instance_Field
            or Inst__St_Instance_Field)
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

        public InstructionWriter Nop() => AddAndReturn(new Inst__Nop());
        public InstructionWriter Invalid() => AddAndReturn(new Inst__Invalid());
        public InstructionWriter Call(BaseFunctionBuilder r) => AddAndReturn(new Inst__Call(r));
        public InstructionWriter CallVirt() => AddAndReturn(new Inst__Call_virt());
        public InstructionWriter Ret(bool value) => AddAndReturn(new Inst__Ret(value));
        
        public InstructionWriter Add() => AddAndReturn(new Inst__Add());
        public InstructionWriter Sub() => AddAndReturn(new Inst__Sub());
        public InstructionWriter Mul(bool signed) => AddAndReturn(new Inst__Mul(signed));
        public InstructionWriter Div(bool signed) => AddAndReturn(new Inst__Div(signed));
        public InstructionWriter Rem(bool signed) => AddAndReturn(new Inst__Rem(signed));
        public InstructionWriter Neg() => AddAndReturn(new Inst__Neg());
        public InstructionWriter Not() => AddAndReturn(new Inst__Not());
        public InstructionWriter And() => AddAndReturn(new Inst__And());
        public InstructionWriter Or() => AddAndReturn(new Inst__Or());
        public InstructionWriter Xor() => AddAndReturn(new Inst__Xor());
        public InstructionWriter Shr() => AddAndReturn(new Inst__Shr());
        public InstructionWriter Shl() => AddAndReturn(new Inst__Shl());
        public InstructionWriter Ror() => AddAndReturn(new Inst__Ror());
        public InstructionWriter Rol() => AddAndReturn(new Inst__Rol());
        
        public InstructionWriter Block(string label) => AddAndReturn(new Inst__Block(label));
        public InstructionWriter Loop(string label) => AddAndReturn(new Inst__Loop(label));
        public InstructionWriter If() => AddAndReturn(new Inst__If());
        public InstructionWriter Else() => AddAndReturn(new Inst__Else());
        public InstructionWriter Switch() => AddAndReturn(new Inst__Switch());
        public InstructionWriter End() => AddAndReturn(new Inst__End());
        
        public InstructionWriter LdConstI1(bool value) => AddAndReturn(new Inst__Ld_Const_i1(value));
        public InstructionWriter LdConstI8(byte value) => AddAndReturn(new Inst__Ld_Const_i8(value));
        public InstructionWriter LdConstI16(ushort value) => AddAndReturn(new Inst__Ld_Const_i16(value));
        public InstructionWriter LdConstI32(uint value) => AddAndReturn(new Inst__Ld_Const_i32(value));
        public InstructionWriter LdConstI64(ulong value) => AddAndReturn(new Inst__Ld_Const_i64(value));
        public InstructionWriter LdConstI128(UInt128 value) => AddAndReturn(new Inst__Ld_Const_i128(value));
        public InstructionWriter LdConstIptr(ulong value) => AddAndReturn(new Inst__Ld_Const_iptr(value));
        public InstructionWriter LdConstI(byte size, BigInteger value) => AddAndReturn(new Inst__Ld_Const_i(size, value));
        public InstructionWriter LdConstNull() => AddAndReturn(new Inst__Ld_Const_Null());
        
        public InstructionWriter LdNewSlice() => AddAndReturn(new Inst__Ld_New_Slice());
        public InstructionWriter LdNewObject(TypeBuilder r) => AddAndReturn(new Inst__Ld_New_Object(r));
        
        public InstructionWriter LdLocal(short index) => AddAndReturn(new Inst__Ld_Local(index));
        public InstructionWriter LdLocalRef(short index) => AddAndReturn(new Inst__Ld_Local_Ref(index));
        public InstructionWriter LdField(FieldBuilder r) => AddAndReturn(new Inst__Ld_Field(r));
        public InstructionWriter LdFieldRef() => AddAndReturn(new Inst__Ld_Field_Ref());
        public InstructionWriter LdInstanceField(FieldBuilder r) => AddAndReturn(new Inst__Ld_Instance_Field(r));
        public InstructionWriter LdInstanceFieldRef() => AddAndReturn(new Inst__Ld_Instance_Field_Ref());
        public InstructionWriter LdFuncRef(FunctionBuilder funcref) => AddAndReturn(new Inst__Ld_Func_Ref(funcref));
        public InstructionWriter LdTypeRef(TypeBuilder typeref) => AddAndReturn(new Inst__Ld_Type_Ref(typeref));
        public InstructionWriter LdIndex() => AddAndReturn(new Inst__Ld_Index());
        
        public InstructionWriter StLocal(short index) => AddAndReturn(new Inst__St_Local(index));
        public InstructionWriter StField(FieldBuilder r) => AddAndReturn(new Inst__St_Field(r));
        public InstructionWriter StInstanceField(FieldBuilder r) => AddAndReturn(new Inst__St_Instance_Field(r));
        public InstructionWriter StIndex() => AddAndReturn(new Inst__St_Index());
        
        public InstructionWriter Extend(byte size) => AddAndReturn(new Inst__Extend(size));
        public InstructionWriter Trunc(byte size) => AddAndReturn(new Inst__Trunc(size));
        public InstructionWriter Sigcast(bool signess) => AddAndReturn(new Inst__Sigcast(signess));
        public InstructionWriter Bitcast() => AddAndReturn(new Inst__Bitcast());
        
        public InstructionWriter MemCopy() => AddAndReturn(new Inst__Mem_Copy());
        public InstructionWriter MemFill() => AddAndReturn(new Inst__Mem_Fill());
        public InstructionWriter MemEq() => AddAndReturn(new Inst__Mem_Eq());
        
        public InstructionWriter AllowOvf() => AddAndReturn(new Flag_Allow_Ovf());
        public InstructionWriter AllowNil() => AddAndReturn(new Flag_Allow_Nil());
    }
}
