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

public readonly struct Inst__Nop : IOmegaInstruction
{
    public override string ToString() => "nop";
}

public readonly struct Inst__Invalid : IOmegaInstruction
{
    public override string ToString() => "invalid";
}

public readonly struct Inst__Call(BaseFunctionBuilder r) : IOmegaInstruction
{
    public readonly BaseFunctionBuilder function = r;
    public override string ToString() => $"call {r.ToReadableReference()}";
}

public readonly struct Inst__Call_virt() : IOmegaInstruction
{
    public override string ToString() => "call.virt";
}
public readonly struct Inst__Ret(bool value) : IOmegaInstruction
{
    public readonly bool value = value;
    public override string ToString() => "ret" + (value ? "v" : "");
}

public readonly struct Inst__Add : IOmegaInstruction
{
    public override string ToString() => "add";
}

public readonly struct Inst__Sub : IOmegaInstruction
{
    public override string ToString() => "sub";
}

public readonly struct Inst__Mul(u1 signed) : IOmegaInstruction
{
    public readonly bool signed = signed;
    public override string ToString() => "mul." + (signed ? 's' : 'u');
}

public readonly struct Inst__Div(u1 signed) : IOmegaInstruction
{
    public override string ToString() => "div." + (signed ? 's' : 'u');
}

public readonly struct Inst__Rem(u1 signed) : IOmegaInstruction
{
    public override string ToString() => "rem." + (signed ? 's' : 'u');
}

public readonly struct Inst__Neg : IOmegaInstruction
{
    public override string ToString() => "neg";
}

public readonly struct Inst__Not : IOmegaInstruction
{
    public override string ToString() => "not";
}

public readonly struct Inst__And : IOmegaInstruction
{
    public override string ToString() => "and";
}

public readonly struct Inst__Or : IOmegaInstruction
{
    public override string ToString() => "or";
}

public readonly struct Inst__Xor : IOmegaInstruction
{
    public override string ToString() => "xor";
}

public readonly struct Inst__Shr : IOmegaInstruction
{
    public override string ToString() => "shr";
}

public readonly struct Inst__Shl : IOmegaInstruction
{
    public override string ToString() => "shl";
}

public readonly struct Inst__Ror : IOmegaInstruction
{
    public override string ToString() => "ror";
}

public readonly struct Inst__Rol : IOmegaInstruction
{
    public override string ToString() => "rol";
}

public readonly struct Inst__Block(string label): IOmegaInstruction {}
public readonly struct Inst__Loop(string label): IOmegaInstruction { }
public readonly struct Inst__If: IOmegaInstruction { }
public readonly struct Inst__Else: IOmegaInstruction { }
public readonly struct Inst__Switch(string[] labels): IOmegaInstruction { }
public readonly struct Inst__End: IOmegaInstruction { }

public readonly struct Inst__Ld_Const_i1(u1 value) : IOmegaInstruction
{
    public readonly u1 value = value;
    public override string ToString() => $"ld.const.i1 {value}";
}

public readonly struct Inst__Ld_Const_i8(u8 value) : IOmegaInstruction
{
    public readonly u8 value = value;
    public override string ToString() => $"ld.const.i8 0x{value:x} ;;{value}";
}

public readonly struct Inst__Ld_Const_i16(u16 value) : IOmegaInstruction
{
    public readonly u16 value = value;
    public override string ToString() => $"ld.const.i16 0x{value:x} ;;{value}";
}

public readonly struct Inst__Ld_Const_i32(u32 value) : IOmegaInstruction
{
    public readonly u32 value = value;
    public override string ToString() => $"ld.const.i32 0x{value:x} ;;{value}";
}

public readonly struct Inst__Ld_Const_i64(u64 value) : IOmegaInstruction
{
    public readonly u64 value = value;
    public override string ToString() => $"ld.const.i64 0x{value:x} ;;{value}";
}

public readonly struct Inst__Ld_Const_i128(u128 value) : IOmegaInstruction
{
    public readonly u128 value = value;
    public override string ToString() => $"ld.const.i128 0x{value:x} ;;{value}";
}

public readonly struct Inst__Ld_Const_iptr(u64 value) : IOmegaInstruction
{
    public readonly u64 value = value;
    public override string ToString() => $"ld.const.iptr 0x{value:x} ;;{value}";
}

public readonly struct Inst__Ld_Const_i(u8 len, BigInteger value) : IOmegaInstruction
{
    public readonly u8 len = len;
    public readonly BigInteger value = value;
    public override string ToString() => $"ld.const.i{len} 0x{value:x} ;;{value}";
}

