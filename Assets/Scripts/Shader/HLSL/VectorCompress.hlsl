// byte 的最大值是255
float3 decodeFloatToByte3(float x)//original x + (y << 8) + (z << 16)
{
    return floor(frac(x / float3(256.0, 65536.0, 16777216.0)) * 256.0);
}
float2 decodeFloatToByteUshort(float x)//original x + (y << 8) byte在低8位; ushort 移动8位 放更高位
{
    return floor(frac(x / float2(256.0, 16777216.0)) * float2(256.0, 65536.0));
}
float4 decodeFloatTo31X4(float x)//original x + (y << 5) + (z << 10) + (w << 15)
{
    return floor(frac(x / float4(32.0, 1024.0, 32768.0, 1048576.0)) * 32.0);
}
float2 decodeAsUIntToUshort2(float x)//original x + (y << 16)
{
    uint v = asuint(x);
    //return v & int2(65280, -16777216);
    return (v >> int2(0, 16)) & 65280;
}
float4 decodeAsUIntToByte4(float x)//original x + (y << 8) + (z << 16)+ (w << 24)
{
    uint v = asuint(x);
    //return v & int4(255, 65280, 16711680, -16777216);
    return (v >> int4(0, 8, 16, 24)) & 255;
    //return float4(v & 255, v & 65280, v & 16711680, v & -16777216);
}
float3 decodeToWaterFaceCubeAngle(float x)// original x + (y << 8) + (z << 16)
{
    return decodeFloatToByte3(x) * 90.0 * 0.017453292519943296;
}
float decodeFloatTo90AngleRadius(float x)// x : 0 ~ 3
{
    return x * 90.0 * 0.017453292519943296;
}