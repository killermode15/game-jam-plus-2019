Shader "Unlit/Custom_Water"
{
    Properties
    {
		//[Header("Texture)]
		_Water("Water Texture", 2D) = "white" {}
		//[Header("Properties")]
		_WaveSpeed("Wave Speed", Range(0.1, 2)) = 1.0
		_WaveAmount("Wave Amount", Range(0.1, 2.0)) = 1.0
		_WaveHeight("Wave Height", Range(-1.0, 2.0)) = 1.0
		_WaterTint("Tint", Color) = (1,1,1,1)

		_FoamWidth ("Foam Width", Range(0.05, 0.5)) = 1.0
		_FoamTint ("Foam Tint", Color) = (1,1,1,1)

		_DistortMap("Caustics", 2D) = "white" {}
		_DistortStrength("Distort Strength", Range(0.1, 3)) = 1.0

    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
				float4 screenPos : TEXCOORD1;
				float4 worldPos : TEXCOORD2;
            };

			uniform sampler2D _CameraDepthTexture;
            sampler2D _Water;
            float4 _Water_ST;

			float4 _WaterTint;
			float _WaveSpeed, _WaveAmount, _WaveHeight;

			float _FoamWidth;
			float4 _FoamTint;

			float _DistortStrength;
			sampler2D _DistortMap;
			float4 _DistortMap_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex.y += sin(_Time.z * _WaveSpeed + (v.vertex.x * v.vertex.z * _WaveAmount)) * _WaveHeight ;
                o.uv = TRANSFORM_TEX(v.uv, _Water);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 distortion = tex2D(_DistortMap, (i.uv - _Time.x)) / (10 * _DistortStrength);

				half4 col = tex2D(_Water, i.uv + distortion) * _WaterTint;
				half depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
				
				half4 foamLine = 1 - saturate(_FoamWidth * (depth - i.screenPos.w));

				col += foamLine * _FoamTint;

                return col;
            }
            ENDCG
        }
    }
}
