Shader "CheesyRobot/ToonWater"
{
    Properties{
        _DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
        _DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1
        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        _SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777
        _FoamDistance("Foam Distance", Float) = 0.4
        _SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)
        _SurfaceDistortion("Surface Distortion", 2D) = "white" {}	
    _SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27
    }
    SubShader {
        //Tags{ "RenderPipeline" = "UniversalPipeline" }

        Pass {
            //Name "ForwardLit" // For debugging
            //Tags{"LightMode" = "UniversalForward"} // Pass specific tags. 
            Tags { "RenderType"="Transparent" "Queue"="Transparent" }
            // "UniversalForward" tells Unity this is the main lighting pass of this shader
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
           
            struct Attributes {
	            float3 positionOS : POSITION; // Position in object space
                float4 uv : TEXCOORD0;
            };

           
            struct Interpolators {
	            // This value should contain the position in clip space (which is similar to a position on screen)
	            // when output from the vertex function. It will be transformed into pixel position of the current
	            // fragment on the screen when read from the fragment function
	            float4 positionCS : SV_POSITION;
                float4 screenPosition : TEXCOORD2;
                float2 noiseUV : TEXCOORD0;
                float2 distortUV : TEXCOORD1;
            };

            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;

            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;

            Interpolators Vertex(Attributes input) {
	            Interpolators output;

	            output.positionCS = UnityObjectToClipPos(input.positionOS);
                output.screenPosition = ComputeScreenPos(output.positionCS);
                output.noiseUV = TRANSFORM_TEX(input.uv, _SurfaceNoise);
                output.distortUV = TRANSFORM_TEX(input.uv, _SurfaceDistortion);
	            return output;
            }

            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;
            float _DepthMaxDistance;

            sampler2D _CameraDepthTexture;
            sampler2D sampler_CameraDepthTexture;

            float _SurfaceNoiseCutoff;
            float _FoamDistance;

            float2 _SurfaceNoiseScroll;
            float _SurfaceDistortionAmount;

            float4 Fragment(Interpolators input) : SV_TARGET {
                float2 screenSpaceUV = input.screenPosition.xy / input.screenPosition.w;
                float existingDepthLinear = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,screenSpaceUV));

                // Depth water color
                float depthDifference = existingDepthLinear - input.positionCS.w;
	            float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);
                float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference01);

                float2 distortSample = (tex2D(_SurfaceDistortion, input.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;

                // Scrolling texture
                float2 noiseUV = float2((input.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x, 
                    (input.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);

                // White edges and white parts in water
                float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;
                float foamDepthDifference01 = saturate(depthDifference / _FoamDistance);
                float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;
                float surfaceNoise = surfaceNoiseSample > surfaceNoiseCutoff ? 1 : 0;

                return waterColor + surfaceNoise;
            }

            ENDCG
        }
    }
}
