// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/guangquan" {
    Properties {
        _Color ("Surround Color", Color) = (1,0,0,1)
        _MainTex ("Base (RGB)", 2D) = "red" {}
        _Range("Range",Range(0.1,0.3))=0.15
    }
    SubShader {
    Tags{"RenderType"="Opaque"}
        //Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100
        
        Pass{
        //Tags{"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        //Blend One Zero
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            uniform float _Range;
            uniform float4 _Color;
            struct appdata_t
            {
                float4 vertex : POSITION;
                //half4 color : COLOR;
                //float2 texcoord : TEXCOORD0;
            };
            struct v2f
            {
                float4 vertex : POSITION;
                half4 color : COLOR;
                //float2 texcoord : TEXCOORD0;
                //float2 worldPos : TEXCOORD1;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                v.vertex.xyz+=_Range*v.vertex.xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = _Color;//v.color;
                //o.texcoord = v.texcoord;
                //o.worldPos = TRANSFORM_TEX(v.vertex.xy, _MainTex);
                return o;
            }

            half4 frag (v2f IN) : COLOR
            {
                return IN.color;
                // Sample the texture
        //        half4 col = IN.color;
        //        col.a *= tex2D(_MainTex, IN.texcoord).a;

        //        float2 factor = abs(IN.worldPos);
        //        float val = 1.0 - max(factor.x, factor.y);

                // Option 1: 'if' statement
        //        if (val < 0.0) col.a = 0.0;

                // Option 2: no 'if' statement -- may be faster on some devices
                //col.a *= ceil(clamp(val, 0.0, 1.0));

        //        return col;
            } 
            ENDCG
        }
        
        Pass{
            Tags{"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
            Blend One Zero
            Offset -1,-1
            ColorMaterial AmbientAndDiffuse
            
            SetTexture [_MainTex]
            {
                Combine Texture
            }
        }
    
    // third part
    } 
    //FallBack "Diffuse"
}
