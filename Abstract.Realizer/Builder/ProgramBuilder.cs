using Abstract.Realizer.Builder.ProgramMembers;

namespace Abstract.Realizer.Builder;

public class ProgramBuilder()
{

    private ModuleBuilder? _root = null;
    public ModuleBuilder? GetRoot() => _root;
    
    public ModuleBuilder AddModule(string name)
    {
        _root = new ModuleBuilder(name);
        return _root;
    }

    public override string ToString()
    {
        return _root?.ToString() ?? "";
    }
}
