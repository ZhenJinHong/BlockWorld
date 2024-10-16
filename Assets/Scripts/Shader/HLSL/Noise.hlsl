#include "E:\UnityProject\String World\Assets\Scripts\Shader\HLSL\Hash.hlsl"
float Random(float3 co)
{
    return frac(sin(dot(co, float3(12.9898, 78.233, 53.539))) * 43758.5453);
}
float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
{
    float x;
    Hash_Tchou_2_1_float(p, x);
    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
}
        
float Unity_GradientNoise_Deterministic_float(float2 UV, float2 Scale)
{
    float2 p = UV * Scale.xy;
    float2 ip = floor(p);
    float2 fp = frac(p);
    float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
    float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
    float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
    float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
}