public readonly struct Inst__Ld_Const_Null : IOmegaInstruction
{
    public override string ToString() => "ld.const.null";
}

public readonly struct Inst__Ld_New_Slice : IOmegaInstruction
{
    public override string ToString() => "ld.new.slice";
}

public readonly struct Inst__Ld_New_Object(TypeBuilder r) : IOmegaInstruction
{
    public readonly TypeBuilder type = r;
    public override string ToString() => $"ld.new.obj {r.Name}";
}

public readonly struct Inst__Ld_Local(i16 index) : IOmegaInstruction
{
    public readonly i16 local = index;
    public override string ToString() => index < -3
            ? $"ld.arg ${-index - 1}"
            : index < 0
                ? $"ld.arg.{-index - 1}"
                : $"ld.local ${index}";
}

public readonly struct Inst__Ld_Local_Ref(i16 index) : IOmegaInstruction
{
    public override string ToString() => index < 0
            ? $"ld.arg.ref {-index - 1}"
            : $"ld.local.ref ${index}";
}

public readonly struct Inst__Ld_Field(FieldBuilder r) : IOmegaInstruction
{
    public readonly FieldBuilder field = r;
    public override string ToString() => $"ld.field {field.ToReadableReference()}";
}

public readonly struct Inst__Ld_Field_Ref() : IOmegaInstruction
{
    public override string ToString() => $"ld.field.ref";
}
public readonly struct Inst__Ld_Instance_Field(FieldBuilder r) : IOmegaInstruction
{
    public readonly FieldBuilder field = r;
    public override string ToString() => $"ld.instance.field {field.ToReadableReference()}";
}

public readonly struct Inst__Ld_Instance_Field_Ref(FieldBuilder r) : IOmegaInstruction
{
    public readonly FieldBuilder field = r;
    public override string ToString() => $"ld.instance.field.ref";
}

public readonly struct Inst__Ld_Func_Ref(FunctionBuilder r) : IOmegaInstruction
{
    public override string ToString() => $"ld.func.ref {r.Name}";
}

public readonly struct Inst__Ld_Type_Ref(TypeBuilder r) : IOmegaInstruction
{
    public override string ToString() => $"ld.type.ref {r.Name}";
}

public readonly struct Inst__Ld_Index : IOmegaInstruction
{
    public override string ToString() => $"ld.index";
}

public readonly struct Inst__St_Local(i16 index) : IOmegaInstruction
{
    public readonly i16 index = index;
    public override string ToString() => index < -3
            ? $"st.arg ${-index - 1}"
            : index < 0 ?
                $"st.arg.{-index - 1}"
                : $"st.local ${index}";
}

public readonly struct Inst__St_Field(FieldBuilder f) : IOmegaInstruction
{
    public readonly FieldBuilder field = f;
    public override string ToString() => $"st.field {f.ToReadableReference()}";
}
public readonly struct Inst__St_Instance_Field(FieldBuilder f) : IOmegaInstruction
{
    public readonly FieldBuilder field = f;
    public override string ToString() => $"st.instance.field {f.ToReadableReference()}";
}

public readonly struct Inst__St_Index : IOmegaInstruction
{
    public override string ToString() => $"ld.index";
}

public readonly struct Inst__Extend(u8 len) : IOmegaInstruction
{
    public readonly u8 len = len;
    public override string ToString() => $"extend {len}";
}

public readonly struct Inst__Trunc(u8 len) : IOmegaInstruction
{
    public readonly u8 len = len;
    public override string ToString() => $"trunc {len}";
}

public readonly struct Inst__Sigcast(bool signed) : IOmegaInstruction
{
    public readonly bool segned = signed;
    public override string ToString() => $"sigcast." + (signed ? 's' : 'u');
}
public readonly struct Inst__Bitcast(u8 len) : IOmegaInstruction
{
    public override string ToString() => $"bitcast {len}";
}


public readonly struct Inst__Mem_Copy : IOmegaInstruction
{
    public override string ToString() => "mem.copy";
}

public readonly struct Inst__Mem_Fill : IOmegaInstruction
{
    public override string ToString() => "mem.fill";
}

public readonly struct Inst__Mem_Eq : IOmegaInstruction
{
    public override string ToString() => "mem.eq";
}

public readonly struct Flag_Allow_Ovf : IOmegaFlag
{
    public override string ToString() => "allow.ovf.";
}

public readonly struct Flag_Allow_Nil : IOmegaFlag
{
    public override string ToString() => "allow.nil.";
}

public readonly struct Inst__Src_offset_global(u32 off): IOmegaInstruction { }
public readonly struct Inst__Src_offset_rel(u16 off): IOmegaInstruction { }
