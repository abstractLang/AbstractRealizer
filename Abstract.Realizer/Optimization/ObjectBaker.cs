using System.Diagnostics;
using Abstract.Realizer.Builder.ProgramMembers;
using Abstract.Realizer.Builder.References;
using Abstract.Realizer.Core.Configuration.LangOutput;

namespace Abstract.Realizer.Optimization;

public static class ObjectBaker
{
    public static void BakeTypeMetadata(
        ILanguageOutputConfiguration config,
        StructureBuilder[] structs,
        TypeDefinitionBuilder[] typedefs)
    {
        
        BakeTypedefs(config, typedefs);
        BakeStructsFields(config, structs);
        
        PackStructures(structs);
    }

    private static void BakeStructsFields(
        ILanguageOutputConfiguration configuration,
        StructureBuilder[] structs)
    {
        List<StructureBuilder> toiter = structs.ToList();
        Dictionary<StructureBuilder, (uint a, uint s)> baked = [];

        // FIXME cyclic dependencies will result in 
        // infinite loop

        looooop:
        if (toiter.Count == 0) goto end;
        for (var i = 0; i < toiter.Count; i++)
        {
            var cur = toiter[i];
            uint minAlig = 0;
            uint size = 0;
            
            foreach (var j in cur.Fields)
            {
                j.Offset = size;
                switch (j.Type)
                {
                    case IntegerTypeReference @it:
                        var intAlig = (it.Bits ?? configuration.IptrSize).AlignForward(configuration.MemoryUnit);
                        
                        j.Size = it.Bits;
                        j.Alignment = (uint)intAlig;
                        
                        size += it.Bits ?? configuration.IptrSize;
                        minAlig = Math.Max(minAlig, intAlig);
                        break;
                    
                    case NodeTypeReference @nt:
                        switch (nt.TypeReference)
                        {
                            case StructureBuilder @struc:
                                if (!baked.TryGetValue(struc, out var shit)) goto hardbreak;
                                
                                j.Size = struc.Length;
                                j.Alignment = struc.Alignment;
                                
                                minAlig = Math.Max(minAlig, shit.a);
                                size += shit.s;
                                break;
                            
                            case TypeDefinitionBuilder @typedef:
                                break;
                        }
                        break;
                    
                    default: throw new UnreachableException();
                }
            }

            cur.Alignment = minAlig;
            cur.Length = size;
            baked.Add(cur, (minAlig, size));
            toiter.RemoveAt(i);
            
            hardbreak:
            continue;
        }
        goto looooop;
        end:
        
        return;
    }

    private static void BakeTypedefs(
        ILanguageOutputConfiguration config,
        TypeDefinitionBuilder[] typedefs)
    {
        
    }

    private static void PackStructures(StructureBuilder[] structs)
    {
        foreach (var i in structs)
        {
            i.Fields.Sort((a, b)
                => -(int)(a.Alignment!.Value - b.Alignment!.Value));

            uint off = 0;
            foreach (var f in i.Fields)
            {
                var alignedOff = off.AlignForward(f.Alignment!.Value);
                f.Offset = alignedOff;
                off = alignedOff + f.Size!.Value;
            }
        }
    }
}
