using System.Text;
using Abstract.Binutils.ELF.ProgramNodes;
using Directory = Abstract.Binutils.ELF.ProgramNodes.Directory;

namespace Abstract.Binutils.ELF;

public class ElfProgram {

    private Directory _root = new("ROOT");
    public Directory Root => _root;


    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("Abstract ELF Program");
        sb.Append(_root.ToString());

        return sb.ToString();
    }
}
