Shader "Custom/GlowCone_FlameFlicker"
{
    Properties
    {
        _Color ("Glow Color", Color) = (0.2, 0.8, 1, 1)
        _Intensity ("Base Intensity", Float) = 2.0
        _PulseSpeed ("Pulse Speed", Float) = 3.0
        _SoftEdge ("Soft Edge", Range(0, 1)) = 0.5
        _FlickerStrength ("Flicker Strength", Range(0, 2)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha One
            ZWrite Off
            Cull Off
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _Color;
            float _Intensity;
            float _PulseSpeed;
            float _SoftEdge;
            float _FlickerStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                float dist = length(uv - float2(0.5, 0.0));
                float radialFalloff = saturate(1.0 - dist / _SoftEdge);

                float verticalFlare = uv.y * uv.y * 2.0;
                float flicker = sin(_Time.y * _PulseSpeed + uv.y * 12.0 + sin(_Time.y * 2.1 + uv.x * 10.0)) * 0.5 + 0.5;
                flicker = lerp(1.0, flicker, verticalFlare * _FlickerStrength);

                float finalIntensity = _Intensity * flicker;
                float alpha = radialFalloff * verticalFlare;

                fixed4 col = _Color * finalIntensity;
                col.a = alpha * finalIntensity;

                return col;
            }
            ENDCG
        }
    }
}
