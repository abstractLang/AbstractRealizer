using Abstract.Realizer.Builder;
using Abstract.Realizer.Core.Configuration.LangOutput;

namespace Abstract.Realizer;

public class Processor
{
    public ProgramBuilder Process(ProgramBuilder program, ILanguageOutputConfiguration configuration)
    {
        var p = new Processor();
        return p.ProcessInternal(program, configuration);
    }

    private ProgramBuilder ProcessInternal(ProgramBuilder program, ILanguageOutputConfiguration configuration)
    {
        // To be able to understand the code at high level,
        // it must unwrap the instruction into an lisp-like
        // intermediate representation.

        return program;
    }
}
