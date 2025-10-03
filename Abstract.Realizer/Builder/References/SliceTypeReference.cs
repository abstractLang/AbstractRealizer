namespace Abstract.Realizer.Builder.References;

public class SliceTypeReference(TypeReference subtype) : TypeReference
{
    public readonly TypeReference Subtype = subtype;
    public override string ToString() => $"[]{Subtype}";
}
