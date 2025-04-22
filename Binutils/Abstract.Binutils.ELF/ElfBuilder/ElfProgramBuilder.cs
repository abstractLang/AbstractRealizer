using System.Text;
using Abstract.Binutils.ELF.ElfBuilder.ProgramNodes;
using Abstract.Binutils.ELF.ElfBuilder.ProgramNodes.Streams;
using Directory = Abstract.Binutils.ELF.ElfBuilder.ProgramNodes.Directory;

namespace Abstract.Binutils.ELF.ElfBuilder;

public class ElfProgramBuilder
{

    private Directory _root = new("ROOT");
    private Directory _moduleLump;
    private Directory _externLump;

    public Directory Root => _root;
    public Directory Module => _moduleLump;
    public Directory Dependences => _externLump;


    public ElfProgramBuilder()
    {
        _moduleLump = (Directory)_root.Branch("MODULE", NodeTypes.Directory);
        _externLump = (Directory)_root.Branch("EXTERN", NodeTypes.Directory);
    }


    public ELFProgram Bake()
    {
        var newProgram = new ELFProgram();

        // Here we will order every directory into a
        // linear list, recording the position and length
        // of the future children.

        List<(Node dir, bool isDir, uint cstart, uint length)> linearDirs = [];
        Queue<(Directory dir, int linearIndex)> toIterate = [];

        linearDirs.Add((_root, true, 0, 0));
        toIterate.Enqueue((_root, 0));

        while (toIterate.Count > 0)
        {
            var (current_dir, current_linearIndex) = toIterate.Dequeue();

            var current_linear_data = linearDirs[current_linearIndex];
            current_linear_data.length = (uint)current_dir.Children.Length;
            current_linear_data.cstart = (uint)linearDirs.Count;
            linearDirs[current_linearIndex] = current_linear_data;

            foreach (var child in current_dir.Children)
            {
                if (child is Directory d) toIterate.Enqueue((d, linearDirs.Count));
                linearDirs.Add((child, child is Directory, 0, 0));
            }
        }


        foreach (var i in linearDirs)
        {
            var type = newProgram.GetOrCreateDirType(i.dir.name);

            if (i.isDir) newProgram.AppendDirectory(new ELFDirectory(type, i.cstart, i.length));
            else
            {
                if (i.dir is Pointer p)
                {
                    var pointedIdx = linearDirs.FindIndex(e => e.dir == p.PointsTo);
                    newProgram.AppendDirectory(new ELFDirectory(type, (uint)pointedIdx, unchecked((uint)-1), true));
                }

                else if (i.dir is Content c)
                {
                    var stream = c.Stream;
                    var lump_ptr = newProgram.AppendLump(new ElfLump(stream));
                    newProgram.AppendDirectory(new ELFDirectory(type, lump_ptr, (uint)stream.Length, true));
                }

                else if (i.dir is TextSection t)
                {
                    var stream = new ContentStream();
                    var lump_ptr = newProgram.AppendLump(new ElfLump(stream));
                    newProgram.AppendDirectory(new ELFDirectory(type, lump_ptr, (uint)stream.Length, true));
                }
            }
        }


        return newProgram;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("Abstract ELF Program (building mode)");
        sb.Append(_root.ToString());

        return sb.ToString();
    }
}
