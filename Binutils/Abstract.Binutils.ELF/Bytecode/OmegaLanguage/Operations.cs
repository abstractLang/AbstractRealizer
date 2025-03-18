namespace Abstract.Binutils.ELF.Bytecode.OmegaLanguage;

public enum G1Operation : byte
{
    Nop = 0,
    Invalid = 1,
    Pop = 2,
    Dup = 3,
    Swap = 4,
    Add = 5,
    Sub = 6,
    Mul = 7,
    Div = 8,
    Rem = 9,
    Neg = 10,
    And = 11,
    Or = 12,
    Not = 13,
    Nand = 14,
    Nor = 15,
    Xor = 16,
    Xnor = 17,
    Shr = 18,
    Shl = 19,
}

public enum G2Operation : byte
{
    Ld = 0b_010_00000,
    St = 0b_011_00000,
}
public enum G2Operand : byte
{
    Const_null = 0,
    Const_i1_false = 2,
    Const_i1_true = 3,
    Const_i8 = 4,
    Const_i16 = 5,
    Const_i32 = 6,
    Const_i64 = 7,
    Const_i128 = 8,
    Const_iptr = 9,
    Const_in = 10,
    Const_str = 11,

    Field = 12,
    Type = 13,
    Funcref = 14,
    Enumref = 15,
    Faultref = 16,
    Index = 17,

    Arg_0 = 18,
    Arg_1 = 19,
    Arg_2 = 20,
    Arg_3 = 21,
    Local_b = 22,
    Local_w = 23,

    Length = 24,
}

public enum G3Operation : byte
{
    Call_static = 0x80,
    Call_virt = 0x81,
    Tail_call = 0x82,

    Ret = 0x83,
    Throw = 0x84,
    Break = 0x85,

    Jmp = 0x90,
    Check = 0xA0,

    Conv_i1 = 0xB1,
    Conv_i8 = 0xB2,
    Conv_i16 = 0xB3,
    Conv_i32 = 0xB4,
    Conv_i64 = 0xB5,
    Conv_i128 = 0xB6,
    Conv_iptr = 0xB7,
    Conv_in = 0xB8,
}
public enum G3Operand : byte
{
    zero = 1,
    nzero = 2,
    eq = 3,
    neq = 4,
    gr = 5,
    gre = 6,
    ls = 7,
    lse = 8
}

public enum G4Operand : byte
{
    src_offset_global = 0xC0,
    src_offset_rel_b = 0xC1,
    src_offset_rel_w = 0xC2,

    flag_check_overflow = 0xE0,
    flag_check_null = 0xE2
}
