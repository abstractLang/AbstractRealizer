using System.Numerics;
using Abstract.Realizer.Builder.ProgramMembers;
using Abstract.Realizer.Builder.References;
using TypeBuilder = Abstract.Realizer.Builder.ProgramMembers.TypeBuilder;

using i16 = short;
using u1 = bool;
using u128 = System.UInt128;
using u16 = ushort;
using u32 = uint;
using u64 = ulong;
using u8 = byte;

namespace Abstract.Realizer.Builder.Language.Omega;

public interface IOmegaInstruction { }
public interface IOmegaFlag: IOmegaInstruction { }
public interface IOmegaMacro: IOmegaInstruction { }
public interface IOmegaTypePrefix { }
public interface IOmegaRequiresTypePrefix { }


public readonly struct InstNop : IOmegaInstruction
{
    public override string ToString() => "nop";
}
public readonly struct InstInvalid : IOmegaInstruction
{
    public override string ToString() => "invalid";
}


public readonly struct InstCall(BaseFunctionBuilder r) : IOmegaInstruction
{
    public readonly BaseFunctionBuilder function = r;
    public override string ToString() => $"call {r.ToReadableReference()}";
}
public readonly struct InstCallvirt() : IOmegaInstruction
{
    public override string ToString() => "call.virt";
}
public readonly struct InstRet(bool value) : IOmegaInstruction
{
    public readonly bool value = value;
    public override string ToString() => "ret" + (value ? "v" : "");
}


public readonly struct InstAdd : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "add";
}
public readonly struct InstSub : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "sub";
}
public readonly struct InstMul : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "mul";
}
public readonly struct InstDiv : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "div";
}
public readonly struct InstRem : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "rem";
}
public readonly struct InstNeg : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "neg";
}
public readonly struct InstNot : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "not";
}
public readonly struct InstAnd : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "and";
}
public readonly struct InstOr  : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "or";
}
public readonly struct InstXor : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "xor";
}
public readonly struct InstShr : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "shr";
}
public readonly struct InstShl : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "shl";
}
public readonly struct InstRor : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "ror";
}
public readonly struct InstRol : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "rol";
}


public readonly struct InstBlock(string label): IOmegaInstruction {}
public readonly struct InstLoop(string label): IOmegaInstruction { }
public readonly struct InstIf: IOmegaInstruction { }
public readonly struct InstElse: IOmegaInstruction { }
public readonly struct InstSwitch(string[] labels): IOmegaInstruction { }
public readonly struct InstEnd: IOmegaInstruction { }

public readonly struct InstMStaackEnter(StructureBuilder StackFrame) : IOmegaInstruction
{
    public readonly StructureBuilder StackFrame = StackFrame;
    public override string ToString() => $"mstaack.enter {StackFrame.ToReadableReference()}";
}
public readonly struct InstMStaackLeave() : IOmegaInstruction
{
    public override string ToString() => "mstaack.leave";
}

public readonly struct InstLdConstI1(u1 value) : IOmegaInstruction
{
    public readonly u1 Value = value;
    public override string ToString() => $"ld.const.i1 {Value}";
}
public readonly struct InstLdConstIptr(u64 value) : IOmegaInstruction
{
    public readonly u64 Value = value;
    public override string ToString() => $"ld.const.iptr 0x{Value:x} ;;{Value}";
}
public readonly struct InstLdConstI(u8 len, BigInteger value) : IOmegaInstruction
{
    public readonly u8 Len = len;
    public readonly BigInteger Value = value;
    public override string ToString() => $"ld.const.i{Len} 0x{Value:x} ;;{Value}";
}
public readonly struct InstLdConstNull : IOmegaInstruction
{
    public override string ToString() => "ld.const.null";
}
public readonly struct InstLdNewSlice : IOmegaInstruction
{
    public override string ToString() => "ld.new.slice";
}
public readonly struct InstLdNewObject(TypeBuilder r) : IOmegaInstruction
{
    public readonly TypeBuilder Type = r;
    public override string ToString() => $"ld.new.obj {Type.ToReadableReference()}";
}
public readonly struct InstLdLocal(i16 index) : IOmegaInstruction
{
    public readonly i16 Local = index;
    public override string ToString() => Local < -3
            ? $"ld.arg ${-Local - 1}"
            : Local < 0
                ? $"ld.arg.{-Local - 1}"
                : $"ld.local ${Local}";
}
public readonly struct InstLdLocalRef(i16 index) : IOmegaInstruction
{
    public override string ToString() => index < 0
            ? $"ld.arg.ref {-index - 1}"
            : $"ld.local.ref ${index}";
}
public readonly struct InstLdStaticField(StaticFieldBuilder r) : IOmegaInstruction
{
    public readonly StaticFieldBuilder StaticField = r;
    public override string ToString() => $"ld.static.field {StaticField.ToReadableReference()}";
}
public readonly struct InstLdStaticFieldRef() : IOmegaInstruction
{
    public override string ToString() => $"ld.static.field.ref";
}
public readonly struct InstLdField(InstanceFieldBuilder r) : IOmegaInstruction
{
    public readonly InstanceFieldBuilder StaticField = r;
    public override string ToString() => $"ld.field {StaticField.ToReadableReference()}";
}
public readonly struct InstLdFieldRef(StaticFieldBuilder r) : IOmegaInstruction
{
    public readonly StaticFieldBuilder StaticField = r;
    public override string ToString() => $"ld.field.ref";
}
public readonly struct InstLdFuncRef(FunctionBuilder r) : IOmegaInstruction
{
    public override string ToString() => $"ld.func.ref {r.ToReadableReference()}";
}
public readonly struct InstLdTypeRef(TypeBuilder r) : IOmegaInstruction
{
    public override string ToString() => $"ld.type.ref {r.ToReadableReference()}";
}
public readonly struct InstLdIndex : IOmegaInstruction
{
    public override string ToString() => $"ld.index";
}


