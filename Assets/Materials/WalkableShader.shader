Shader "Custom/WalkableShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Ana doku
        _OverlayColor ("Overlay Color", Color) = (1, 0, 0, 0.5) // Transparan renk
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Transparanlık için blend ayarı
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex; // Doku
            fixed4 _OverlayColor; // Transparan renk

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0; // UV koordinatları
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0; // UV koordinatları
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Doku rengi alınıyor
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Doku ile transparan rengi karıştırıyoruz
                fixed4 finalColor = lerp(texColor, _OverlayColor, _OverlayColor.a);

                return finalColor;
            }
            ENDCG
        }
}
}
