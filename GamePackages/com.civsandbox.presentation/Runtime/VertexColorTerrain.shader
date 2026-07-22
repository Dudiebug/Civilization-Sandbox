Shader "CivSandbox/Vertex Color Terrain"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass
        {
            Tags { "LightMode"="ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                fixed4 color : COLOR;
            };

            v2f vert(appdata input)
            {
                v2f output;
                output.position = UnityObjectToClipPos(input.vertex);
                output.worldNormal = UnityObjectToWorldNormal(input.normal);
                output.color = input.color;
                return output;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                float3 normal = normalize(input.worldNormal);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float diffuse = saturate(dot(normal, lightDirection));
                float lighting = 0.46 + diffuse * 0.54;
                return fixed4(input.color.rgb * _LightColor0.rgb * lighting, 1.0);
            }
            ENDCG
        }
    }

    Fallback "Diffuse"
}
