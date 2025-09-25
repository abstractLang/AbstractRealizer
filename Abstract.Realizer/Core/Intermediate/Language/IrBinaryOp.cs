namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrBinaryOp(BinaryOperation op, IrValue left, IrValue right) : IrValue
{
    public readonly BinaryOperation Op;
    public readonly IrValue Left;
    public readonly IrValue Right;

    public override string ToString() => $"({op.ToString().Replace('_', '.')}"
                                         + $"\n{Left.ToString().TabAllLines()}"
                                         + $"\n{Right.ToString().TabAllLines()})";
}