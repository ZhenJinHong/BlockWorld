Shader "VoxelWorld/VoxelWater"
{
    Properties
    {
        _MainColor("MainColor", Color) = (1.0,1.0,1.0,1.0)
        _FogDistance("FogDistance", Float) = 100.0
        _FogBottomHeight("FogBottomHeight", Range(0.1, 1)) = 0.8
        _FogTopHeight("FogTopHeight", Range(0.1, 1)) = 0.9
        _FogGradient("FogGradient", Range(1, 100)) = 2
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

        #include "Assets\Scripts\Shader\HLSL\DepthTexture.hlsl"
        CBUFFER_START(UnityPerMaterial)
        float4 _MainColor;
        float _FogDistance;
        float _FogTopHeight;
        float _FogBottomHeight;
        float _FogGradient;
        CBUFFER_END

        struct vertexInput
        {
            float3 positionOS : POSITION;
        };
        struct vertexOutput
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS : TEXCOORD0;
            float3 positionOS : TEXCOORD1;
            
        };
        vertexOutput vert(vertexInput v)
        {
            vertexOutput o;
            o.positionOS = v.positionOS;
            o.positionWS = TransformObjectToWorld(v.positionOS);
            o.positionCS = TransformWorldToHClip(o.positionWS);
            return o;
        }
        ENDHLSL

        Pass
        {
            Name "Universal Forward"
            Tags
            {
                  // LightMode: <None>
            }
            Cull Back 
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha 
            ZTest LEqual 
// 关闭之后消耗反而增加了,打开的时候这个shader消耗很接近于非透明的,如果把透明关掉,消耗低于非透明
// 或许是和之前那个只有透明物体时消耗极大的Bug有关
            ZWrite Off

            HLSLPROGRAM
        
            // Pragmas
            #pragma target 4.0
            
            #pragma vertex vert
            #pragma fragment frag

            half4 frag(vertexOutput v) : SV_Target
            {
                float depth = DepthFade(v.positionCS);
                // 要注意的是深度值多数时候远超 1 
                depth -= _FogDistance;
                depth = max(depth, 0.0);
                depth /= _FogGradient;
                depth = clamp(depth, 0.0, 1.0);
                //depth = saturate(depth * (1.5 - v.positionOS.y));
                //depth = depth * (1.0 - v.positionOS.y);
                //depth *= step(v.positionOS.y, _FogHeight);
                //depth = lerp(depth,0.0,  v.positionOS.y);
                //depth *= 1.0 - v.positionOS.y + _FogHeight;
                //float wsY = v.positionWS.y;
                depth *= 1 - smoothstep(_FogBottomHeight, _FogTopHeight, v.positionOS.y);
                return depth * _MainColor;
                //return _DeepColor;
            }

            ENDHLSL
        }
    }
}
