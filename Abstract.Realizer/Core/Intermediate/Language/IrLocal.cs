namespace Abstract.Realizer.Core.Intermediate.Language;

internal class IrLocal(short index) : IrValue, IAssignable
{
    public readonly short Index = index;

    public override string ToString() => $"(local {Index})";
}
