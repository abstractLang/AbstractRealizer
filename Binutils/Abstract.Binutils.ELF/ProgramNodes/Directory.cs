namespace Abstract.Binutils.ELF.ProgramNodes;

public class Directory : Node
{
    private List<Node> _nodeChildren = [];
    public Node[] Children => [.. _nodeChildren];

    internal Directory(string name) : base(name) {}

    public Node Branch(string name, NodeTypes type)
    {
        if (type == NodeTypes.Directory)
        {
            var newdic = new Directory(name);
            _nodeChildren.Add(newdic);
            return newdic;
        }
        else
        {
            var newcon = new Content(name);
            _nodeChildren.Add(newcon);
            return newcon;
        }
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        foreach (var i in _nodeChildren) i.Dispose();
    }
}
