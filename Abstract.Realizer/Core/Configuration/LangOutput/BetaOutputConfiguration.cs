namespace Abstract.Realizer.Core.Configuration.LangOutput;

public class BetaOutputConfiguration : ILanguageOutputConfiguration
{
    public bool BakeGenerics { get; set; }
    public byte MemoryUnit { get; set; }
    
    public BetaExtendableInstructionSet EnabledOpcodes { get; set; } = BetaExtendableInstructionSet.None;
    public BetaExtendableScopes EnabledScopes { get; set; } = BetaExtendableScopes.None;
    
    public BetaSizedOperationsOptions SizedOperations { get; set; } =  BetaSizedOperationsOptions.None;
}

[Flags]
public enum BetaExtendableInstructionSet
{
        None = 0,
        All = int.MaxValue,
            
        Dup = (1 << 0),
        Swap = (1 << 1),
        NewObj = (1 << 2),
}

[Flags]
public enum BetaExtendableScopes
{
        None = 0,
        All = int.MaxValue,
            
        Block = (1 << 0),
        Loop = (1 << 1),
        IfElse = (1 << 2),
        Switch = (1 << 3),
}

[Flags]
public enum BetaSizedOperationsOptions
{
    None = 0,
    All = int.MaxValue,
    
    IntegerSigness = (1 << 0),
    IntegerSize = (1 << 1),
    
    FloatingSize = (1 << 2),
    
    Object = (1 << 3),
}
