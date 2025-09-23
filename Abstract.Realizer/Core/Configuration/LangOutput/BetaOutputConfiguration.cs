namespace Abstract.Realizer.Core.Configuration.LangOutput;

public class BetaOutputConfiguration : ILanguageOutputConfiguration
{

    public BetaExtendableInstructionSet EnabledOpcodes { get; set; } = BetaExtendableInstructionSet.All;
    public BetaExtendableScopes EnabledScopes { get; set; } = BetaExtendableScopes.All;
    
    public bool SizedOperations { get; set; } = true;
    
    

    
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
