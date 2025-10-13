using System.Diagnostics;
using Abstract.Realizer.Builder.Language;
using Abstract.Realizer.Builder.Language.Omega;
using Abstract.Realizer.Builder.ProgramMembers;
using Abstract.Realizer.Core.Configuration.LangOutput;
using Abstract.Realizer.Core.Intermediate;
using Abstract.Realizer.Core.Intermediate.Language;
using Abstract.Realizer.Core.Intermediate.Types;

namespace Abstract.Realizer.Compiler;

internal static class OmegaCompiler
{
    public static OmegaBlockBuilder CompileBlock(IntermediateBlockBuilder intermediateBlock, OmegaOutputConfiguration config)
    {
        var block = new OmegaBlockBuilder(intermediateBlock.Parent, intermediateBlock.Name, intermediateBlock.Index);
        UnwrapNode(block, intermediateBlock.Root, DataMode.Load, config);
        return block;
    }
    
    private static void UnwrapNode(OmegaBlockBuilder builder, IrNode node, DataMode datamode, OmegaOutputConfiguration configuration)
    {
        switch (node)
        {
            case IrRoot root: 
                foreach (var i in root.content) UnwrapNode(builder, i, DataMode.Load, configuration); break;
            
            case IrMacroDefineLocal macroDefineLocal: builder.Writer.MacroDefineLocal(macroDefineLocal.Type); break;
            
            case IrBinaryOp bop:
                UnwrapTypeFlag(builder, bop.Type, configuration);
                switch (bop.Op)
                {
                    case BinaryOperation.Add: builder.Writer.Add(); break;
                    case BinaryOperation.Sub: builder.Writer.Sub(); break;
                    case BinaryOperation.Mul: builder.Writer.Mul(); break;
                    case BinaryOperation.Div: builder.Writer.Div(); break;
                    case BinaryOperation.Rem: builder.Writer.Rem(); break;
                    case BinaryOperation.BitAnd: builder.Writer.And(); break;
                    case BinaryOperation.BitOr: builder.Writer.Or(); break;
                    case BinaryOperation.BitXor: builder.Writer.Xor(); break;
                    
                    default: throw new ArgumentOutOfRangeException();
                }
                UnwrapNode(builder, bop.Left, DataMode.Load, configuration);
                UnwrapNode(builder, bop.Right, DataMode.Load, configuration);
                break;
                
            case IrAssign assign:
                UnwrapNode(builder, (IrNode)assign.to, DataMode.Store, configuration);
                UnwrapNode(builder, assign.value, DataMode.Load, configuration);
                break;
            
            case IrCall call:
                builder.Writer.Call(call.Function);
                foreach (var i in call.Arguments) UnwrapNode(builder, i, DataMode.Load, configuration);
                break;
                
            case IrLocal local:
                switch (datamode)
                {
                    case DataMode.Load: builder.Writer.LdLocal(local.Index); break;
                    case DataMode.Store: builder.Writer.StLocal(local.Index); break;
                    
                    case DataMode.LoadRef: builder.Writer.LdLocalRef(local.Index); break;
                    case DataMode.StoreRef: builder.Writer.StLocalRef(local.Index); break;
                    
                    default: throw new UnreachableException();
                }
                break;
            
            case IrField field:
                switch (datamode)
                {
                    case DataMode.Load when field.Field is StaticFieldBuilder @sfb: builder.Writer.LdField(sfb); break;
                    case DataMode.Load when field.Field is InstanceFieldBuilder @ifb: builder.Writer.LdField(ifb); break;
                    case DataMode.Store when field.Field is StaticFieldBuilder @sfb: builder.Writer.StField(sfb); break;
                    case DataMode.Store when field.Field is InstanceFieldBuilder @ifb: builder.Writer.StField(ifb); break;
                    
                    default: throw new UnreachableException();
                }
                break;
            
            case IrAccess access:
                UnwrapNode(builder, access.Left, DataMode.Load, configuration);
                UnwrapNode(builder, access.Right, datamode, configuration);
                break;
            
            case IrRefOf refof:
                UnwrapNode(builder, refof.Value, datamode switch
                {
                    DataMode.Load => DataMode.LoadRef,
                    DataMode.Store => DataMode.StoreRef,
                    _ => datamode
                }, configuration);
                break;

            
            case IrNewObj newobj: builder.Writer.LdNewObject(newobj.Type); break;
            
            case IrInteger constint: 
                if (constint.Size.HasValue) builder.Writer.LdConstI(constint.Size.Value, constint.Value);
                else builder.Writer.LdConstIptr(unchecked((ulong)(Int128)constint.Value));
                break;
            
            case IrSliceBytes @slice: builder.Writer.LdSlice(slice.Values); break;
            
            case IrExtend ext:
                UnwrapTypeFlag(builder, ext.ToType, configuration);
                builder.Writer.Extend();
                UnwrapNode(builder, ext.Value, DataMode.Load, configuration);
                break;
            
            case IrTrunc trunc:
                UnwrapTypeFlag(builder, trunc.ToType, configuration);
                builder.Writer.Trunc();
                UnwrapNode(builder, trunc.Value, DataMode.Load, configuration);
                break;
            
            case IrConv conv:
                UnwrapTypeFlag(builder, conv.Type, configuration);
                builder.Writer.Conv();
                UnwrapNode(builder, conv.Value, DataMode.Load, configuration);
                break;
            
            case IrBranch @b:
                builder.Writer.Branch(b.To.Index);
                break;
            
            case IrBranchIf @bif:
                builder.Writer.BranchIf(bif.IfTrue.Index, bif.IfFalse.Index);
                UnwrapNode(builder, @bif.Condition, DataMode.Load, configuration);
                break;
                
            case IrRet ret:
                builder.Writer.Ret(ret.Value != null);
                if (ret.Value != null) UnwrapNode(builder, ret.Value, DataMode.Load, configuration);
                break;
            
            default: throw new UnreachableException(); 
        }
    }

    private static void UnwrapTypeFlag(OmegaBlockBuilder builder, RealizerType t, OmegaOutputConfiguration configuration)
    {
        switch (t)
        {
            case IntegerType integerType: builder.Writer.TypeInt(integerType.Signed, integerType.Size ?? configuration.NativeIntegerSize); break;
            default: throw new UnreachableException(t.ToString());
        }   
    }

    private enum DataMode
    {
        Load, Store, 
        LoadRef, StoreRef
    }
}
