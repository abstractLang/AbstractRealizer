using Abstract.Realizer.Builder.ProgramMembers;

namespace Abstract.Realizer.Builder;

public class ModuleBuilder(string moduleName)
{

    private NamespaceBuilder _root = new(moduleName);

    public NamespaceBuilder GetRoot() => _root;
}
