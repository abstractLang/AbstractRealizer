using System.Text;
using Abstract.Binutils.ELF.ElfBuilder.ProgramNodes;

namespace Abstract.Binutils.ELF.ElfBuilder.ProgramNodes;

public class Directory : Node
{
    private List<Node> _nodeChildren = [];
    public Node[] Children => [.. _nodeChildren];

    internal Directory(string name) : base(name) { }

    public Node Branch(string name, NodeTypes type)
    {
        if (type == NodeTypes.Directory)
        {
            var newdic = new Directory(name);
            _nodeChildren.Add(newdic);
            return newdic;
        }
        else if (type == NodeTypes.Content)
        {
            var newcon = new Content(name);
            _nodeChildren.Add(newcon);
            return newcon;
        }
        else if (type == NodeTypes.TextSection)
        {
            var newtxt = new TextSection(name);
            _nodeChildren.Add(newtxt);
            return newtxt;
        }
        else if (type == NodeTypes.Pointer)
        {
            var newptr = new Pointer(name);
            _nodeChildren.Add(newptr);
            return newptr;
        }
        else throw new NotImplementedException();
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        foreach (var i in _nodeChildren) i.Dispose();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"({name}");
        foreach (var i in _nodeChildren)
        {
            var lines = i.ToString().Split(Environment.NewLine);
            foreach (var j in lines) sb.AppendLine($"  {j}");
        }
        if (_nodeChildren.Count == 0) sb.Length -= Environment.NewLine.Length;
        sb.Length -= Environment.NewLine.Length;
        sb.Append(')');

        return sb.ToString();
    }
}
