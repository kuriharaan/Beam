Shader "Custom/SecurityLazerShader" {
	 Properties {
        _Color ("Color", Color) = (.34, .85, .92, 1)
		_Power ("Color power", float) = 2.0
    }
	

    SubShader {
        Tags {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
				
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

        CGPROGRAM
        #pragma surface surf Lambert alpha
		
        float4 _Color;
		float _Power;

        struct Input {
			float4 color;
			float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = _Color.rbg * _Power;
            //o.Alpha  = _Color.a 
			//* (frac((IN.worldPos.x + _SinTime.y) * 10) * 0.7 + 0.3)
			//* (frac(IN.worldPos.y * 100) * 0.4 + 0.6)
			//* (frac(IN.worldPos.z * 100) * 0.4 + 0.6);
			o.Alpha = _Color.a;
			//clip(frac(IN.worldPos.y * 100 + IN.worldPos.x * 20) + _SinTime.x * 2);
			//clip(frac(IN.worldPos.x + IN.worldPos.y * _SinTime.x) - 0.05 * _SinTime.z);
			clip( (frac((IN.worldPos.x - _Time.y) * 2) + frac((IN.worldPos.y - _Time.x)) * 2) * 0.5 - 0.3);
        }
        ENDCG
    }
}