Shader "Custom/TransparentColorShader" {

	 Properties {
        _Color ("Color", Color) = (.34, .85, .92, 1)
    }

    SubShader {

        Pass {
            Tags { "RenderType" = "Transparent" "Queue"="Transparent" }
            Blend SrcAlpha One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 vert(float4 v:POSITION) : SV_POSITION {
                return mul (UNITY_MATRIX_MVP, v);
            }

			
            float4 _Color;
            fixed4 frag() : SV_Target {
                return _Color;
            }
            ENDCG
        }
    }
}