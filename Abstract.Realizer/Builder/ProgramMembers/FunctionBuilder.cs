using System.Text;
using Abstract.Realizer.Builder.Language;
using Abstract.Realizer.Builder.Language.Omega;
using Abstract.Realizer.Builder.References;
using Abstract.Realizer.Core.Intermediate.Language;

namespace Abstract.Realizer.Builder.ProgramMembers;

public class FunctionBuilder: BaseFunctionBuilder
{
    public string? ExportSymbol = null;

    public bool IsInstance => Parameters.Count > 0 
                           && Parameters[0].name == "self"
                           && (Parameters[0].type is ReferenceTypeReference { Subtype: NodeTypeReference @ntr } 
                           && ntr.TypeReference == Parent);

    public readonly List<BlockBuilder> CodeBlocks = [];

    internal FunctionBuilder(INamespaceOrStructureBuilder parent, string name, bool annonymous)
        : base(parent, name, annonymous) {}
    
    
    public OmegaBlockBuilder CreateOmegaBytecodeBlock(string name)
    {
        var realname = name;
        var i = 1;
        while (CodeBlocks.Any(e => e.Name == realname)) realname = name + i++;

        var block = new OmegaBlockBuilder(this, realname, (uint)CodeBlocks.Count);
        CodeBlocks.Add(block);
        return block;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"(func \"{Symbol}\"");
        foreach (var (name, type) in Parameters) sb.Append($" (param \"{name}\" {type})");
        if (ReturnType != null) sb.Append($" (ret {ReturnType})");

        foreach (var builder in CodeBlocks)
        {
            sb.AppendLine($"\n\t(block \"{builder.Name}\"");
            sb.Append($"{builder.DumpInstructionsToString().TabAllLines().TabAllLines()})");
        }
        if (CodeBlocks.Count == 0) sb.Append("(no_body)");

        sb.Append(')');
        return sb.ToString();
    }
    
}
