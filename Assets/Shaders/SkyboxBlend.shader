Shader "Custom/SkyboxBlend"
{
    Properties
    {
        _CubemapA ("Skybox A", CUBE) = "" {}
        _CubemapB ("Skybox B", CUBE) = "" {}
        _BlendFactor ("Blend Factor", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            samplerCUBE _CubemapA;
            samplerCUBE _CubemapB;
            float _BlendFactor;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 colorA = texCUBE(_CubemapA, i.texcoord);
                fixed4 colorB = texCUBE(_CubemapB, i.texcoord);
                return lerp(colorA, colorB, _BlendFactor);
            }
            ENDCG
        }
    }
}
