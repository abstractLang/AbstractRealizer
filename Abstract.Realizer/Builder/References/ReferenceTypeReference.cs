namespace Abstract.Realizer.Builder.References;

public class ReferenceTypeReference(TypeReference subtype, uint? alignment = null): TypeReference
{
    public readonly TypeReference Subtype = subtype;
    public override string ToString() => $"*{Subtype}";
    
    public sealed override uint? Alignment { get; init; } = alignment;
}