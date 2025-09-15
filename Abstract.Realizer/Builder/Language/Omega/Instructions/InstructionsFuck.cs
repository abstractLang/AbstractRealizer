using System.Numerics;
using System.Reflection.Emit;
using Abstract.Realizer.Builder.ProgramMembers;
using FieldBuilder = Abstract.Realizer.Builder.ProgramMembers.FieldBuilder;
using TypeBuilder = Abstract.Realizer.Builder.ProgramMembers.TypeBuilder;

namespace Abstract.Realizer.Builder.Language.Omega.Instructions;
using i16 = short;
using u1 = bool;
using u128 = UInt128;
using u16 = ushort;
using u32 = uint;
using u64 = ulong;
using u8 = byte;

public interface IOmegaInstruction { }
public interface IOmegaFlag: IOmegaInstruction { }

public struct Inst__Nop : IOmegaInstruction
{
    public override string ToString() => "nop";
}

public struct Inst__Invalid : IOmegaInstruction
{
    public override string ToString() => "invalid";
}

public struct Inst__Pop : IOmegaInstruction
{
    public override string ToString() => "pop";
}

public struct Inst__Dup : IOmegaInstruction
{
    public override string ToString() => "dup";
}

public struct Inst__Swap : IOmegaInstruction
{
    public override string ToString() => "swap";
}

public struct Inst__Call(FunctionBuilder r) : IOmegaInstruction
{
    public override string ToString() => $"call {r.ToReadableReference()}";
}

public struct Inst__Call_virt() : IOmegaInstruction
{
    public override string ToString() => "call.virt";
}

public struct Inst__Add : IOmegaInstruction
{
    public override string ToString() => "add";
}

public struct Inst__Sub : IOmegaInstruction
{
    public override string ToString() => "sub";
}

public struct Inst__Mul(u1 signed) : IOmegaInstruction
{
    public override string ToString() => "mul." + (signed ? 's' : 'u');
}

public struct Inst__Div(u1 signed) : IOmegaInstruction
{
    public override string ToString() => "div." + (signed ? 's' : 'u');
}

public struct Inst__Rem(u1 signed) : IOmegaInstruction
{
    public override string ToString() => "rem." + (signed ? 's' : 'u');
}

public struct Inst__Neg : IOmegaInstruction
{
    public override string ToString() => "neg";
}

public struct Inst__Not : IOmegaInstruction
{
    public override string ToString() => "not";
}

public struct Inst__And : IOmegaInstruction
{
    public override string ToString() => "and";
}

public struct Inst__Or : IOmegaInstruction
{
    public override string ToString() => "or";
}

public struct Inst__Xor : IOmegaInstruction
{
    public override string ToString() => "xor";
}

public struct Inst__Shr : IOmegaInstruction
{
    public override string ToString() => "shr";
}

public struct Inst__Shl : IOmegaInstruction
{
    public override string ToString() => "shl";
}

public struct Inst__Ror : IOmegaInstruction
{
    public override string ToString() => "ror";
}

public struct Inst__Rol : IOmegaInstruction
{
    public override string ToString() => "rol";
}

public struct Inst__Block(string label): IOmegaInstruction {}
public struct Inst__Loop(string label): IOmegaInstruction { }
public struct Inst__If: IOmegaInstruction { }
public struct Inst__Else: IOmegaInstruction { }
public struct Inst__Switch(string[] labels): IOmegaInstruction { }
public struct Inst__End: IOmegaInstruction { }

public struct Inst__Ld_Const_i1(u1 value) : IOmegaInstruction
{
    public override string ToString() => $"ld.const.i1 {value}";
}

public struct Inst__Ld_Const_i8(u8 value) : IOmegaInstruction
{
    public override string ToString() => $"ld.const.i8 0x{value:x} ;;{value}";
}

public struct Inst__Ld_Const_i16(u16 value) : IOmegaInstruction
{
    public override string ToString() => $"ld.const.i16 0x{value:x} ;;{value}";
}

public struct Inst__Ld_Const_i32(u32 value) : IOmegaInstruction
{
    public override string ToString() => $"ld.const.i32 0x{value:x} ;;{value}";
}

