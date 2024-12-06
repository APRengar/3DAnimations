Shader "Custom/WorldSpaceTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            sampler2D _MainTex;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // Мировые координаты
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Используем мировые координаты для текстуры
                float2 uv = i.worldPos.xz; // Привязка к XZ-плоскости
                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}