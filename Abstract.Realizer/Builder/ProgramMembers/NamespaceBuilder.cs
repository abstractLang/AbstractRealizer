using System.Text;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class NamespaceBuilder: ProgramMemberBuilder, INamespaceOrStructureBuilder
{
    
    protected List<NamespaceBuilder> namespaces = [];
    protected List<FieldBuilder> fields = [];
    protected List<BaseFunctionBuilder> functions = [];
    protected List<StructureBuilder> structures = [];
    protected List<TypeDefinitionBuilder> typedefs = [];


    internal NamespaceBuilder(INamespaceOrStructureBuilder parent, string name) : base(null!, name) { }
    internal NamespaceBuilder(INamespaceOrStructureBuilder parent, NamespaceBuilder tocopy) : this(parent, tocopy.Name)
    {
        foreach (var i in tocopy.namespaces) namespaces.Add(new NamespaceBuilder(this, i));
        foreach (var i in tocopy.structures) structures.Add(new StructureBuilder(this, i)); 
        foreach (var i in tocopy.typedefs) typedefs.Add(new(this, i));
        
        foreach (var i in tocopy.functions)
        {
            switch (i)
            {
                case FunctionBuilder f: functions.Add(new FunctionBuilder(this, f)); break;
                case ImportedFunctionBuilder f: functions.Add(new ImportedFunctionBuilder(this, f)); break;
            }
        } 
    }
    
    
    public NamespaceBuilder AddNamespace(string ns)
    {
        var newNamespace = new NamespaceBuilder(this, ns);
        namespaces.Add(newNamespace);
        return newNamespace;
    }
    public FunctionBuilder AddFunction(string fn)
    {
        var newFunction = new FunctionBuilder(this, fn);
        functions.Add(newFunction);
        return newFunction;
    }
    public ImportedFunctionBuilder AddExternImportedFunction(string fn)
    {
        var newFunction = new ImportedFunctionBuilder(this, fn);
        functions.Add(newFunction);
        return newFunction;
    }
    public StructureBuilder AddStructure(string st)
    {
        var newStructure = new StructureBuilder(this, st);
        structures.Add(newStructure);
        return newStructure;
    }
    public TypeDefinitionBuilder AddTypedef(string td)
    {
        var newTypedef = new TypeDefinitionBuilder(this, td);
        typedefs.Add(newTypedef);
        return newTypedef;
    }
    public FieldBuilder AddStaticField(string fn)
    {
        var newField = new FieldBuilder(this, fn);
        fields.Add(newField);
        return newField;
    }
    
    
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"(namespace \"{Name}\"");
        foreach (var i in namespaces) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in fields) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in functions) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in structures) sb.AppendLine(i.ToString().TabAllLines());
        foreach (var i in typedefs) sb.AppendLine(i.ToString().TabAllLines());
        sb.Length -= Environment.NewLine.Length;
        sb.Append(')');
        
        return sb.ToString();
    }
    
    public override string ToReadableReference() => '"' + string.Join('.', GlobalIdentifier) + '"';
    
}