public struct Inst__Ld_Const_i64(u64 value) : IOmegaInstruction
{
    public override string ToString() => $"ld.const.i64 0x{value:x} ;;{value}";
}

public struct Inst__Ld_Const_i128(u128 value) : IOmegaInstruction
{
    public override string ToString() => $"ld.const.i128 0x{value:x} ;;{value}";
}

public struct Inst__Ld_Const_iptr(u64 value) : IOmegaInstruction
{
    public override string ToString() => $"ld.const.iptr 0x{value:x} ;;{value}";
}

public struct Inst__Ld_Const_i(u8 len, BigInteger value) : IOmegaInstruction
{
    public override string ToString() => $"ld.const.i{len} 0x{value:x} ;;{value}";
}

public struct Inst__Ld_Const_Null : IOmegaInstruction
{
    public override string ToString() => "ld.const.null";
}

public struct Inst__Ld_New_Slice : IOmegaInstruction
{
    public override string ToString() => "ld.new.slice";
}

public struct Inst__Ld_New_Object(TypeBuilder r) : IOmegaInstruction
{
    public override string ToString() => $"ld.new.obj {r.Name}";
}

public struct Inst__Ld_Local(i16 index) : IOmegaInstruction
{
    public override string ToString() => index < -3
            ? $"ld.arg ${-index - 1}"
            : index < 0
                ? $"ld.arg.{-index - 1}"
                : $"ld.local ${index}";
}

public struct Inst__Ld_Local_Ref(i16 index) : IOmegaInstruction
{
    public override string ToString() => index < 0
            ? $"ld.arg.ref {-index - 1}"
            : $"ld.local.ref ${index}";
}

public struct Inst__Ld_Field() : IOmegaInstruction
{
    public override string ToString() => $"ld.field";
}

public struct Inst__Ld_Field_Ref() : IOmegaInstruction
{
    public override string ToString() => $"ld.field.ref";
}

public struct Inst__Ld_Func_Ref(FunctionBuilder r) : IOmegaInstruction
{
    public override string ToString() => $"ld.func.ref {r.Name}";
}

public struct Inst__Ld_Type_Ref(TypeBuilder r) : IOmegaInstruction
{
    public override string ToString() => $"ld.type.ref {r.Name}";
}

public struct Inst__Ld_Index : IOmegaInstruction
{
    public override string ToString() => $"ld.index";
}

public struct Inst__St_Local(i16 index) : IOmegaInstruction
{
    public override string ToString() => index < -3
            ? $"st.arg ${-index - 1}"
            : index < 0 ?
                $"st.arg.{-index - 1}"
                : $"st.local ${index}";
}

public struct Inst__St_Field(FieldBuilder f) : IOmegaInstruction
{
    public override string ToString() => $"st.field {f.ToReadableReference()}";
}

public struct Inst__St_Index : IOmegaInstruction
{
    public override string ToString() => $"ld.index";
}

public struct Inst__Extend(u8 len) : IOmegaInstruction
{
    public override string ToString() => $"extend {len}";
}

public struct Inst__Trunc(u8 len) : IOmegaInstruction
{
    public override string ToString() => $"trunc {len}";
}

public struct Inst__Sigcast(bool signed) : IOmegaInstruction
{
    public override string ToString() => $"sigcast." + (signed ? 's' : 'u');
}
public struct Inst__Bitcast(u8 len) : IOmegaInstruction
{
    public override string ToString() => $"bitcast {len}";
}


public struct Inst__Mem_Copy : IOmegaInstruction
{
    public override string ToString() => "mem.copy";
}

public struct Inst__Mem_Fill : IOmegaInstruction
{
    public override string ToString() => "mem.fill";
}

public struct Inst__Mem_Eq : IOmegaInstruction
{
    public override string ToString() => "mem.eq";
}

public struct Flag_Allow_Ovf : IOmegaFlag
{
    public override string ToString() => "allow.ovf.";
}

public struct Flag_Allow_Nil : IOmegaFlag
{
    public override string ToString() => "allow.nil.";
}

public struct Inst__Src_offset_global(u32 off): IOmegaInstruction { }
public struct Inst__Src_offset_rel(u16 off): IOmegaInstruction { }
