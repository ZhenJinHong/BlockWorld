Shader "VoxelWorld/VoxelWater"
{
    Properties
    {
        //_ShallowColor("ShallowColor", Color) = (0.0235849, 0.815128, 1, 1)
        _WaterColor("WaterColor", Color) = (0.0235849, 0.815128, 1, 1)
        _WaterDepth("WaterDepth", Range(0.0, 1.0)) = 1
        //_DeepColor("DeepColor", Color) = (0.1012371, 0.2579553, 0.6132076, 1)
        _FoamColor("FoamColor", Color) = (0.1839623, 0.3464136, 1, 0)
        _FoamAmount("FoamAmount", Float) = 0
        _FoamSpeed("FoamSpeed", Float) = 0.1
        _FoamScale("FoamScale", Float) = 100
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            // 透明的消耗大于非透明 rendertype 似乎无法决定任何东西,只有Queue可以决定
            "RenderType"="Transparent"
            //"RenderType" = "Opaque"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            //"Queue" = "Geometry"
            "DisableBatching" = "False"
        }
        HLSLINCLUDE
        //#pragma multi_compile_instancing
        //#pragma multi_compile_fog
        //#pragma instancing_options renderinglayer

        //// Keywords
        //#pragma multi_compile _ LIGHTMAP_ON
        //#pragma multi_compile _ DIRLIGHTMAP_COMBINED
        //#pragma shader_feature _ _SAMPLE_GI
        //#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        //#pragma multi_compile_fragment _ DEBUG_DISPLAY
        //#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        //// GraphKeywords: <None>
        
        //// Defines
        
        //#define ATTRIBUTES_NEED_NORMAL
        //#define ATTRIBUTES_NEED_TANGENT
        //#define VARYINGS_NEED_POSITION_WS
        //#define VARYINGS_NEED_NORMAL_WS
        //#define FEATURES_GRAPH_VERTEX
        ///* WARNING: $splice Could not find named fragment 'PassInstancing' */
        //#define SHADERPASS SHADERPASS_UNLIT
        //#define _FOG_FRAGMENT 1
        //#define _SURFACE_TYPE_TRANSPARENT 1
        //#define REQUIRE_DEPTH_TEXTURE

		#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"

        #include "Assets\Scripts\Shader\HLSL\RotateMatrix.hlsl"
        #include "Assets\Scripts\Shader\HLSL\VectorCompress.hlsl"
        #include "Assets\Scripts\Shader\HLSL\DepthTexture.hlsl"
        #include "Assets\Scripts\Shader\HLSL\Movement.hlsl"
        #include "Assets\Scripts\Shader\HLSL\Noise.hlsl"
        CBUFFER_START(UnityPerMaterial)
        float4 _WaterColor;
        float _WaterDepth;
        float _FoamAmount;
        float _FoamSpeed;
        float _FoamScale;
        float4 _FoamColor;
        CBUFFER_END

        struct vertexInput
        {
            float3 positionOS : POSITION;
        };
        struct vertexOutput
        {
            float4 positionOS : SV_POSITION;
        };
        vertexOutput vert(vertexInput v)
        {
            vertexOutput o;
            o.positionOS = float4(v.positionOS, 0.0);
            return o;
        }
        struct geometryOutPut
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS : TEXCOORD0;
        };
        geometryOutPut geometryVertex(float3 positionOS, float3 vertex, float3 rotateVertex)
        {
            geometryOutPut o;
            o.positionCS = TransformObjectToHClip(rotateVertex + positionOS + 0.5);
            o.positionWS = TransformObjectToWorld(vertex + positionOS);// 参数不一样,就不采用先转世界空间,世界空间再转裁减空间
            return o;
        }
// 几何展开是可以降低消耗的,至少在显存上很明显
        [maxvertexcount(4)]
        void geom(point vertexOutput IN[1] : SV_POSITION, inout TriangleStream<geometryOutPut> triStream)
        {
            float3 pack = IN[0].positionOS.xyz;
            float3 positionOS = decodeFloatToByte3(pack.x);
            float3 compressAngle = decodeToWaterFaceCubeAngle(pack.y);
            float3x3 mat = EulerXYZ(compressAngle);
    
            triStream.Append(geometryVertex(positionOS, float3(-0.5,0.5,-0.5), mul(mat, float3(-0.5,0.5,-0.5))));
            triStream.Append(geometryVertex(positionOS, float3(-0.5,0.5, 0.5), mul(mat, float3(-0.5,0.5, 0.5))));
            triStream.Append(geometryVertex(positionOS, float3( 0.5,0.5,-0.5), mul(mat, float3( 0.5,0.5,-0.5))));
            triStream.Append(geometryVertex(positionOS, float3( 0.5,0.5, 0.5), mul(mat, float3( 0.5,0.5, 0.5))));
            triStream.RestartStrip();
        }
        ENDHLSL

        Pass
        {
            Name "Universal Forward"
            Tags
            {
                  // LightMode: <None>
            }
            Cull Off 
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha 
            ZTest LEqual 
// 关闭之后消耗反而增加了,打开的时候这个shader消耗很接近于非透明的,如果把透明关掉,消耗低于非透明
// 或许是和之前那个只有透明物体时消耗极大的Bug有关
            ZWrite Off

            HLSLPROGRAM
        
            // Pragmas
            #pragma target 4.0
            #pragma require geometry
            
            #pragma vertex vert
	        #pragma geometry geom
            #pragma fragment frag

            half4 frag(geometryOutPut v) : SV_Target
            {
                float depth = DepthFade(v.positionCS);
    
                //half4 color = lerp(_ShallowColor, _DeepColor, SaturateDepth(depth, _WaterDepth));
                float4 color = _WaterColor * SaturateDepth(depth, _WaterDepth);
                float moveNoise = Unity_GradientNoise_Deterministic_float(WroldAsUV_Offset(v.positionWS.xz, _FoamSpeed), _FoamScale);
                float foamPer = step(SaturateDepth(depth, _FoamAmount), moveNoise);
    
                color = lerp(color, _FoamColor, foamPer);
                color.a = depth > 0.015 ? color.a : 0.0;
                            // a = depth 可以获得一些奇异效果
                return color;
                //return _DeepColor;
            }

            ENDHLSL
        }
    }
}
