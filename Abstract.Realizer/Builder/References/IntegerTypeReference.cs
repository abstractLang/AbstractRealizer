namespace Abstract.Realizer.Builder.References;

public class IntegerTypeReference(bool signed, byte? bits) : TypeReference
{
    public readonly bool Signed = signed;
    public readonly byte? Bits = bits;

    public override string ToString() => (Signed ? "i" : "u") + (Bits.HasValue ? $"{Bits.Value}" : "ptr");
}