public readonly struct InstStLocal(i16 index) : IOmegaInstruction
{
    public readonly i16 index = index;
    public override string ToString() => index < -3
            ? $"st.arg ${-index - 1}"
            : index < 0 ?
                $"st.arg.{-index - 1}"
                : $"st.local ${index}";
}
public readonly struct InstStStaticField(StaticFieldBuilder f) : IOmegaInstruction
{
    public readonly StaticFieldBuilder StaticField = f;
    public override string ToString() => $"st.static.field {f.ToReadableReference()}";
}
public readonly struct InstStField(InstanceFieldBuilder f) : IOmegaInstruction
{
    public readonly InstanceFieldBuilder StaticField = f;
    public override string ToString() => $"st.field {f.ToReadableReference()}";
}
public readonly struct InstStIndex : IOmegaInstruction
{
    public override string ToString() => $"ld.index";
}
public readonly struct InstExtend() : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => "extend";
}
public readonly struct InstTrunc(u8 len) : IOmegaInstruction, IOmegaRequiresTypePrefix
{
    public override string ToString() => $"trunc";
}
public readonly struct InstSigcast(bool signed) : IOmegaInstruction
{
    public readonly bool Signed = signed;
    public override string ToString() => $"sigcast." + (Signed ? 's' : 'u');
}
public readonly struct InstBitcast(u8 len) : IOmegaInstruction
{
    public readonly u8 Len = len;
    public override string ToString() => $"bitcast {Len}";
}


public readonly struct InstMemCopy : IOmegaInstruction
{
    public override string ToString() => "mem.copy";
}
public readonly struct InstMemFill : IOmegaInstruction
{
    public override string ToString() => "mem.fill";
}
public readonly struct InstMemEq : IOmegaInstruction
{
    public override string ToString() => "mem.eq";
}


public readonly struct FlagAllowOvf : IOmegaFlag
{
    public override string ToString() => "allow.ovf.";
}
public readonly struct FlagAllowNil : IOmegaFlag
{
    public override string ToString() => "allow.nil.";
}


public readonly struct FlagTypeInt(bool signed, u8? size) : IOmegaFlag, IOmegaTypePrefix
{
    public readonly bool Signed = signed;
    public readonly byte? Size = size;
    public override string ToString() => (Signed ? 's' : 'u') + (Size.HasValue ? $"{Size.Value}" : "int") + '.';
}
public readonly struct FlagTypeFloat(u8 size) : IOmegaFlag, IOmegaTypePrefix
{
    public readonly byte Size = size;
    public override string ToString() => $"f{Size}.";
}
public readonly struct FlagTypeObject() : IOmegaFlag, IOmegaTypePrefix
{
    public override string ToString() => "obj.";
}

public readonly struct MacroDefineLocal(TypeReference typeref) : IOmegaMacro
{
    public readonly TypeReference Type = typeref;
    public override string ToString() => $"$DEFINE_LOCAL {Type}";
}


public readonly struct InstSrcOffsetGlobal(u32 off): IOmegaInstruction { }
public readonly struct InstSrcOffsetRel(u16 off): IOmegaInstruction { }
