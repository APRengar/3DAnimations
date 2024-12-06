Shader "Custom/TriplanarMapping_1"
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
            };

            sampler2D _MainTex;
            float _Tiling;

            // Функция для получения проекции текстуры на одну ось
            float4 GetTriplanarTexture(float3 worldPos, float3 normal, sampler2D tex)
            {
                float3 absNormal = abs(normal);

                // Проекции текстуры для каждой оси, с учетом переворота по оси Y для вертикальных граней
                float2 xProjection = worldPos.yz * _Tiling;
                float2 yProjection = worldPos.xz * _Tiling;
                float2 zProjection = worldPos.xy * _Tiling;

                // Коррекция ориентации текстуры на вертикальных осях
                // Например, на оси Z, если нормаль направлена по Z, мы поворачиваем текстуру
                if (absNormal.y > absNormal.x && absNormal.y > absNormal.z)
                {
                    yProjection = worldPos.xz * _Tiling;  // Оставляем без изменений, так как проекция по Y нормальна
                }
                else if (absNormal.x > absNormal.z)
                {
                    xProjection = worldPos.yz * _Tiling;  // Для оси X, используем нормальные координаты
                }
                else
                {
                    zProjection = worldPos.xy * _Tiling;  // Для оси Z корректируем проекцию
                }

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
                o.normal = normalize(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Используем треосную проекцию для текстуры с учетом ориентации нормалей
                float4 textureColor = GetTriplanarTexture(i.worldPos, i.normal, _MainTex);

                return textureColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}