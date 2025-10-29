Shader "Unlit/WaterShader"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (0,0,0,1)
        //_Ambient ("Ambient Color", Color) = (0,0,0,1)
        _Normals ("Normal Map", 2D) = "bump" {}
        [NoScaleOffset] _Displacement ("Displacement Map", 2D) = "gray" {}
        [NoScaleOffset] _DiffuseIBL ("Diffuse IBL", 2D) = "black" {}
        [NoScaleOffset] _SpecularIBL ("Specular IBL", 2D) = "black" {}
        _Gloss ("Gloss", Range(0,1)) = 0.2
        _Rotation ("IBL rotation", Range(0,1)) = 0.5
        _SpecIBLIntensity ("Specular IBL Intensity", Range(0,1)) = 0.5
        _DiffIBLIntensity ("Diffuse IBL Intensity", Range(0,1)) = 0.5
        _FresnelPower ("Fresnel Power", Range(0,10)) = 2
        _LerpT ("Scrolling T Value", Range(0,1)) = 0.5
        _Scale ("UV Scale", Float) = 1
        _Speed ("Flow Speed", Range(0.001,2)) = 1
        _DepthFade ("Depth Fade", Range(0.1,5)) = 1
        _DispStrength ("Displacement Strength", Range(0,0.4)) = 0.1
        _Smoothness ("Normal Smoothness", Range(0,20)) = 0.0
        _NormalIntensity ("Normal Intensity", Range(0,1)) = 1
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        // Grab the screen behind the object into _BackgroundTexture
        // GrabPass
        // {
        //     //"_BackgroundTexture"
        //     "_CameraOpaqueTexture"
        // }

        Pass
        {
            // Tags {"LightMode" = "ForwardBase"}
            // Cull off
            // ZWrite off
            // Blend SrcAlpha OneMinusSrcAlpha
            //Blend DstColor Zero

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            #define pi 3.14159265359

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 worldPos : TEXCOORD2;
                float3 tangent : TEXCOORD3;
                float3 bitangent : TEXCOORD4;
                float2 uv1 : TEXCOORD5;
                float2 uv2 : TEXCOORD6;
                float height : TEXCOORD7;
                float4 screenPos : TEXCOORD8;
                float4 grabPos : TEXCOORD9;
            };

            sampler2D _MainTex;
            sampler2D _Normals;
            sampler2D _Displacement;
            sampler2D _DiffuseIBL;
            sampler2D _SpecularIBL;
            sampler2D _CameraDepthTexture;
            sampler2D _CameraOpaqueTexture;
            float _DispStrength;
            float _Smoothness;
            float _NormalIntensity;
            float _Gloss;
            float _SpecIBLIntensity;
            float _DiffIBLIntensity;
            float _Rotation;
            float _FresnelPower;
            float _LerpT;
            float _Scale;
            float _Speed;
            float _DepthFade;
            float3 _Color;
            float3 _Color2;
            float4 _Normals_ST;

            // float2 Rotate(float2 uv, float andRad) {
            //     float ca = cos(angRad);
            //     float sa = sin(angRad);
            //     return float2(ca * v.x - sa * v.y, sa * v.x + ca * v.y);
            // }

            float2 DirToRectilinear(float3 dir)
            {
                float x = atan2(dir.z, dir.x) / (pi * 2) + _Rotation; // 0-1
                float y = dir.y * 0.5 + 0.5; // 0-1
                return float2(x,y);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _Normals);
                // o.uv = v.uv;
                o.uv1 = float2(v.uv.x - _Time.x * _Speed, v.uv.y) * _Scale;
                o.uv2 = float2(-v.uv.x - _Time.x * 0.5 * _Speed, -v.uv.y + _Time.x * _Speed) * _Scale;

                float height1 = tex2Dlod(_Displacement, float4((o.uv1),0,0)).x;
                float height2 = tex2Dlod(_Displacement, float4((o.uv2),0,0)).x;
                o.height = lerp(height1, height2, _LerpT);

                // v.vertex.xyz += v.normal * (o.height * 2 - 1) * _DispStrength;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.tangent = UnityObjectToWorldDir(v.tangent.xyz);
                o.bitangent = cross(o.normal, o.tangent);
                o.bitangent *= v.tangent.w * unity_WorldTransformParams.w; // handle flipping/mirroring

                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                //o.screenPos = ComputeScreenPos(o.vertex);
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float3 normals1 = tex2D(_Normals, i.uv1).xyz;
                float3 normals2 = tex2D(_Normals, i.uv2).xyz;
                normals1 = (normals1 * 2 - 0.5);
                normals2 = (normals2 * 2 - 0.5);
                // float3 normals1 = UnpackNormal(tex2D(_Normals, i.uv1));
                // float3 normals2 = UnpackNormal(tex2D(_Normals, i.uv2));
                // normals1 = normalize(lerp(normals3, normals1, 0.5));
                // normals2 = normalize(lerp(normals4, normals2, 0.5));
                normals1 = normalize(lerp(float3(0,0,1), normals1, _NormalIntensity));
                normals2 = normalize(lerp(float3(0,0,1), normals2, _NormalIntensity));
                // float3 normalsComb = normalize(lerp(normals1, normals2, _LerpT));
                // normalsComb = normalize(lerp(float3(0,0,1), normalsComb, _NormalIntensity));

                float3x3 mtxTangToWorld =
                {
                    i.tangent.x, i.bitangent.x, i.normal.x,
                    i.tangent.y, i.bitangent.y, i.normal.y,
                    i.tangent.z, i.bitangent.z, i.normal.z
                };
                float3x3 mtxTangToWorldInv =
                {
                    -i.tangent.x, i.bitangent.x, i.normal.x,
                    i.tangent.y, -i.bitangent.y, i.normal.y,
                    -i.tangent.z, -i.bitangent.z, i.normal.z
                };
                
                // vectors
                float3 N1 = mul(mtxTangToWorld, normals1);
                float3 N2 = mul(mtxTangToWorld, normals2);
                float3 N = normalize(lerp(N1, N2, _LerpT));
                // float3 N = mul(mtxTangToWorld, normalsComb);
                // N = i.normal;
                //N = normals2;
                // return float4(N, 0);

                float3 L = normalize(UnityWorldSpaceLightDir(i.worldPos));
                float3 V = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 H = normalize(L + V);
                
                // smoother normals
                float3 Ns = normalize(N + i.normal * _Smoothness);

                float dotNL = saturate(dot(Ns,L));
                float dotNV = saturate(dot(N,V));
                float dotNH = saturate(dot(N,H));
                float dotNHs = saturate(dot(Ns,H));
                

                float fresnel0 = pow(1 - dotNV, _FresnelPower);
                //float fresnel = fresnel0 * fresnel0;
                //float fresnelInv = clamp(pow(dotNV,3), 0, 0.4);

                // diffuse
                float3 diffuseLight = saturate(dotNL + 0.0) * _LightColor0.xyz;
                // float3 depthAdd = fresnelInv * saturate(1.8 - i.height) * _Color2 * (diffuseLight * 0.8 + 0.2);

                // specular
                float specularExponent = exp2(_Gloss * 12) + 2;
                float3 specularLight = pow(dotNHs, specularExponent) * _LightColor0.xyz * _Gloss * fresnel0;

                // specular light bleed
                specularLight += pow(dotNHs, specularExponent * 0.2) * _Color * _Gloss;

                // specular culling
                specularLight *= dotNL > 0;
                

                // diffuse IBL
                //float3 diffuseIBL = tex2Dlod(_DiffuseIBL, float4(DirToRectilinear(N),0,0)).xyz;
                float3 diffuseIBL = tex2Dlod(_DiffuseIBL, float4(DirToRectilinear(N),0,0)).xyz;
                diffuseLight += diffuseIBL * _DiffIBLIntensity;// * (0.8 + fresnelInv);

                // specular IBL
                // float3 viewReflect = saturate(reflect(-V, N));
                float3 viewReflect = reflect(-V, N);
                float mip = (1 - _Gloss) * 6;
                float3 specularIBL = tex2Dlod(_SpecularIBL, float4(DirToRectilinear(viewReflect),mip,mip)).xyz;
                specularLight += specularIBL * _SpecIBLIntensity * fresnel0;
                //specularLight = float3(0,0,0);
                // return float4(specularIBL, 1);
                
                // water color
                float3 finalLight = diffuseLight * _Color + specularLight;// + depthAdd;

                // SSS (backlighting actually, no thickness)
                // https://www.alanzucconi.com/2017/08/30/fast-subsurface-scattering-2/
                // float3 subH = normalize(L + N * 0.05);
                // float ViewDotH = pow(saturate(dot(V, -subH)), 2);
                // float h = i.height;
                // float heightMult = -2*(h*h*h - h*h);
                // finalLight = saturate(finalLight + _Color * ViewDotH * heightMult);

                // depth based opacity
                float2 screenSpaceUV = i.screenPos.xy / i.screenPos.w;
                float screenDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenSpaceUV));
                float waterDepth = screenDepth - i.screenPos.w;
                float opacity = saturate(_DepthFade * waterDepth);
                float opacityEdge = pow(1 - saturate(waterDepth), 10);

                // // distortion
                float4 grabPassUV = i.grabPos;
                grabPassUV.x += N.x * 0.25 * opacity;
                grabPassUV.y -= N.z * 0.1;

                float screenDepthDist = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, grabPassUV));
                float waterDepthDist = screenDepthDist - i.screenPos.w;
                float opacityDist = saturate(_DepthFade * waterDepthDist);

                // blending
                float4 bgcolor = tex2Dproj(_CameraOpaqueTexture, grabPassUV);
                bgcolor = float4(lerp(bgcolor.xyz, _Color, opacity), 1);

                // fix the edges when distorting
                bgcolor = float4(lerp(bgcolor, finalLight, (opacityDist > 0.95)), 1);

                float4 col = float4(lerp(bgcolor.xyz, finalLight, opacity), 1);
                col.xyz += _Color2 * finalLight.xxx * opacityEdge * 4;
                return col;

                // float4 col = float4(finalLight, opacity);
                col.xyz += _Color2 * finalLight.xxx * opacityEdge * 4;
                return col;

                // return float4(finalLight, 1);
            }
            ENDCG
        }
    }
    //FallBack "Legacy Shaders/VertexLit"
}
