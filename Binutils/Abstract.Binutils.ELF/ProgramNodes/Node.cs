namespace Abstract.Binutils.ELF.ProgramNodes;

public abstract class Node : IDisposable
{
    public readonly string name;

    internal Node(string name)
    {
        this.name = name[..Math.Min(name.Length, 8)];
    }

    public abstract void Dispose();
    ~Node() => Dispose();

    public override string ToString() => $"{name} ({GetType().Name})";
}
