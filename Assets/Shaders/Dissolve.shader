Shader "Custom/DissolveGate"
{
    Properties
    {
        _MainTex ("Gate Shape Texture", 2D) = "white" {}  // The round gate texture
        _DissolveTex ("Dissolve Texture", 2D) = "white" {} // The noise texture
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _EdgeWidth ("Edge Width", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha  // Preserve transparency
        ZWrite Off // Ensure proper transparency handling
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;      // Round gate texture
            sampler2D _DissolveTex;  // Noise texture for dissolve
            float _DissolveAmount;
            float4 _GlowColor;
            float _EdgeWidth;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dissolveValue = tex2D(_DissolveTex, i.uv).r; // Get dissolve noise
                float edge = smoothstep(_DissolveAmount - _EdgeWidth, _DissolveAmount, dissolveValue);
                
                fixed4 gateColor = tex2D(_MainTex, i.uv); // Get gate shape texture
                
                // Apply dissolve while keeping original shape
                gateColor.a *= edge;

                // Clip pixels based on dissolve value
                clip(dissolveValue - _DissolveAmount);

                return gateColor;
            }
            ENDCG
        }
    }
}
