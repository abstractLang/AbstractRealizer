using System.Text;
using Abstract.Binutils.ELF.ElfBuilder.ProgramNodes.Streams;

namespace Abstract.Binutils.ELF.ElfBuilder.ProgramNodes;

public class Content : Node
{
    private ContentStream _content = new();
    public ContentStream Stream => _content;

    internal Content(string name) : base(name) { }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        _content.Dispose();
    }
    public override string ToString()
    {
        var sb = new StringBuilder();

        var oldcursor = _content.Position;
        _content.Position = 0;

        sb.Append($"({name} \"{_content}\")");

        _content.Position = oldcursor;

        return sb.ToString();
    }
    public override (string str, int c) ToStringCounting(int c)
    {
        var sb = new StringBuilder();

        var oldcursor = _content.Position;
        _content.Position = 0;

        sb.Append($"({name} \"{_content}\")");

        _content.Position = oldcursor;

        return (sb.ToString(), c + 1);
    }
}
