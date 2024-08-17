Shader "Custom/GridShader"
{
    Properties
    {
        _Color("Line Color", Color) = (1, 1, 1, 1)
        _Thickness("Line Thickness", Float) = 1.0
        _GridSize("Grid Size", Float) = 10.0
        _MaskTex("Mask Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _GridSize;
            float _Thickness;
            half4 _Color;
            sampler2D _MaskTex;

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float2 gridUV = i.uv * _GridSize;
                float2 grid = abs(frac(gridUV - 0.5) - 0.5) / fwidth(gridUV);
                float line1 = min(grid.x, grid.y);
                line1 = smoothstep(_Thickness - 0.5, _Thickness + 0.5, line1);

                half4 mask = tex2D(_MaskTex, i.uv);
                return half4(_Color.rgb, _Color.a * (1.0 - line1) * mask.r);
            }
            ENDHLSL
        }
    }
}
