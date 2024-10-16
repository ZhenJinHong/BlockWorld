Shader"VoxelWorld/VoxelGrass"
{
    Properties
    {
		_AlphaClipThreshold("AlphaClipThreshold",Range(0,1)) = 0.1
		_BaseTex2DArray("BaseTex2DArray", 2DArray) = "" {}
		_PosOffsetStrength("Pos Offset Strength", Range(0,1)) = 0.3
		//_Width("Width", float) = 0.5
		//_Height("Height", float) = 1
		_WindDensity("WindDensity", Float) = 0.1
        _WindDirection("WindDirection", Vector) = (0, 0, 0, 0)
        _WiggleRange("WiggleRange", Float) = 0.5
    }
    SubShader
    {
        Tags
		{
			"RenderType" = "Opaque"
			"UniversalMaterialType" = "SimpleLit"
            "Queue" = "AlphaTest"
			"RenderPipeline" = "UniversalPipeline"
			"IgnoreProjector" = "True"
		}

		HLSLINCLUDE
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"

			// #pragma multi_compile _ LIGHTMAP_ON
			// #pragma multi_compile _ DIRLIGHTMAP_COMBINED
			// #pragma shader_feature _ _SAMPLE_GI
			// #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			// #pragma multi_compile_fragment _ DEBUG_DISPLAY
			// #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION

			TEXTURE2D_ARRAY(_BaseTex2DArray);
			SAMPLER(sampler_BaseTex2DArray); 

			CBUFFER_START(UnityPerMaterial)
			float _AlphaClipThreshold;
			float _PosOffsetStrength;
			float _WindDensity;
			float2 _WindDirection;
			float _WiggleRange;
        
			CBUFFER_END
			// 顶点UV的x为索引
			struct vertexInput
			{
				float3 positionOS  : POSITION;
			};

			struct vertexOutput
			{
				float4 positionOS : SV_POSITION;
			};
			vertexOutput geomVert (vertexInput v)
            {
				vertexOutput o; 
				o.positionOS = float4(v.positionOS, 0.0);
                return o;
            }

		ENDHLSL
        Pass
        {
			Name "Universal Forward"
			Tags
			{
				 "LightMode" = "UniversalForward"
			}
			Cull Off
			Blend One Zero

			ZTest LEqual
			ZWrite On

			//AlphaToMask On
            HLSLPROGRAM

			// #pragma multi_compile_instancing
			// #pragma multi_compile_fog
			// #pragma instancing_options renderinglayer

			// #define _FOG_FRAGMENT 1
            // #define _ALPHATEST_ON 1
   //         #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			//#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			//#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

			#pragma require 2darray
			#pragma require geometry

			#pragma vertex geomVert
			#pragma geometry geom
            #pragma fragment frag
			struct geometryOutPut
            {
                float4 positionCS : SV_POSITION;
                float4 uv : TEXCOORD0;
				float3 postionWS : TEXCOORD1;
				//float4 shadowCoord : TEXCOORD2;
			};
            geometryOutPut GeometryVertexOutput(float3 positionOS, float4 uv)
            {
                geometryOutPut o;
				float3 postionWS = TransformObjectToWorld(positionOS);
                o.positionCS = TransformWorldToHClip(postionWS);
				o.uv = uv;
				o.postionWS = postionWS;
				//o.shadowCoord = TransformWorldToShadowCoord(postionWS);
                return o;
            }
			#include "Assets\Scripts\Shader\HLSL\GeometryGrassQuad.hlsl"
			half4 frag(geometryOutPut i) : SV_TARGET
            {
				float4 color = PLATFORM_SAMPLE_TEXTURE2D_ARRAY(UnityBuildTexture2DArrayStruct(_BaseTex2DArray).tex, UnityBuildTexture2DArrayStruct(_BaseTex2DArray).samplerstate, i.uv.xy, i.uv.z);
				
				//float4 shadowCoord = TransformWorldToShadowCoord(i.posWs);
				//color.xyz *= MainLightRealtimeShadow(i.shadowCoord) + _AmbientStrength;
				//color.xyz *= _AmbientLight.xyz;
				//color.xyz += color.xyz * _AmbientLight.xyz * _AmbientStrength;
				//color.xyz *= (1 + _AmbientStrength) * _AmbientLight.xyz * (MainLightRealtimeShadow(i.shadowCoord) + _AmbientLight.xyz * _AmbientStrength);
				//color.xyz *= (1 - lerp(_AmbientOcclusion, 0, i.uv.y) + _AmbientStrength) * _AmbientLight.xyz;
				//color.xyz = lerp(color.xyz * _AmbientLight.xyz, _AmbientLight.xyz, 0.25);
				//color
				//color.a = step(_AlphaClipThreshold, color.a);
				clip(color.a - _AlphaClipThreshold);
				// 这个不仅包括阴影shadowAttenuation(用阴影MainLightShadow.hlsl计算),还有别的
				Light mainLight = GetMainLight(/*i.shadowCoord*/TransformWorldToShadowCoord(i.postionWS), i.postionWS, 1);
				// shadowAttenuation 大概为0或1
				color *= (mainLight.shadowAttenuation /** mainLight.distanceAttenuation */+ 0.2)  * float4(mainLight.color, 1);
				return color;
			}

			ENDHLSL
		}
		Pass
		{
		    Name "ShadowCaster"
			Tags
			{
					"LightMode" = "ShadowCaster"
			}

			Cull Off
			ZTest LEqual
			ZWrite On

			ColorMask 0
			//AlphaToMask On
            HLSLPROGRAM
			//#pragma multi_compile_instancing

			 //   #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			//#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			//#pragma multi_compile _ _SHADOWS_SOFT
			//#define SHADERPASS SHADERPASS_SHADOWCASTER
			//#define _ALPHATEST_ON 1
    
			#pragma require 2darray
			#pragma require geometry

			#pragma vertex geomVert
			#pragma geometry geom
            #pragma fragment frag
			struct geometryOutPut
            {
                float4 positionCS : SV_POSITION;
                float4 uv : TEXCOORD0;
			};
            geometryOutPut GeometryVertexOutput(float3 positionOS, float4 uv)
            {
                geometryOutPut o;
                o.positionCS = TransformObjectToHClip(positionOS);
                o.uv = uv;
                return o;
            }
			#include "Assets\Scripts\Shader\HLSL\GeometryGrassQuad.hlsl"
			// Keywords
			//#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
			half4 frag(geometryOutPut i) : SV_TARGET
			{
				float4 color = PLATFORM_SAMPLE_TEXTURE2D_ARRAY(UnityBuildTexture2DArrayStruct(_BaseTex2DArray).tex, UnityBuildTexture2DArrayStruct(_BaseTex2DArray).samplerstate, i.uv.xy, i.uv.z);
    
				//color.a = step(0.5, color.a);
				clip(color.a - _AlphaClipThreshold);
				return 0.0;
			}
	       ENDHLSL
        }

    }
    FallBack "Diffuse"
}