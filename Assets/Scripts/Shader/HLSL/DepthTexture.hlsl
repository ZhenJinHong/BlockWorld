#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Function.hlsl"

TEXTURE2D_X_FLOAT(_CameraDepthTexture);
SAMPLER(sampler_CameraDepthTexture);

float SampleSceneDepth(float2 uv)
{
    return SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(uv)).r;
}
// x = 1 or -1 (-1 if projection is flipped)
// y = near plane
// z = far plane
// w = 1/far plane
// float4 _ProjectionParams;

// 这个深度纹理似乎只能在片元着色器使用
float Unity_SceneDepth_Eye_float(float2 UV)
{
    return LinearEyeDepth(SampleSceneDepth(UV), _ZBufferParams);
    //return Linear01DepthFromNear(SampleSceneDepth(UV), _ZBufferParams);
}
float DepthFade(float4 positionCS)
{
    // 实际就是posCS,只是因为不同显卡驱动的坐标系起点不一样,需要调整
    float2 PixelPosition = positionCS.xy;
//#if UNITY_UV_STARTS_AT_TOP
//    PixelPosition = float2(positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - positionCS.y) : positionCS.y);
//#else
//    PixelPosition = float2(positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - positionCS.y) : positionCS.y);
//    #endif
    // 计算像素位置在屏幕空间的比例
    float2 NDCPosition = PixelPosition.xy / _ScaledScreenParams.xy;
    //NDCPosition.y = 1.0f - NDCPosition.y;// 为何逆转?
    
    float sceneDepth = Unity_SceneDepth_Eye_float(NDCPosition);
    
    float ScreenPositionW = ComputeScreenPos(positionCS, _ProjectionParams.x).w;
    return sceneDepth - ScreenPositionW;
}
float SaturateDepth(float depth, float distance)
{
    return saturate(depth * distance);// 原本是除法
}