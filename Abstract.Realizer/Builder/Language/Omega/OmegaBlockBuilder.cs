using System.Numerics;
using System.Text;
using Abstract.Realizer.Builder.ProgramMembers;
using Abstract.Realizer.Builder.References;
using TypeBuilder = Abstract.Realizer.Builder.ProgramMembers.TypeBuilder;

namespace Abstract.Realizer.Builder.Language.Omega;

public class OmegaBlockBuilder: BlockBuilder
{
    private List<IOmegaInstruction> _instructions = [];
    public List<IOmegaInstruction> InstructionsList => _instructions;
    public InstructionWriter Writer => new(this);

    internal OmegaBlockBuilder(FunctionBuilder parent, string name, uint idx) : base(parent, name, idx) {}

    public override string DumpInstructionsToString()
    {
        var sb = new StringBuilder();
        var instQueue = new Queue<IOmegaInstruction>(_instructions);
        
        while (instQueue.Count > 0) sb.Append(WriteInstruction(instQueue, false));
        
        // sb.AppendLine();
        // var newline = true;
        // foreach (var i in _instructions)
        // {
        //     if (newline) sb.Append(";; ");
        //     newline = false;
        //     sb.Append(i);
        //     
        //     if (i is IOmegaFlag) continue;
        //     sb.AppendLine();
        //     newline = true;
        // }
        //
        if (sb.Length > Environment.NewLine.Length) sb.Length -= Environment.NewLine.Length;
        return sb.ToString();
    }

    
    private string WriteInstruction(Queue<IOmegaInstruction> instQueue, bool recursive)
    {
        var sb = new StringBuilder();
        
        var a = instQueue.Peek();
        switch (a)
        {
            case MacroDefineLocal @dl:
                instQueue.Dequeue();
                sb.Append($"$DEFINE_LOCAL {dl.Type}");
                break;
            
            case InstNop:
                instQueue.Dequeue();
                sb.Append("nop");
                break;

            case InstRet @r:
                instQueue.Dequeue();
                sb.Append("ret");
                if (r.value) sb.Append(" " + WriteInstructionValue(instQueue).TabAllLines()[1..]);
                break;
            
            case InstStLocal @stlocal:
            {
                instQueue.Dequeue();
                if (stlocal.index < 0) sb.Append($"(arg ${(-stlocal.index) - 1}) = ");
                else sb.Append($"(local {stlocal.index}) = ");
                sb.Append(WriteInstructionValue(instQueue).TabAllLines().TrimStart('\t'));
            } break;
            case InstStLocalRef @stlocref:
            {
                instQueue.Dequeue();
                if (stlocref.index < 0) sb.Append($"(arg ${(-stlocref.index) - 1})* = ");
                else sb.Append($"(local {stlocref.index})* = ");
                sb.Append(WriteInstructionValue(instQueue).TabAllLines().TrimStart('\t'));
            } break;
            
            case InstStStaticField @stfld:
            {
                instQueue.Dequeue();
                sb.Append($"(field {stfld.StaticField.ToReadableReference()}) = ");
                sb.Append(WriteInstructionValue(instQueue).TabAllLines().TrimStart('\t'));
            } break;
            case InstStField @stfld:
            {
                instQueue.Dequeue();
                sb.Append($"(field {stfld.StaticField.ToReadableReference()}) = ");
                sb.Append(WriteInstructionValue(instQueue).TabAllLines().TrimStart('\t'));
            } break;
            
            case InstBranch @b: instQueue.Dequeue(); sb.Append($"(branch \"{Parent.CodeBlocks[(int)b.To].Name}\")"); break;
            case InstBranchIf @b: instQueue.Dequeue(); sb.Append(
                $"(branch.if {WriteInstructionValue(instQueue)} \"{Parent.CodeBlocks[(int)b.IfTrue].Name}\"" +
                $" \"{Parent.CodeBlocks[(int)b.IfFalse].Name}\")");
                break;

            default:
                sb.Append(WriteInstructionValue(instQueue).TrimStart('\t'));
                break;
        }
        if (!recursive) sb.AppendLine();

        return sb.ToString();
    }
    private string WriteInstructionValue(Queue<IOmegaInstruction> instQueue)
    {
        if (instQueue.Count == 0) return "<eof>";
        var sb = new StringBuilder();
        
        var a = instQueue.Dequeue();
        switch (a)
        {
            case InstInc: sb.Append($"(inc {WriteInstructionValue(instQueue)})"); break;
            case InstDec: sb.Append($"(dec {WriteInstructionValue(instQueue)})"); break;
            
            case InstNot: sb.Append($"NOT\n\t{WriteInstructionValue(instQueue)})"); break;
            case InstAdd: sb.Append($"add" +
                                    $"\n{WriteInstructionValue(instQueue).TabAllLines()}" +
                                    $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            case InstSub: sb.Append($"sub" +
                                    $"\n{WriteInstructionValue(instQueue).TabAllLines()}" +
                                    $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            case InstMul: sb.Append($"mul" +
                                    $"\n{WriteInstructionValue(instQueue).TabAllLines()}" +
                                    $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            case InstAnd: sb.Append($"AND" +
                                    $"\n{WriteInstructionValue(instQueue).TabAllLines()}" +
                                    $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            case InstOr: sb.Append($"OR" +
                                    $"\n{WriteInstructionValue(instQueue).TabAllLines()}" +
                                    $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            case InstXor: sb.Append($"XOR" +
                                    $"\n{WriteInstructionValue(instQueue).TabAllLines()}" +
                                    $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            
            case InstCmpEq: sb.Append($"(cmp" +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()} == " +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            case InstCmpNeq: sb.Append($"(cmp" +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()} != " +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            case InstCmpGr: sb.Append($"(cmp" +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()} > " +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            case InstCmpGe: sb.Append($"(cmp" +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()} >= " +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            case InstCmpLr: sb.Append($"(cmp" +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()} < " +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            case InstCmpLe: sb.Append($"(cmp" +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()} <= " +
                                      $"\n{WriteInstructionValue(instQueue).TabAllLines()})"); break;
            
            
            case InstSigcast @s: sb.Append("(sigcast."
                                           + (s.Signed ? 's' : 'u')
                                           + $" {WriteInstructionValue(instQueue)})"); break;
            case InstTrunc @t: sb.Append($"trunc {WriteInstructionValue(instQueue)})"); break;
            case InstExtend @e: sb.Append($"extend {WriteInstructionValue(instQueue)})"); break;
            case InstConv @c: sb.Append($"conv {WriteInstructionValue(instQueue)}"); break;
            
            case InstLdConstI @ldconsti: sb.Append($"(const {ldconsti.Len} 0x{ldconsti.Value:x})"); break;
            case InstLdConstI1 @ldc: sb.Append("(const 1 " + (ldc.Value ? "true" : "false" + ")")); break;
            case InstLdConstIptr @ldc: sb.Append($"(const ptr 0x{ldc.Value:x})"); break;
            
            case InstLdSlicePtr @sliceptr: sb.Append($"(slice.ptr 0x{sliceptr.Pointer:x} {sliceptr.Length})"); break;
            case InstLdSlice @slice: sb.Append($"(slice [{slice.Content}])"); break;
            case InstLdStringUtf8 @str: sb.Append($"(string \"{str.Value}\""); break;
            
            case InstLdLocal @ldl:
                if (ldl.Local < 0) sb.Append($"(arg {(-ldl.Local)-1})");
                else sb.Append($"(local {ldl.Local})");
                break;
            case InstLdLocalRef @ldr:
                if (ldr.Local < 0) sb.Append($"(arg.ref {(-ldr.Local)-1})");
                else sb.Append($"(local.ref {ldr.Local})");
                break;
            case InstLdStaticField @ldf: sb.Append($"(field {ldf.StaticField.ToReadableReference()})"); break;
            case InstLdField @acf: sb.Append($"(field {acf.StaticField.ToReadableReference()})"); break;
            
            case InstLdTypeRefOf: sb.Append($"(typeof {WriteInstructionValue(instQueue)})"); break;
            
            case InstCall @call:
            {
                sb.Append($"(call {call.function.ToReadableReference()} (");
                foreach (var i in call.function.Parameters)
                    sb.Append("\n" + WriteInstructionValue(instQueue).TabAllLines());
                sb.Append("))");
            } break;
            
            case InstLdNewObject @newobj:
                sb.Append($"newobj({newobj.Type.ToReadableReference()})");
                break;
            
            case FlagTypeInt @tint:
                sb.Append("(" + (tint.Signed ? "s" : "u") + (tint.Size.HasValue ? $"{tint.Size}" : "ptr") + '.');
                sb.Append(WriteInstructionValue(instQueue));
                break;
            case FlagTypeFloat @tflo:
                sb.Append($"f{tflo.Size}.");
                sb.Append(WriteInstructionValue(instQueue));
                break;
            
            default: throw new NotImplementedException();
        }

        if (instQueue.Count > 0 && instQueue.Peek() 
            is InstLdField
            or InstStField)
        {
            sb.Append($"->{WriteInstruction(instQueue, true)}");
        }

        return sb.ToString();
    }
    

    public struct InstructionWriter
    {
        private OmegaBlockBuilder _parentBuilder;
        internal InstructionWriter(OmegaBlockBuilder builder) => _parentBuilder = builder;

        private InstructionWriter AddAndReturn(IOmegaInstruction value)
        {
            if (value is IOmegaRequiresTypePrefix && _parentBuilder._instructions[^1] is not IOmegaTypePrefix)
                throw new Exception($"Instruction '{value}' expects type prefix");
                
            _parentBuilder._instructions.Add(value);
            return this;
        }

        public InstructionWriter Nop() => AddAndReturn(new InstNop());
        public InstructionWriter Invalid() => AddAndReturn(new InstInvalid());
        public InstructionWriter Call(BaseFunctionBuilder r) => AddAndReturn(new InstCall(r));
        public InstructionWriter CallVirt() => AddAndReturn(new InstCallvirt());
        public InstructionWriter Ret(bool value) => AddAndReturn(new InstRet(value));
        
        public InstructionWriter Inc() => AddAndReturn(new InstDec());
        public InstructionWriter Dec() => AddAndReturn(new InstDec());
        public InstructionWriter Add() => AddAndReturn(new InstAdd());
        public InstructionWriter Sub() => AddAndReturn(new InstSub());
        public InstructionWriter Mul() => AddAndReturn(new InstMul());
        public InstructionWriter Div() => AddAndReturn(new InstDiv());
        public InstructionWriter Rem() => AddAndReturn(new InstRem());
        public InstructionWriter Neg() => AddAndReturn(new InstNeg());
        public InstructionWriter Not() => AddAndReturn(new InstNot());
        public InstructionWriter And() => AddAndReturn(new InstAnd());
        public InstructionWriter Or() => AddAndReturn(new InstOr());
        public InstructionWriter Xor() => AddAndReturn(new InstXor());
        public InstructionWriter Shr() => AddAndReturn(new InstShr());
        public InstructionWriter Shl() => AddAndReturn(new InstShl());
        public InstructionWriter Ror() => AddAndReturn(new InstRor());
        public InstructionWriter Rol() => AddAndReturn(new InstRol());
        
        public InstructionWriter Block(string label) => AddAndReturn(new InstBlock(label));
        public InstructionWriter Loop(string label) => AddAndReturn(new InstLoop(label));
        public InstructionWriter Branch(uint to) => AddAndReturn(new InstBranch(to));
        public InstructionWriter BranchIf(uint iftrue, uint iffalse) => AddAndReturn(new InstBranchIf(iftrue, iffalse));
        
        public InstructionWriter LdConstI1(bool value) => AddAndReturn(new InstLdConstI1(value));
        public InstructionWriter LdConstIptr(ulong value) => AddAndReturn(new InstLdConstIptr(value));
        public InstructionWriter LdConstI(byte size, BigInteger value) => AddAndReturn(new InstLdConstI(size, value));
        public InstructionWriter LdConstNull() => AddAndReturn(new InstLdConstNull());
        
        public InstructionWriter LdNewSlice() => AddAndReturn(new InstLdNewSlice());
        public InstructionWriter LdSlicePtr(uint ptr, uint len) => AddAndReturn(new InstLdSlicePtr(ptr, len));
        public InstructionWriter LdSlice(byte[] data) => AddAndReturn(new InstLdSlice(data));
        public InstructionWriter LdStringUtf8(string val) => AddAndReturn(new InstLdStringUtf8(val));
        public InstructionWriter LdNewObject(StructureBuilder r) => AddAndReturn(new InstLdNewObject(r));
        
        public InstructionWriter LdLocal(short index) => AddAndReturn(new InstLdLocal(index));
        public InstructionWriter LdLocalRef(short index) => AddAndReturn(new InstLdLocalRef(index));
        
        public InstructionWriter LdField(StaticFieldBuilder r) => AddAndReturn(new InstLdStaticField(r));
        public InstructionWriter LdField(InstanceFieldBuilder r) => AddAndReturn(new InstLdField(r));
        public InstructionWriter LdFieldRef() => AddAndReturn(new InstLdFieldRef());
        //public InstructionWriter LdFieldRef() => AddAndReturn(new InstLdStaticField());
        public InstructionWriter LdIndex() => AddAndReturn(new InstLdIndex());
        public InstructionWriter LdFuncRef(FunctionBuilder funcref) => AddAndReturn(new InstLdFuncRef(funcref));
        public InstructionWriter LdTypeRef(TypeBuilder typeref) => AddAndReturn(new InstLdTypeRef(typeref));
        public InstructionWriter LdTypeRefOf() => AddAndReturn(new InstLdTypeRefOf());
        public InstructionWriter LdMeta(OmegaMetadataKind kind) => AddAndReturn(new InstLdMeta(kind));
        
        public InstructionWriter StLocal(short index) => AddAndReturn(new InstStLocal(index));
        public InstructionWriter StLocalRef(short index) => AddAndReturn(new InstStLocalRef(index));
        public InstructionWriter StField(StaticFieldBuilder r) => AddAndReturn(new InstStStaticField(r));
        public InstructionWriter StField(InstanceFieldBuilder r) => AddAndReturn(new InstStField(r));
        public InstructionWriter StIndex() => AddAndReturn(new InstStIndex());
        
        public InstructionWriter Conv() => AddAndReturn(new InstConv());
        public InstructionWriter Extend() => AddAndReturn(new InstExtend());
        public InstructionWriter Trunc() => AddAndReturn(new InstTrunc());
        public InstructionWriter Sigcast(bool signess) => AddAndReturn(new InstSigcast(signess));
        public InstructionWriter Bitcast() => AddAndReturn(new InstBitcast());

        public InstructionWriter CmpEq() => AddAndReturn(new InstCmpEq());
        public InstructionWriter CmpNeq() => AddAndReturn(new InstCmpNeq());
        public InstructionWriter CmpGr() => AddAndReturn(new InstCmpGr());
        public InstructionWriter CmpLr() => AddAndReturn(new InstCmpLr());
        public InstructionWriter CmpGe() => AddAndReturn(new InstCmpGe());
        public InstructionWriter CmpLe() => AddAndReturn(new InstCmpLe());
        
        public InstructionWriter MemCopy() => AddAndReturn(new InstMemCopy());
        public InstructionWriter MemFill() => AddAndReturn(new InstMemFill());
        public InstructionWriter MemEq() => AddAndReturn(new InstMemEq());
        
        public InstructionWriter AllowOvf() => AddAndReturn(new FlagAllowOvf());
        public InstructionWriter AllowNil() => AddAndReturn(new FlagAllowNil());
        
        public InstructionWriter TypeInt(bool signed, byte? size) => AddAndReturn(new FlagTypeInt(signed, size));
        public InstructionWriter TypeFloat(byte size) => AddAndReturn(new FlagTypeFloat(size));
        public InstructionWriter TypeObj() => AddAndReturn(new FlagTypeObject());
        
        public InstructionWriter MacroDefineLocal(TypeReference typer) => AddAndReturn(new MacroDefineLocal(typer));
    }
}
