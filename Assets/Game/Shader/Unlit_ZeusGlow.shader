Shader "Unlit/ZeusGlow"
{
    Properties
    {
        _MoonTex("Moon Texture", 2D) = "black" {}
        _ColorA("Color A", Color) = (1,1,1,1)
        _ColorB("Color B", Color) = (0,0,0,1)
        _Slide("Slide", Range(0, 2)) = 0.5
        _Multiply("Multiply", Range(0, 20)) = 6
        _Alpha("Alpha", Float) = 1
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
            LOD 200
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MoonTex;
                float4 _MoonTex_ST;
                float4 _ColorA;
                float4 _ColorB;
                float _Slide;
                float _Multiply;
                float _Alpha;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MoonTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float moon = tex2D(_MoonTex, i.uv).r;
                    float grad = saturate(i.uv.y + _Slide);
                    float3 col = lerp(_ColorB.rgb, _ColorA.rgb, grad) * _Multiply;
                    col += moon;
                    return fixed4(col, _Alpha);
                }
                ENDCG
            }
        }
}