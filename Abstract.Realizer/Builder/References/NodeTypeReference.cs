using System.Diagnostics;
using Abstract.Realizer.Builder.ProgramMembers;

namespace Abstract.Realizer.Builder.References;

public class NodeTypeReference : TypeReference
{
    public readonly ProgramMemberBuilder TypeReference;
    public sealed override uint? Alignment
    {
        get => TypeReference switch
            {
                StructureBuilder @struct => @struct.Alignment,
                _ => throw new UnreachableException(),
            };
        init { }
    }

    public NodeTypeReference(TypeDefinitionBuilder typedef) => TypeReference = typedef;

    public NodeTypeReference(StructureBuilder structure) =>TypeReference = structure;

    public override string ToString() => TypeReference.ToReadableReference();
}