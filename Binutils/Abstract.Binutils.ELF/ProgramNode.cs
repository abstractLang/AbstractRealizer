namespace Abstract.Binutils.ELF;

public abstract class Node : IDisposable
{
    public readonly string name;

    internal Node(string name)
    {
        this.name = name[..Math.Min(name.Length, 8)];
    }

    public abstract void Dispose();
    ~Node() => Dispose();
}

public class Directory : Node
{
    private List<Node> _nodeChildren = [];
    public Node[] Children => [.. _nodeChildren];

    internal Directory(string name) : base(name) {}

    public Directory BranchDirectory(string name)
    {
        var newdic = new Directory(name);
        _nodeChildren.Add(newdic);
        return newdic;
    }
    public Content BranchContent(string name)
    {
        var newcon = new Content(name);
        _nodeChildren.Add(newcon);
        return newcon;
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        foreach (var i in _nodeChildren) i.Dispose();
    }
}

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
