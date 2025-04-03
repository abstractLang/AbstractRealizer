namespace Abstract.Binutils.ELF.ElfBuilder.ProgramNodes;

public abstract class Node : IDisposable
{
    public readonly string name;

    internal Node(string name)
    {
        this.name = name[..Math.Min(name.Length, 8)];
    }

    public abstract void Dispose();
    ~Node() => Dispose();

    public override string ToString() => $"({name})";
    public virtual (string str, int c) ToStringCounting(int c) => ($"({name} (;{c};))", c + 1);
}
