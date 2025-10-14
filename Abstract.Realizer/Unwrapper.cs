using System.Diagnostics;
using System.Text;
using Abstract.Realizer.Builder.Language;
using Abstract.Realizer.Builder.Language.Omega;
using Abstract.Realizer.Builder.ProgramMembers;
using Abstract.Realizer.Builder.References;
using Abstract.Realizer.Core.Intermediate;
using Abstract.Realizer.Core.Intermediate.Language;
using Abstract.Realizer.Core.Intermediate.Types;

namespace Abstract.Realizer;

internal static class Unwrapper
{
    // To be able to understand the code at high level,
    // it must unwrap the instruction into an lisp-like
    // intermediate representation.
    
    public static void UnwerapFunction(FunctionBuilder function)
    {
        List<IntermediateBlockBuilder> newblocks = [];
        newblocks.AddRange(function.CodeBlocks.Select(builder
            => new IntermediateBlockBuilder(function, builder.Name, builder.Index)));

        foreach (var (i, builder) in function.CodeBlocks.ToArray().Index())
        {
            var irblock = newblocks[i].Root;
        
            var queue = builder switch
            {
                OmegaBlockBuilder @omega => new Queue<IOmegaInstruction>(omega.InstructionsList),
                _ => throw new ArgumentException("function do not has a valid input bytecode!")
            };

            while (queue.Count > 0) irblock.content.Add(UnwrapInstruction(queue, newblocks));
            function.CodeBlocks[i] = newblocks[i];
        }
    }

    private static IrNode UnwrapInstruction(Queue<IOmegaInstruction> instructions, List<IntermediateBlockBuilder> newblocks)
    {
        var a = instructions.Peek();
        switch (a)
        {
            case MacroDefineLocal @mdl:
                instructions.Dequeue();
                return new IrMacroDefineLocal(mdl.Type);
            
            case InstStLocal stLocal:
                instructions.Dequeue();
                return new IrAssign(new IrLocal(stLocal.index), UnwrapValue(instructions, newblocks));
            
            case InstStField stField:
                instructions.Dequeue();
                return new IrAssign(new IrField(stField.StaticField), UnwrapValue(instructions, newblocks));
            
            case InstRet @r:
                instructions.Dequeue();
                return new IrRet(r.value ? UnwrapValue(instructions, newblocks) : null);
                
            case InstBranch @b:
                instructions.Dequeue();
                return new IrBranch(newblocks[(int)b.To]);
            
            case InstBranchIf @b:
                instructions.Dequeue();
                return new IrBranchIf(UnwrapValue(instructions, newblocks),
                    newblocks[(int)b.IfTrue], newblocks[(int)b.IfFalse]);
            
            default:
            {
                IrNode r = UnwrapValue(instructions, newblocks);

                while (instructions.Count > 0 && instructions.Peek() is InstStField)
                {
                    var b = (IrAssign)UnwrapInstruction(instructions, newblocks);
                    var c = new IrAccess((IrValue)r, (IrValue)b.to);
                    r = new IrAssign(c, b.value);
                }

                return r;
            }
        }
    }

    private static IrValue UnwrapValue(Queue<IOmegaInstruction> instructions, List<IntermediateBlockBuilder> newblocks)
    {
        var a = instructions.Dequeue();
        var r = a switch
        {
            IOmegaRequiresTypePrefix => throw new Exception($"instruction \"{a}\" expects type prefix"),
            
            InstLdLocal @ldlocal => new IrLocal(ldlocal.Local),
            InstLdNewObject @newobj => new IrNewObj(newobj.Type),
            
            InstLdLocalRef @ldlocalref => new IrRefOf(new IrLocal(ldlocalref.Local)),
            
            InstLdTypeRefOf @ldtyperefof => new IrTypeOf(UnwrapValue(instructions, newblocks)),
            
            InstLdField @ldField => new IrField(ldField.StaticField),
            InstStField @stField => new IrField(stField.StaticField),
            
            InstLdConstI1 @ldc1 => new IrInteger(1, ldc1.Value ? 1 : 0),
            InstLdConstI @ldci => new IrInteger(ldci.Len, ldci.Value),
            InstLdConstIptr @ldcp => new IrInteger(null, ldcp.Value),
            
            InstLdStringUtf8 @str => new IrSliceBytes(Encoding.UTF8.GetBytes(str.Value)),
            
            InstCall @cal => new IrCall(cal.function, UnwrapValues(instructions, cal.function.Parameters.Count, newblocks)),
            
            IOmegaTypePrefix @tprefix => UnwrapValueTyped(tprefix, instructions, newblocks),
            
            _ => throw new UnreachableException(),
        };
                
        while (instructions.Count > 0 && instructions.Peek() is InstLdField)
        {
            var b = UnwrapValue(instructions, newblocks);
            r = new IrAccess(r, b);
            
        }

        return r;
    }

