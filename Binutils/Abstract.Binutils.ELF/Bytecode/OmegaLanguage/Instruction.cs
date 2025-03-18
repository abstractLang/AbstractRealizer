namespace Abstract.Binutils.ELF.Bytecode.OmegaLanguage;

internal struct Instruction (
    byte group,
    byte op1, byte op2,
    dynamic[] arguments
)
{
    private byte gp = group;
    private byte op1 = op1;
    private byte op2 = op2;

    private dynamic[] arguments = arguments;


    public byte[] Assemble()
    {
        List<byte> result = new();

        if (gp == 1)
        {
            var operand = (G2Operand)op2;
            result.Add((byte)(op1 | op2));

            // TODO serialize arguments
        }

        else if (gp == 2)
        {
            var operation = (G3Operation)op1;
            result.Add((byte)(op1 | op2));

            // TODO serialize arguments
        }

        else if (gp == 3)
        {
            var operation = (G4Operand)op1;
            result.Add(op1);

            // TODO serialize arguments
        }

        else result.Add(op1);

        return [.. result];
    }
    public override string ToString()
    {
        return "instruction (placeholder bruh)";
    }
}
