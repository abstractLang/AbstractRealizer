using ContentNode = Abstract.Binutils.ELF.ProgramNodes.Content;

namespace Abstract.Binutils.ELF.Bytecode;

public static class OmegaLanguage
{

    public static ContentNode Omega_Nop(this ContentNode content)
    {
        content.Stream.WriteByte(0x00);
        return content;
    }

}
