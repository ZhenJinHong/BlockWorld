Shader "VoxelWorld/VoxelFire"
{
    Properties
    {
        _BaseColor("BaseColor", Color) = (1, 0, 0.009523392, 1)
        _InColor("InColor", Color) = (1, 0.6567878, 0, 1)
        _Speed("Speed", Float) = -1
        _Scale("Scale", Float) = 5
        _Tilling("Tilling", Vector) = (1, 1, 0, 0)
        _Transition("Transition", Float) = -0.4
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue" = "Transparent"
            "DisableBatching" = "False"
   //         "RenderType" = "Opaque"
			//"UniversalMaterialType" = "SimpleLit"
   //         "Queue" = "AlphaTest"
			//"RenderPipeline" = "UniversalPipeline"
			//"IgnoreProjector" = "True"
        }
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"

        #include "Assets\Scripts\Shader\HLSL\Constant.hlsl"
        #include "Assets\Scripts\Shader\HLSL\VectorCompress.hlsl"
        #include "Assets\Scripts\Shader\HLSL\RotateMatrix.hlsl"
        #include "Assets\Scripts\Shader\HLSL\Movement.hlsl"
        #include "Assets\Scripts\Shader\HLSL\Noise.hlsl"
        CBUFFER_START(UnityPerMaterial)
        float4 _BaseColor;
        float _Speed;
        float _Scale;
        float2 _Tilling;
        float4 _InColor;
        float _Transition;
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
            float4 uv : TEXCOORD0;// 因为4浮点对齐的原因,这里2或4看起来没有性能影响浮动
        };
        geometryOutPut GeometryVertexOutput(float3 positionOS, float4 uv)
        {
            geometryOutPut o;
            o.positionCS = TransformObjectToHClip(positionOS);
            //uv.xy += positionOS.xz;
            o.uv = uv;
            return o;
        }
// 几何展开是可以降低消耗的,至少在显存上很明显
        void geomAppendQuad(float3 pos, float angle,  inout TriangleStream<geometryOutPut> triStream)
        {
            float3x3 transformationMatrix = RotateY(angle * UNITY_PI);
            float _Width = 0.7;
            float _Height = 1.5;
            triStream.Append(GeometryVertexOutput(pos+mul(transformationMatrix, float3(-_Width, 0.0, 0.0)), float4(0.0, 0.0, 0.0, 0.0)));
            triStream.Append(GeometryVertexOutput(pos+mul(transformationMatrix, float3(-_Width, _Height, 0.0)), float4(0.0, 1.0, 0.0, 0.0)));
            triStream.Append(GeometryVertexOutput(pos+mul(transformationMatrix, float3(_Width, 0.0, 0.0)), float4(1.0, 0.0, 0.0, 0.0)));
            triStream.Append(GeometryVertexOutput(pos+mul(transformationMatrix, float3(_Width, _Height, 0.0)), float4(1.0, 1.0, 0.0, 0.0)));
        }
        [maxvertexcount(8)]
        void geom(point vertexOutput IN[1] : SV_POSITION, inout TriangleStream<geometryOutPut> triStream)
        {
            float3 pack = IN[0].positionOS.xyz;
            float3 pos = decodeFloatToByte3(pack.x);
	        pos += float3(0.5,-0.1,0.5);
            float randomAngle = Random(pos);
           
	
            geomAppendQuad(pos, randomAngle, triStream);
            triStream.RestartStrip();
            geomAppendQuad(pos, randomAngle + 0.5, triStream);
        }
        //[maxvertexcount(4)]
        //void geom(point vertexOutput IN[1] : SV_POSITION, inout TriangleStream<geometryOutPut> triStream)
        //{
        //    float3 pack = IN[0].positionOS.xyz;
        //    float3 positionOS = decodeFloatToByte3(pack.x);
        //    float3 compressAngle = decodeToWaterFaceCubeAngle(pack.y);
        //    float3x3 mat = EulerXYZ(compressAngle);
    
        //    triStream.Append(geometryVertex(positionOS, float4( 0.0,0.0, 0.0,0.0), mul(mat, float3( 0.5, 0.5, 0.5))));
        //    triStream.Append(geometryVertex(positionOS, float4( 0.0,1.0, 0.0,0.0), mul(mat, float3( 0.5, 0.5,-0.5))));
        //    triStream.Append(geometryVertex(positionOS, float4( 1.0,0.0, 0.0,0.0), mul(mat, float3(-0.5, 0.5, 0.5))));
        //    triStream.Append(geometryVertex(positionOS, float4( 1.0,1.0, 0.0,0.0), mul(mat, float3(-0.5, 0.5,-0.5))));
        //    triStream.RestartStrip();
        //}
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
            //Cull Off
			//Blend One Zero

			//ZTest LEqual
			//ZWrite On

            HLSLPROGRAM
        
            // Pragmas
            #pragma target 4.0
            #pragma require geometry
            
            #pragma vertex vert
	        #pragma geometry geom
            #pragma fragment frag

            half4 frag(geometryOutPut v) : SV_Target
            {
                float2 offset = v.uv.xy;
                offset.y += _Time.y * _Speed;
                float moveNoise = Unity_GradientNoise_Deterministic_float(offset, _Tilling * _Scale);
                float oneMinusUvy = (1 - v.uv.y);
                moveNoise = moveNoise * oneMinusUvy;// 从下到上渐变
                //moveNoise = lerp(moveNoise, 1.0, oneMinusUvy);// 从下到上渐少填满噪声黑色部分,
                moveNoise += oneMinusUvy;// 拉高底部亮度
                //moveNoise *= ;
                float4 color = lerp(_BaseColor, _InColor, moveNoise + _Transition);
                color.a = moveNoise;
                //clip(color.a - 0.5);
                return color;
            }

            ENDHLSL
        }
    }
}
