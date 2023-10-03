#ifndef GET_LOOKUP_UV
#define GET_LOOKUP_UV

#define _lookupUV_1 _lookupUV_1, input

void GetLookupUV_float(UnitySamplerState screenPos, sampler2D _sampler, out float2 lookupUV)
{
    lookupUV = SAMPLE_TEXTURE2D(_sampler, unity_DynamicDirectionality, screenPos).ba;
}

#endif
