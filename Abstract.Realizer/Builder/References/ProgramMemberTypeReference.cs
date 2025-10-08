namespace Abstract.Realizer.Builder.References;

public class ProgramMemberTypeReference : TypeReference
{
    public override uint? Alignment { get => 0; init {} }

    public override string ToString() => "ProgramMember";
}
