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

        foreach (var i in _instructions)
        {
            if (i is IOmegaFlag) sb.Append(i);
            else sb.AppendLine(i.ToString());
        }
        
        return sb.ToString();
    }

    public sealed class InstructionWriter
    {
        private OmegaBytecodeBuilder parentBuilder;
        internal InstructionWriter(OmegaBytecodeBuilder builder) => parentBuilder = builder;

        private InstructionWriter AddAndReturn(IOmegaInstruction value)
        {
            parentBuilder._instructions.Add(value);
            return this;
        }

        public InstructionWriter Nop() => AddAndReturn(new Inst__Nop());
        public InstructionWriter Invalid() => AddAndReturn(new Inst__Invalid());
        public InstructionWriter Pop() => AddAndReturn(new Inst__Pop());
        public InstructionWriter Dup() => AddAndReturn(new Inst__Dup());
        public InstructionWriter Swap() => AddAndReturn(new Inst__Swap());
        public InstructionWriter Call(FunctionBuilder r) => AddAndReturn(new Inst__Call(r));
        public InstructionWriter CallVirt() => AddAndReturn(new Inst__Call_virt());
        
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
        public InstructionWriter LdNewObject() => AddAndReturn(new Inst__Ld_New_Object());
        
        public InstructionWriter LdLocal(short index) => AddAndReturn(new Inst__Ld_Local(index));
        public InstructionWriter LdLocalRef(short index) => AddAndReturn(new Inst__Ld_Local_Ref(index));
        public InstructionWriter LdField() => AddAndReturn(new Inst__Ld_Field());
        public InstructionWriter LdFieldRef() => AddAndReturn(new Inst__Ld_Field_Ref());
        public InstructionWriter LdFuncRef(FunctionBuilder funcref) => AddAndReturn(new Inst__Ld_Func_Ref(funcref));
        public InstructionWriter LdTypeRef(TypeBuilder typeref) => AddAndReturn(new Inst__Ld_Type_Ref(typeref));
        public InstructionWriter LdIndex() => AddAndReturn(new Inst__Ld_Index());
        
        public InstructionWriter StLocal(short index) => AddAndReturn(new Inst__St_Local(index));
        public InstructionWriter StField() => AddAndReturn(new Inst__St_Field());
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
