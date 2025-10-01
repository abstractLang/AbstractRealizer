using System.Diagnostics;
using Abstract.Realizer.Builder.Language.Beta;
using Abstract.Realizer.Builder.ProgramMembers;
using Abstract.Realizer.Core.Configuration.LangOutput;
using Abstract.Realizer.Core.Intermediate;
using Abstract.Realizer.Core.Intermediate.Language;
using Abstract.Realizer.Core.Intermediate.Types;

namespace Abstract.Realizer.Compiler;

internal static class BetaCompiler
{
    public static void CompileFunction(FunctionBuilder function, BetaOutputConfiguration config)
    {
        var intermediate = function._intermediateRoot!;
        function._intermediateRoot = null;
        
        var builder = new BetaBytecodeBuilder();
        function.BytecodeBuilder = builder;
        CompileRecursive(builder, intermediate, config);
    }

    private static void CompileRecursive(BetaBytecodeBuilder builder, IrNode node, BetaOutputConfiguration config)
    {
        switch (node)
        {
            case IrRoot @root:
                foreach (var n in root.content) CompileRecursive(builder, n, config);
                break;

            case IrMacroDefineLocal @mdl:
                builder.Writer.MacroDefineLocal(@mdl.Type);
                break;
            
            
            case IrAssign @assign:
            {
                CompileRecursive(builder, assign.value, config);
                CompileRecursive_Store(builder, (IrValue)assign.to, config);
            } break;

            case IrNewObj @newobj:
            {
                Console.WriteLine($"TODO: newobj {newobj.Type.ToReadableReference()}");
            } break;

            case IrInteger @integer:
            {
                if (!integer.Size.HasValue) builder.Writer.LdConstIptr(integer.Value);
                else builder.Writer.LdConstI(integer.Size.Value, integer.Value);
            } break;

            case IrLocal @local: builder.Writer.LdLocal(local.Index); break;

            case IrField @field: builder.Writer.LdField(field.Field); break;
            
            case IrAccess @access:
            {
                CompileRecursive(builder, access.Left, config);
                CompileRecursive(builder, access.Right, config);
            } break;

            case IrCall @call:
            {
                foreach (var i in call.Arguments)
                    CompileRecursive(builder, i, config);
                builder.Writer.Call(call.Function);
            } break;
            
            case IrBinaryOp @binop:
            {
                var w = builder.Writer;

                // Writing values
                CompileRecursive(builder, binop.Left, config);
                CompileRecursive(builder, binop.Right, config);
                
                // Writing operator
                switch (binop.Type)
                {
                    case IntegerType @intt:
                    {
                        if ((config.SizedOperations & BetaSizedOperationsOptions.IntegerSigness) != 0 
                            && (config.SizedOperations & BetaSizedOperationsOptions.IntegerSize) != 0)
                            w.TypeInt(intt.Signed, intt.Size);
                        
                        else w.TypeInt(intt.Signed, null);
                    } break;

                    default: throw new UnreachableException();
                }
                switch (binop.Op)
                {
                    case BinaryOperation.Add: w.Add(); break;
                    case BinaryOperation.Sub: w.Sub(); break;
                    case BinaryOperation.Mul: w.Mul(); break;
                    case BinaryOperation.Div: w.Div(); break;
                    
                    case BinaryOperation.Rem:
                    case BinaryOperation.BitAnd:
                    case BinaryOperation.BitOr:
                    case BinaryOperation.BitXor:
                        throw new NotImplementedException();
                        
                    default: throw new ArgumentOutOfRangeException();
                }
                
            } break;
            
            // FIXME
            case IrExtend @ext:
                CompileRecursive(builder, ext.Value, config);
                builder.Writer.Extend();
                break;
            case IrTrunc @tru:
                CompileRecursive(builder, tru.Value, config);
                builder.Writer.Trunc();
                break;
            
            
            case IrRet @ret:
                if (ret.Value != null) CompileRecursive(builder, ret.Value, config);
                builder.Writer.Ret();
                break;
            
            default: throw new UnreachableException();
        }
    }

    private static void CompileRecursive_Store(BetaBytecodeBuilder builder, IrValue node, BetaOutputConfiguration config)
    {
        switch (node)
        {
            case IrLocal @local: builder.Writer.StLocal(local.Index); break;

            case IrAccess @access:
            {
                CompileRecursive(builder, access.Left, config);
                CompileRecursive_Store(builder, access.Right, config);
            } break;

            case IrField @field:
            {
                builder.Writer.StField(field.Field);
            } break;
            
            default: throw new UnreachableException();
        }
    }

    private static void CompileRecursive_Call(BetaBytecodeBuilder builder, IrValue node, BetaOutputConfiguration config)
    {
        switch (node)
        {
            default: throw new UnreachableException();
        }
    }
    
}
