using Abstract.Realizer.Builder.ProgramMembers;

namespace Abstract.Realizer.Builder;

public class ProgramBuilder() : ICloneable
{

    private ModuleBuilder? _root = null;
    public ModuleBuilder? GetRoot() => _root;

    public ModuleBuilder AddModule(string name)
    {
        _root = new ModuleBuilder(name);
        return _root;
    }

    internal void AddModule(ModuleBuilder module)
    {
        _root = module;
    }

    public override string ToString()
    {
        return _root?.ToString() ?? "";
    }

    public object Clone()
    {
        var a = new ProgramBuilder();
        a.AddModule(new ModuleBuilder(GetRoot()));
        return a;
    }

    
}
