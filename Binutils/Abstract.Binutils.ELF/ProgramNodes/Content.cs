namespace Abstract.Binutils.ELF.ProgramNodes;

public class Content : Node
{
    private MemoryStream _content = new();
    public Stream Stream => _content;

    internal Content(string name) : base(name) {}

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        _content.Dispose();
    }
}
