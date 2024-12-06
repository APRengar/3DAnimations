Shader "Custom/ToonShaderWithTexture"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {} // Добавлена текстура
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _LightColor ("Light Color", Color) = (1,1,1,1)
        _Ramp ("Ramp Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0; // Для UV-координат текстуры
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
                float2 uv : TEXCOORD1; // Передача UV-координат
            };

            sampler2D _MainTex;
            float4 _MainColor;
            float4 _LightColor;
            sampler2D _Ramp;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(v.normal);
                o.uv = v.uv; // Передача UV-координат во фрагментный шейдер
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Получение цвета текстуры
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Освещение
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float diff = max(0, dot(i.normal, lightDir));

                // Toon эффект с использованием градиентной текстуры
                fixed rampValue = tex2D(_Ramp, float2(diff, 0)).r;

                return texColor * _MainColor * rampValue * _LightColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
