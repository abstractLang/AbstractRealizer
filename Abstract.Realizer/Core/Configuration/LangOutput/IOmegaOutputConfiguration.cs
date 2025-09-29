namespace Abstract.Realizer.Core.Configuration.LangOutput;

public class OmegaOutputConfiguration : ILanguageOutputConfiguration
{
    public bool BakeGenerics { get; init; }
    public bool UnnestMembers { get; init; }
    
    public byte MemoryUnit { get; init; }
    public byte IptrSize { get; init; }
}
