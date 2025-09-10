using System.Text;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class NamespaceBuilder(string name): ProgramMemberBuilder(name)
{
    
    private List<NamespaceBuilder> namespaces = [];
    private List<FunctionBuilder> functions = [];
    private List<StructureBuilder> structures = [];
    private List<TypeDefinitionBuilder> typedefs = [];


    public NamespaceBuilder AddNamespace(string ns)
    {
        var newNamespace = new NamespaceBuilder(ns);
        namespaces.Add(newNamespace);
        return newNamespace;
    }

    public FunctionBuilder AddFunction(string fn)
    {
        var newFunction = new FunctionBuilder(fn);
        functions.Add(newFunction);
        return newFunction;
    }

    public StructureBuilder AddStructure(string st)
    {
        var newStructure = new StructureBuilder(st);
        structures.Add(newStructure);
        return newStructure;
    }

    public TypeDefinitionBuilder AddTypedef(string td)
    {
        var newTypedef = new TypeDefinitionBuilder(td);
        typedefs.Add(newTypedef);
        return newTypedef;
    }


    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"(namespace {name}");
        foreach (var i in namespaces) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in functions) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in structures) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in typedefs) sb.AppendLine(i.ToString().TabAllLines());
        sb.Length -= Environment.NewLine.Length;
        sb.Append(')');
        
        return sb.ToString();
    }
}
