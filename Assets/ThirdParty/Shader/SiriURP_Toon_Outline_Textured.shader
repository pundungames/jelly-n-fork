Shader "Siri/URP_Toon_Outline_Textured"
{
    Properties
    {
        [Header(Base Settings)]
        _MainTex("Main Texture", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1,1,1,1)
        
        [Header(Toon Lighting)]
        _Step("Toon Step", Range(0, 1)) = 0.5
        _Smoothness("Toon Smoothness", Range(0, 0.1)) = 0.01
        
        [Header(Outline Settings)]
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(0, 0.1)) = 0.02
        _OutlineTex("Outline Texture", 2D) = "white" {}
        _OutlineTexTiling("Outline Texture Tiling", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" "Queue"="Geometry" }

        // --- PASS 1: TOON SHADING ---
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _BaseColor;
            float _Step;
            float _Smoothness;

            Varyings vert(Attributes IN) {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                float3 normal = normalize(IN.normalWS);
                Light light = GetMainLight();
                float NdotL = dot(normal, light.direction);
                float toonLight = smoothstep(_Step, _Step + _Smoothness, NdotL);
                half4 texColor = tex2D(_MainTex, IN.uv);
                return texColor * _BaseColor * (toonLight + 0.4);
            }
            ENDHLSL
        }

        // --- PASS 2: OUTLINE ---
        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _OutlineWidth;
            float4 _OutlineColor;
            sampler2D _OutlineTex;
            float _OutlineTexTiling;

            Varyings vert(Attributes IN) {
                Varyings OUT;
                float3 posOS = IN.positionOS.xyz + (IN.normalOS * _OutlineWidth);
                OUT.positionCS = TransformObjectToHClip(posOS);
                OUT.uv = IN.uv * _OutlineTexTiling;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                half4 tex = tex2D(_OutlineTex, IN.uv);
                return tex * _OutlineColor;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}