    private static IrValue UnwrapValueTyped(IOmegaTypePrefix type, Queue<IOmegaInstruction> instructions, List<IntermediateBlockBuilder> newblocks)
    {
        RealizerType typeref = type switch
        {
            FlagTypeInt @typei => new IntegerType(typei.Signed, typei.Size),
            _ => throw new UnreachableException(),
        };
        
        var a = instructions.Dequeue();
        return a switch
        {
            InstAdd => new IrBinaryOp(typeref, BinaryOperation.Add,
                UnwrapValue(instructions, newblocks), UnwrapValue(instructions, newblocks)),

            InstSub => new IrBinaryOp(typeref, BinaryOperation.Sub,
                UnwrapValue(instructions, newblocks), UnwrapValue(instructions, newblocks)),

            InstMul => new IrBinaryOp(typeref, BinaryOperation.Mul,
                UnwrapValue(instructions, newblocks), UnwrapValue(instructions, newblocks)),

            
            InstAnd => new IrBinaryOp(typeref, BinaryOperation.BitAnd,
                UnwrapValue(instructions, newblocks), UnwrapValue(instructions, newblocks)),
            
            InstOr => new IrBinaryOp(typeref, BinaryOperation.BitOr,
                UnwrapValue(instructions, newblocks), UnwrapValue(instructions, newblocks)),
            
            InstXor => new IrBinaryOp(typeref, BinaryOperation.BitXor,
                UnwrapValue(instructions, newblocks), UnwrapValue(instructions, newblocks)),
            
            
            InstCmpEq => new IrCmp(((IntegerType)typeref).Signed, CompareOperation.Equals,
                UnwrapValue(instructions, newblocks), UnwrapValue(instructions, newblocks)),
            InstCmpGr => new IrCmp(((IntegerType)typeref).Signed, CompareOperation.Greater,
                UnwrapValue(instructions, newblocks), UnwrapValue(instructions, newblocks)),
            InstCmpGe => new IrCmp(((IntegerType)typeref).Signed, CompareOperation.GreatherEquals,
                UnwrapValue(instructions, newblocks), UnwrapValue(instructions, newblocks)),
            InstCmpLr => new IrCmp(((IntegerType)typeref).Signed, CompareOperation.Lesser,
                UnwrapValue(instructions, newblocks), UnwrapValue(instructions, newblocks)),
            InstCmpLe => new IrCmp(((IntegerType)typeref).Signed, CompareOperation.LesserEquals,
                UnwrapValue(instructions, newblocks), UnwrapValue(instructions, newblocks)),
            
            
            InstConv => new IrConv(typeref, UnwrapValue(instructions, newblocks)),
            InstExtend => new IrExtend((IntegerType)typeref, UnwrapValue(instructions, newblocks)),
            InstTrunc => new IrTrunc((IntegerType)typeref, UnwrapValue(instructions, newblocks)),
            
            _ => throw new Exception($"Instruction \"{a}\" does not allows type prefix"),
        };
    }

    private static IrValue[] UnwrapValues(Queue<IOmegaInstruction> instructions, int count, List<IntermediateBlockBuilder> newblocks)
    {
        List<IrValue> values = [];
        for (var c = 0; c < count; c++) values.Add(UnwrapValue(instructions, newblocks));
        return [..values];
    }
}
