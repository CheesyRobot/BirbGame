Shader "CheesyRobot/ToonWater"
{
    Properties{
        _DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
        _DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1

        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        _SurfaceNoiseSecond("Surface Noise Second", 2D) = "white" {}
        _SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777

        _FoamColor("Foam Color", Color) = (1,1,1,1)
        _FoamMaxDistance("Foam Maximum Distance", Float) = 0.4
        _FoamMinDistance("Foam Minimum Distance", Float) = 0.04

        _SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)
        _SurfaceDistortion("Surface Distortion", 2D) = "white" {}	
        _SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27
    }
    SubShader {
        Tags{ "RenderPipeline" = "UniversalPipeline" }

        Pass {
            //Name "ForwardLit" // For debugging
            //Tags{"LightMode" = "UniversalForward"} // Pass specific tags. 
            Tags { "RenderType"="Transparent" "Queue"="Transparent" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment

            #define SMOOTHSTEP_AA 0.02

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"

           float4 alphaBlend(float4 top, float4 bottom)
            {
	            float3 color = (top.rgb * top.a) + (bottom.rgb * (1 - top.a));
	            float alpha = top.a + bottom.a * (1 - top.a);

	            return float4(color, alpha);
            }
           
            struct Attributes {
	            float3 positionOS : POSITION; // Position in object space
                float4 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
            };

           
            struct Interpolators {
	            float4 positionCS : SV_POSITION;
                float4 screenPosition : TEXCOORD2;
                float2 noiseUV : TEXCOORD0;
                float2 distortUV : TEXCOORD1;
                float3 viewNormal : NORMAL;
            };

            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;

            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;

            Interpolators Vertex(Attributes input) {
	            Interpolators output;

                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                output.positionCS = positionInputs.positionCS;
                output.screenPosition = ComputeScreenPos(output.positionCS);
                output.noiseUV = TRANSFORM_TEX(input.uv, _SurfaceNoise);
                output.distortUV = TRANSFORM_TEX(input.uv, _SurfaceDistortion);
                output.viewNormal = normalInputs.normalWS;
	            return output;
            }

            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;
            float _DepthMaxDistance;

            float _SurfaceNoiseCutoff;

            float4 _FoamColor;
            float _FoamMaxDistance;
            float _FoamMinDistance;

            float2 _SurfaceNoiseScroll;
            float _SurfaceDistortionAmount;

            float4 Fragment(Interpolators input) : SV_TARGET {
                float2 screenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);//input.screenPosition.xy / input.screenPosition.w;

                float existingDepthLinear = LinearEyeDepth(SampleSceneDepth(screenSpaceUV), _ZBufferParams);

                // Depth water color
                float depthDifference = existingDepthLinear - input.positionCS.w;

	            float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);
                float absorption = 1 - exp(-depthDifference * _DepthMaxDistance);
                float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, absorption);

                float2 distortSample = (tex2D(_SurfaceDistortion, input.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;

                // Scrolling texture
                float2 noiseUV = float2((input.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x, 
                    (input.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);
                // Second scrolling texture to make tiling less visible
                float2 noiseUVsecond = float2((input.noiseUV.x*0.7 - _Time.y* _SurfaceNoiseScroll.x)+ distortSample.x,
                    (input.noiseUV.y*0.7 - _Time.y* _SurfaceNoiseScroll.y)+ distortSample.y);
                
                // Getting water surface normals to make outline around semi-submerged objects
                float3 existingNormal = SampleSceneNormals(screenSpaceUV);
                float3 normalDot = saturate(dot(existingNormal, input.viewNormal));

                // White edges and white parts in water
                float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;
                float mixedSurfaceNoise = surfaceNoiseSample * tex2D(_SurfaceNoise, noiseUVsecond);

                float foamDistance = lerp(_FoamMaxDistance, _FoamMinDistance, normalDot);
                float foamDepthDifference01 = saturate(depthDifference / foamDistance);
                float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;
                // Foam anti-aliasing
                float surfaceNoise = smoothstep(surfaceNoiseCutoff - SMOOTHSTEP_AA, surfaceNoiseCutoff + SMOOTHSTEP_AA,
                    mixedSurfaceNoise);
                float4 surfaceNoiseColor = _FoamColor;
                surfaceNoiseColor.a *= surfaceNoise;

                return alphaBlend(surfaceNoiseColor, waterColor);
            }

            ENDHLSL
        }
    }
}
