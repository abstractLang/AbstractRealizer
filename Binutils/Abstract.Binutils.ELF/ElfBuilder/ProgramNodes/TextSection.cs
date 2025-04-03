namespace Abstract.Binutils.ELF.ElfBuilder.ProgramNodes;

public class TextSection : Node
{

    internal TextSection(string name) : base(name) { }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
