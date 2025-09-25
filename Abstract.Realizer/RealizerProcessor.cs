using Abstract.Realizer.Builder;
using Abstract.Realizer.Builder.ProgramMembers;
using Abstract.Realizer.Core.Configuration.LangOutput;
using Abstract.Realizer.Core.Intermediate;
using Abstract.Realizer.Core.Intermediate.Language;
using Abstract.Realizer.Optimization;

namespace Abstract.Realizer;

public class RealizerProcessor
{

    public string? DebugDumpPath { get; set; } = null;
    public bool Verbose { get; set; } = false;

    private ProgramBuilder program;
    private ILanguageOutputConfiguration configuration;
    private bool processRunning = false;
    private int stage = 0;
    
    private Dictionary<FunctionBuilder, IrRoot> functions = [];
    private List<StaticFieldBuilder> fields = [];
    private List<StructureBuilder> structs = [];
    private List<TypeDefinitionBuilder> typedefs = [];

    public void SelectProgram(ProgramBuilder program) => this.program = program;
    public void SelectConfiguration(ILanguageOutputConfiguration config) => configuration = config;

    public void Start()
    {
        if (processRunning) throw new Exception("Already running");
        processRunning = true;
        
        if (DebugDumpPath != null)
            File.WriteAllTextAsync(Path.Combine(DebugDumpPath, "Stage0.txt"), program.ToString());
        
        UnwrapRecursive(program);
    }
    public void Optimize(OptimizationOption optimization)
    {
        stage++;
        if (Verbose) Console.WriteLine($"Realizer: Optimizing ({optimization})...");
        
        switch (optimization)
        {
            case OptimizationOption.PackStructures:
                ObjectBaker.BakeTypeMetadata(configuration, [.. structs], [.. typedefs]);
                break;
            
            default: throw new ArgumentOutOfRangeException(nameof(optimization), optimization, null);
        }
        
        if (DebugDumpPath != null)
            File.WriteAllTextAsync(Path.Combine(DebugDumpPath, $"Stage{stage}.txt"), program.ToString());
    }
    public ProgramBuilder Compile()
    {
        LanguageOutput compileTo = configuration switch
        {
            AlphaOutputConfiguration => LanguageOutput.Alpha,
            BetaOutputConfiguration => LanguageOutput.Beta,
            _ => throw new ArgumentOutOfRangeException(),
        };
        if (Verbose) Console.WriteLine($"Realizer: Compiling to {compileTo}...");

        return program;
    }
    
    
    private void UnwrapRecursive(ProgramBuilder program)
    {
        UnwrapRecursive(program.GetRoot());
    }
    private void UnwrapRecursive(ProgramMemberBuilder member)
    {
        switch (member)
        {
            case NamespaceBuilder @m:
                foreach (var i in m.Namespaces) UnwrapRecursive(i);
                foreach (var i in m.Functions) UnwrapRecursive(i);
                foreach (var i in m.Fields) UnwrapRecursive(i);
                foreach (var i in m.TypeDefinitions) UnwrapRecursive(i);
                foreach (var i in m.Structures) UnwrapRecursive(i);
                break;
            
            case FunctionBuilder @f:
                functions.Add(f, Unwrapper.UnwerapFunction(f));
                break;
            
            case StructureBuilder @s:
                structs.Add(@s);
                foreach (var f in s.Functions) UnwrapRecursive(f);
                break;
            
            case StaticFieldBuilder @f: 
                fields.Add(f);
                break;
            
            case ImportedFunctionBuilder: break;
            
            default: throw new NotImplementedException();
        }
    }
    
    

    private enum LanguageOutput
    {
        Alpha,
        Beta,
        Omega,
    }
    public enum OptimizationOption
    {
        PackStructures
    }
    
}
