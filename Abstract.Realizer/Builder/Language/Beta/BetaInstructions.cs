using System.Numerics;
using System.Reflection;
using Abstract.Realizer.Builder.ProgramMembers;

namespace Abstract.Realizer.Builder.Language.Beta;

public interface IBetaInstruction { }
public interface IBetaFlag: IBetaInstruction { }


public readonly struct InstNop : IBetaInstruction
{
    public override string ToString() => "nop";
}
public readonly struct InstInvalid : IBetaInstruction
{
    public override string ToString() => "invalid";
}
public readonly struct InstPop : IBetaInstruction
{
    public override string ToString() => "pop";
}
public readonly struct InstDup : IBetaInstruction
{
    public override string ToString() => "dup";
}
public readonly struct InstSwap : IBetaInstruction
{
    public override string ToString() => "swap";
}
public readonly struct InstCall(BaseFunctionBuilder f) : IBetaInstruction
{
    public readonly BaseFunctionBuilder Function = f;
    public override string ToString() => $"call {Function.ToReadableReference()}";
}
public readonly struct InstCallVirt : IBetaInstruction
{
    public override string ToString() => $"call.virt";
}


public readonly struct InstRet : IBetaInstruction
{
    public override string ToString() => $"ret";
}
public readonly struct InstBreak : IBetaInstruction
{
    public override string ToString() => $"break";
}


public readonly struct InstLdConstI(byte size, BigInteger value) : IBetaInstruction
{
    public readonly byte Size = size;
    public readonly BigInteger Value = value;

    public override string ToString() => $"ld.const.i{Size} {Value}";
}
public readonly struct InstLdConstIptr(BigInteger value) : IBetaInstruction
{
    public readonly BigInteger Value = value;

    public override string ToString() => $"ld.const.iptr {Value}";
}
public readonly struct InstLdLocal(short index) : IBetaInstruction
{
    public readonly short Index = index;
    public override string ToString() => (Index < 0) ? $"ld.arg {(-Index)-1}" : $"ld.local {Index}";
}
public readonly struct InstLdField(FieldBuilder field) : IBetaInstruction
{
    public readonly FieldBuilder Field = field;
    public override string ToString() => $"ld.field {Field.ToReadableReference()}";
}

public readonly struct InstStLocal(short index) : IBetaInstruction
{
    public readonly short Index = index;
    public override string ToString() => (Index < 0) ? $"st.arg {(-Index)-1}" : $"st.local {Index}";
}
public readonly struct InstStField(FieldBuilder field) : IBetaInstruction
{
    public readonly FieldBuilder Field = field;
    public override string ToString() => $"st.field {Field.ToReadableReference()}";
}


public readonly struct InstAdd() : IBetaInstruction
{
    public override string ToString() => "add";
}
public readonly struct InstSub() : IBetaInstruction
{
    public override string ToString() => "sub";
}
public readonly struct InstMul() : IBetaInstruction
{
    public override string ToString() => "mul";
}
public readonly struct InstDiv() : IBetaInstruction
{
    public override string ToString() => "div";
}


public readonly struct InstExtend(byte size) : IBetaInstruction
{
    public readonly byte Size = size;
    public override string ToString() => $"extend {Size}";
}
public readonly struct InstTrunc(byte size) : IBetaInstruction
{
    public readonly byte Size = size;
    public override string ToString() => $"trunc {Size}";
}


public readonly struct FlagIntTyped(bool signed, byte? size) : IBetaFlag
{
    public readonly bool Signed = signed;
    public readonly byte? Size = size;
    public override string ToString() => (Signed ? "s" : "u") + $"{Size}.";
}
