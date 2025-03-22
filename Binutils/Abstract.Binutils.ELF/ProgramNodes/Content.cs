using System.Text;

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
    public override string ToString()
    {
        var sb = new StringBuilder();

        var oldcursor = _content.Position;
        _content.Position = 0;

        sb.Append($"({name} \""); 
        while (_content.Position < _content.Length) {
            var b = _content.ReadByte();
            if (char.IsAsciiLetter((char)b))
                sb.Append($"{(char)b} ");
            else
                sb.Append($"{b:X2} ");
        }
        sb.Length -= 1;
        sb.Append("\")");

        _content.Position = oldcursor;

        return sb.ToString();
    }
}
