using System.Diagnostics;
using Abstract.Realizer.Builder;
using Abstract.Realizer.Builder.ProgramMembers;

namespace Abstract.Realizer.Optimization;

internal static class Unnester
{
    internal static void UnnestProgram(ProgramBuilder builder)
    {
        Dictionary<ModuleBuilder, ModuleContent> moduleMembers = [];
        
        ModuleBuilder[] modules = builder.Modules;
        foreach (var module in modules)
        {
            var mc = new ModuleContent();
            SelectMembersRecursive(module, mc);
            moduleMembers.Add(module, mc);
        }

        foreach (var (module, content) in moduleMembers)
        {
            foreach (var member in content.fields)
            {
                var global = string.Join('.', member.Parent!.GlobalIdentifier);
                member._symbol = global + '.' + member._symbol;
                member.Parent = module;
            }
            module.fields.Clear();
            module.fields = content.fields;
            
            foreach (var member in content.functions)
            {
                var global = string.Join('.', member.Parent!.GlobalIdentifier);
                member._symbol = global + '.' + member._symbol;
                member.Parent = module;
            }
            module.functions.Clear();
            module.functions = content.functions;
            
            foreach (var member in content.structs)
            {
                var global = string.Join('.', member.Parent!.GlobalIdentifier);
                member._symbol = global + '.' + member._symbol;
                member.Parent = module;
            }
            module.structures.Clear();
            module.structures = content.structs;
            
            foreach (var member in content.typedefs)
            {
                var global = string.Join('.', member.Parent!.GlobalIdentifier);
                member._symbol = global + '.' + member._symbol;
                member.Parent = module;
            }
            module.typedefs.Clear();
            module.typedefs = content.typedefs;
            
            module.namespaces.Clear();
        }
    }

    private static void SelectMembersRecursive(ProgramMemberBuilder member, ModuleContent content)
    {
        switch (member)
        {
            case NamespaceBuilder @nmsp:
                foreach (var nmsps in nmsp.Namespaces) SelectMembersRecursive(nmsps, content);
                foreach (var fields in nmsp.Fields) SelectMembersRecursive(fields, content);
                foreach (var strucs in nmsp.Structures) SelectMembersRecursive(strucs, content);
                foreach (var funcs in nmsp.Functions) SelectMembersRecursive(funcs, content);
                foreach (var typedef in nmsp.TypeDefinitions) SelectMembersRecursive(typedef, content);
                break;
            
            case StructureBuilder @struc:
                //foreach (var functions in struc.Functions) SelectMembersRecursive(functions, content);
                content.structs.Add(struc);
                break;
            
            case TypeDefinitionBuilder @typedef:
                content.typedefs.Add(typedef);
                //TODO Typedef functions
                break;
            
            case BaseFunctionBuilder @func: content.functions.Add(func); break;
            case StaticFieldBuilder @field: content.fields.Add(field); break;
            
            default: throw new UnreachableException();
        }
    }

    private struct ModuleContent()
    {
        public List<StaticFieldBuilder> fields = [];
        public List<BaseFunctionBuilder> functions = [];
        public List<StructureBuilder> structs = [];
        public List<TypeDefinitionBuilder> typedefs = [];
    }
}
