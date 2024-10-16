#include "VectorCompress.hlsl"
#include "RotateMatrix.hlsl"
#include "Noise.hlsl"
#define UNITY_PI 3.14159265359f
void geomAppendQuad(float3 pos, float3 windPos, float angle, float texIndex, inout TriangleStream<geometryOutPut> triStream)
{
    float3x3 transformationMatrix = RotateY(angle * UNITY_PI);
    float _Width = 0.5;
    float _Height = 1.0;
    triStream.Append(GeometryVertexOutput(pos + mul(transformationMatrix, float3(-_Width, 0.0, 0.0)), float4(0.0, 0.0, texIndex, 0.0)));
    triStream.Append(GeometryVertexOutput(windPos + mul(transformationMatrix, float3(-_Width, _Height, 0.0)), float4(0.0, 1.0, texIndex, 0.0)));
    triStream.Append(GeometryVertexOutput(pos + mul(transformationMatrix, float3(_Width, 0.0, 0.0)), float4(1.0, 0.0, texIndex, 0.0)));
    triStream.Append(GeometryVertexOutput(windPos + mul(transformationMatrix, float3(_Width, _Height, 0.0)), float4(1.0, 1.0, texIndex, 0.0)));
}
[maxvertexcount(8)]
void geom(point vertexOutput IN[1] : SV_POSITION, inout TriangleStream<geometryOutPut> triStream)
{
    float3 pack = IN[0].positionOS.xyz;
    float index = pack.y;
    float3 pos = decodeFloatToByte3(pack.x);
	
    float randomAngle = Random(pos);
    pos.xz += (randomAngle * _PosOffsetStrength) * 0.5 + 0.5;
	
    float2 worldPosxz = TransformObjectToWorld(pos).xz;
    float2 noiseValue = sin(((worldPosxz) + float2(_Time.y, _Time.y)) * _WindDensity);
    float2 dir = (noiseValue + _WindDirection) * _WiggleRange;
    float3 windPos = float3(dir.x, 0.0, dir.y) + pos;
	
    geomAppendQuad(pos, windPos, randomAngle, index, triStream);
    triStream.RestartStrip();
    geomAppendQuad(pos, windPos, randomAngle + 0.5, index, triStream);
}