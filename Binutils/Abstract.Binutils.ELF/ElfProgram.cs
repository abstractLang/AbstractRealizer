using System.Text;
using Abstract.Binutils.ELF.ProgramNodes;
using Directory = Abstract.Binutils.ELF.ProgramNodes.Directory;

namespace Abstract.Binutils.ELF;

public class ElfProgram {

    private Directory _root = new("ROOT");
    private Directory _moduleLump;
    private Directory _externLump;

    public Directory Root => _root;
    public Directory Module => _moduleLump;
    public Directory Dependences => _externLump;


    public ElfProgram()
    {
        _moduleLump = (Directory)_root.Branch("MODULE", NodeTypes.Directory);
        _externLump = (Directory)_root.Branch("EXTERN", NodeTypes.Directory);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("Abstract ELF Program");
        sb.Append(_root.ToString());

        return sb.ToString();
    }
}
