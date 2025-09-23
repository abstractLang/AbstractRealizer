using Abstract.Realizer.Builder.ProgramMembers;

namespace Abstract.Realizer.Builder.Language.Beta;

public interface IBetaInstruction { }


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
public readonly struct InstCall(FunctionBuilder f) : IBetaInstruction
{
    public readonly FunctionBuilder Function = f;
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
