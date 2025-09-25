using System.Diagnostics;
using Abstract.Realizer.Builder.Language.Omega;
using Abstract.Realizer.Builder.ProgramMembers;
using Abstract.Realizer.Core.Intermediate;
using Abstract.Realizer.Core.Intermediate.Language;

namespace Abstract.Realizer;

internal static class Unwrapper
{
    // To be able to understand the code at high level,
    // it must unwrap the instruction into an lisp-like
    // intermediate representation.
    
    public static IrRoot UnwerapFunction(FunctionBuilder function)
    {
        var root = new IrRoot();
        Queue<IOmegaInstruction> queue = function.BytecodeBuilder switch
        {
            OmegaBytecodeBuilder @omega => new(omega.InstructionsList),
            _ => throw new ArgumentException("function do not has a valid input bytecode!")
        };

        while (queue.Count > 0)
        {
            root.content.Add(UnwrapInstruction(queue));
            
        }
            
        return root;
    }

    private static IrNode UnwrapInstruction(Queue<IOmegaInstruction> instructions)
    {
        var a = instructions.Peek();
        switch (a)
        {
            case InstStLocal stLocal:
                instructions.Dequeue();
                return new IrAssign(new IrLocal(stLocal.index), UnwrapValue(instructions));
            
            case InstStField stField:
                instructions.Dequeue();
                return new IrAssign(new IrField(stField.StaticField), UnwrapValue(instructions));
            
            case InstRet @r:
                instructions.Dequeue();
                return new IrRet(r.value ? UnwrapValue(instructions) : null);

            default:
            {
                IrNode r = UnwrapValue(instructions);

                while (instructions.Count > 0 && instructions.Peek() is InstStField)
                {
                    var b = (IrAssign)UnwrapInstruction(instructions);
                    var c = new IrAccess((IrValue)r, (IrValue)b.to);
                    r = new IrAssign(c, b.value);
                }

                return r;
            }
        }
    }

    private static IrValue UnwrapValue(Queue<IOmegaInstruction> instructions)
    {
        var a = instructions.Dequeue();
        IrValue r = a switch
        {
            InstLdLocal @ldlocal => new IrLocal(ldlocal.local),
            InstLdNewObject @newobj => new IrNewObj(newobj.type),
            
            InstLdField @ldField => new IrField(ldField.StaticField),
            InstStField @stField => new IrField(stField.StaticField),
            
            InstLdConstI1 @ldc1 => new IrInteger(1, ldc1.value ? 1 : 0),
            InstLdConstI @ldci => new IrInteger(ldci.len, ldci.value),
            InstLdConstIptr @ldcp => new IrInteger(null, ldcp.value),
            
            InstCall @cal => new IrCall(cal.function, UnwrapValues(instructions, cal.function.Parameters.Length)),
            
            InstAdd => new IrBinaryOp(BinaryOperation.add,
                    UnwrapValue(instructions),
                    UnwrapValue(instructions)),
                
            InstSub => new IrBinaryOp(BinaryOperation.sub,
                    UnwrapValue(instructions),
                    UnwrapValue(instructions)),
            
            InstMul @m => new IrBinaryOp(m.signed ? BinaryOperation.mul_s : BinaryOperation.mul_u,
                    UnwrapValue(instructions),
                    UnwrapValue(instructions)),
            
            InstExtend @e => new IrExtend(e.len, UnwrapValue(instructions)),
            InstTrunc @t => new IrTrunc(t.len, UnwrapValue(instructions)),

            _ => throw new UnreachableException(),
        };
                
        while (instructions.Count > 0 && instructions.Peek() is InstLdField)
        {
            var b = UnwrapValue(instructions);
            r = new IrAccess(r, b);
            
        }

        return r;
    }

    private static IrValue[] UnwrapValues(Queue<IOmegaInstruction> instructions, int count)
    {
        List<IrValue> values = [];
        for (var c = 0; c < count; c++) values.Add(UnwrapValue(instructions));
        return [..values];
    }
}
