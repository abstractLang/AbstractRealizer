using System.Text;
using Abstract.Binutils.ELF.ElfBuilder.ProgramNodes.Streams;

namespace Abstract.Binutils.ELF.ElfBuilder.ProgramNodes;

public class Pointer : Node
{
    private Node? _pointsTo = null;
    public Node? PointsTo { get => _pointsTo; set => _pointsTo = value; }


    internal Pointer(string name) : base(name)
    {

    }


    public override void Dispose()
    {
        GC.SuppressFinalize(this);
    }
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"({name} (* -> {_pointsTo?.name ?? "null"}))");

        return sb.ToString();
    }
}
