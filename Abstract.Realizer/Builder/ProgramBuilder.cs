using Abstract.Realizer.Builder.ProgramMembers;

namespace Abstract.Realizer.Builder;

public class ProgramBuilder()
{

    private ModuleBuilder? _root = null;
    
    internal void AddModule(ModuleBuilder module)
    {
        _root = module;
    }

    
    public ModuleBuilder? GetRoot() => _root;

    public ModuleBuilder AddModule(string name)
    {
        _root = new ModuleBuilder(name);
        return _root;
    }
    
    public override string ToString() => _root?.ToString() ?? "";

    
}
