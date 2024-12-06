Shader "Custom/TriplanarMapping_2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tiling ("Tiling", float) = 1.0
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 localPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float _Tiling;

            // Функция для получения проекции текстуры на одну ось
            float4 GetTriplanarTexture(float3 localPos, float3 normal, sampler2D tex)
            {
                float3 absNormal = abs(normal);

                // Проекции текстуры для каждой оси, с учетом переворота по оси Y для вертикальных граней
                float2 xProjection = localPos.yz * _Tiling;
                float2 yProjection = localPos.xz * _Tiling;
                float2 zProjection = localPos.xy * _Tiling;

                // Получаем текстуру по проекции
                float4 texX = tex2D(tex, xProjection);
                float4 texY = tex2D(tex, yProjection);
                float4 texZ = tex2D(tex, zProjection);

                // Веса для каждой оси
                float weightX = absNormal.x;
                float weightY = absNormal.y;
                float weightZ = absNormal.z;

                // Смешиваем результат по весам нормали
                return texX * weightX + texY * weightY + texZ * weightZ;
            }

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // Мировые координаты
                o.localPos = v.vertex.xyz;  // Локальные координаты объекта
                o.normal = normalize(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Используем треосную проекцию для текстуры с учетом локальных координат
                float4 textureColor = GetTriplanarTexture(i.localPos, i.normal, _MainTex);

                return textureColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}