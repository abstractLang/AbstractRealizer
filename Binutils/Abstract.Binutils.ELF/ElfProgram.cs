using System.Text;
using Abstract.Binutils.ELF.ElfBuilder.ProgramNodes.Streams;

namespace Abstract.Binutils.ELF;

public class ELFProgram
{
    private List<ELFDirectory> _dirList = [];
    public ELFDirectory[] Directories => [.. _dirList];


    private List<ElfLump> _lumpList = [];
    private uint _dataLength = 0;
    public ElfLump[] Lumps => [.. _lumpList];

    private List<string> _directoryTypes = [];
    private MemoryStream _identifiers = new();

    internal uint AppendDirectory(ELFDirectory dir)
    {
        _dirList.Add(dir);
        return (uint)_dirList.Count - 1;
    }
    internal uint AppendLump(ElfLump lump)
    {
        _lumpList.Add(lump);
        return (uint)_lumpList.Count - 1;
    }
    internal uint GetOrCreateDirType(string name)
    {
        var pos = _directoryTypes.IndexOf(name);
        if (pos == -1)
        {
            pos = _directoryTypes.Count;
            _directoryTypes.Add(name);
        }
        return (uint)pos;
    }
    internal uint AppendIdentifier(string identifier)
    {
        var pos = _identifiers.Position;
        _identifiers.Write(Encoding.ASCII.GetBytes(identifier));
        return (uint)pos;
    }


    public override string ToString()
    {
        var sb = new StringBuilder();

        var root = _dirList[0];
        WriteDirectory(root, 0, sb);

        return sb.ToString();
    }
    private void WriteDirectory(ELFDirectory dir, int level, StringBuilder sb)
    {
        if (level > 1024) throw new StackOverflowException();

        var tabs = new string(' ', level * 2);

        sb.Append(tabs);
        sb.Append($"({_directoryTypes[(int)dir.identifier]}");
        sb.Append($" (;{_dirList.IndexOf(dir):X};)");

        if (!dir.isLump) {
            foreach (var i in _dirList[(int)dir.pointer .. (int)(dir.pointer + dir.length)])
            {
                sb.AppendLine();
                WriteDirectory(i, level + 1, sb);
            }
        }
        else {
            if (dir.length == uint.MaxValue)
                sb.Append($" * -> ${dir.pointer}");
            else
                sb.Append($" \"{_lumpList[(int)dir.pointer]}\"");
        }

        sb.Append(')');
    }
}

public readonly struct ELFDirectory
{
    public readonly uint identifier;
    public readonly uint pointer;
    public readonly uint length;
    public readonly bool isLump;

    internal ELFDirectory(uint id, uint ptr, uint len, bool il = false)
    {
        identifier = id;
        pointer = ptr;
        length = len;
        isLump = il;
    }
}

public readonly struct ElfLump
{
    public readonly MemoryStream data;
    public readonly uint Length => (uint)data.Length;

    internal ElfLump(ContentStream d)
    {
        data = d.Truncate()._memStream;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        var oldptr = data.Position;
        data.Position = 0;

        while (data.Position < data.Length)
        {
            var val = (char)data.ReadByte();
            if (!char.IsControl(val)) sb.Append(val);
            else {
                data.Position = 0;
                sb.Clear();
                goto fallback;
            }
        }
        goto ret;

        fallback:
        while (data.Position < data.Length)
        {
            var val = (char)data.ReadByte();
            sb.Append($"{val:X2} ");
        }
        sb.Length -= 1;

        ret:
            data.Position = oldptr;
            return sb.ToString();
    }
}
