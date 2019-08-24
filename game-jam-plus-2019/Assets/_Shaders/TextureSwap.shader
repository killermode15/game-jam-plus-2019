Shader "Unlit/TextureSwap"
{
    Properties
    {
		[Header(Textures)]
        _MainTex ("Main Texture", 2D) = "white" {}
		_Tint("Tint", Color) = (1,1,1,1)
		_SwapTex("Swap Texture", 2D) = "white" {}
		_SwapTint("Swap Tint", Color) = (1,1,1,1)

		[Space(50)]
		[Header(Properties)]
		_Radius("Radius", float) = 1
		_LineWidth("Line Width", float) = 1
		

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
				float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _SwapTex;
            float4 _MainTex_ST, _SwapTex_ST;
			float4 _Tint, _SwapTint;

			float3 _Position[50];
			float _Radius;

            v2f vert (appdata v)
            {
				for (int i = 0; i < 50; i++)
				{
					_Position[i] = float3(-9999, -9999, -9999);
				}

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				if (_Radius < 0)
				{
					_Radius = 0;
				}

				for (int j = 0; j < 50; j++)
				{
					_Position[j] = float3(-9999, -9999, -9999);
				}
                // sample the texture
				float3 tex1 = tex2D(_MainTex, i.uv);
				float3 tex2 = tex2D(_SwapTex, i.uv) * _SwapTint;
				float4 finalTex = float4(tex1, 1);

				for (int j = 0; j < 50; j++)
				{
					float3 dis = distance(_Position[j], i.worldPos);
					float pos = 1 - saturate(dis / _Radius);

					//float3 primaryTex = step(pos, 0.1) * tex1.rgb;
					float3 swapTex = step(0.1, pos) * float4(1,1,1,1);// * tex2.rgb;
					finalTex -= float4(swapTex, 1) * float4(tex1.rgb, 1);
					finalTex += float4(swapTex, 1) * float4(tex2.rgb, 1);
					//finalTex += float4(swapTex * tex2.rgb, 1);
					//finalTex += float4(saturate(swapTex), 1);
					saturate(finalTex);
				}
				

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, finalTex);
                return finalTex;
            }
            ENDCG
        }
    }
}
