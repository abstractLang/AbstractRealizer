using Abstract.Binutils.ELF.ProgramNodes;
using Directory = Abstract.Binutils.ELF.ProgramNodes.Directory;

namespace Abstract.Binutils.ELF;

public class ElfProgram {

    private Directory _root = new("ROOT");
    public Directory Root => _root;

}
