
#ifndef VERTEXANIMATIONUTILS_INCLUDED
#define VERTEX_ANIMATION_INCLUDED

#include "VectorEncodingDecoding.hlsl"
#include "SampleTexture2DArrayLOD.hlsl"

float2 VA_UV_float(float2 UV, int MaxFrames, float Time)
{
	float2 uvPosition;
	float timeInFrames = frac(Time);
	timeInFrames = ceil(timeInFrames * MaxFrames);
	timeInFrames /= MaxFrames;
	timeInFrames += round(1.0f / MaxFrames);

	uvPosition.x = UV.x;

//#ifdef VA_FLIP_UVS_ON
//	uvPosition.y = (1.0f - (timeInFrames)) + (1.0f - (1.0f - uv.y));
//#else
	uvPosition.y = (1.0f - (1.0f - UV.y) - (1.0f - (timeInFrames)));
//#endif

	return uvPosition;
}

void VA_float(float2 UV, SamplerState Sampler, Texture2D PositionMap, float Time, int MaxFrames,
	out float3 Position, out float Alpha)
{
	float2 uvPosition = VA_UV_float(UV, MaxFrames, Time);

	// Position.
	float4 texturePos = PositionMap.SampleLevel(Sampler, uvPosition, 0);
	Position = texturePos.xyz;

	// Normal.
	Alpha = texturePos.w;
}

void VA_ARRAY_float(float2 uv, SamplerState texSampler, Texture2DArray positionMap, float positionMapIndex, float time, int maxFrames,
	out float3 position, out float alpha)
{
	float2 uvPosition = VA_UV_float(uv, maxFrames, time);

	// Position.
	float4 texturePos;
	SampleTexture2DArrayLOD_float(positionMap, uvPosition, texSampler, positionMapIndex, 0, texturePos);
	position = texturePos.xyz;

	// Normal.
	//FloatToFloat3_float(texturePos.w, outNormal);
	alpha = texturePos.w;
}

#endif