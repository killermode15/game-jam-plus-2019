Shader "Custom/Lighting"
{
	Properties
	{
		_SunRadius("Sun Radius", float) = 0.04
		_MoonRadius("Moon Radius", float) = 0.04
		_MoonOffset("Moon Offset", float) = 0.1

		_DayTopColor("Day Top Color", Color) = (1,1,1,1)
		_DayBottomColor("Day Bottom Color", Color) = (1,1,1,1)

		_NightTopColor("Night Top Color", Color) = (1,1,1,1)
		_NightBottomColor("Night Bottom Color", Color) = (1,1,1,1)

		_HorizonColorDay("Horizon Color Day", Color) = (1,1,1,1)
		_HorizonColorNight("Horizon Color Night", Color) = (1,1,1,1)

		_Stars("Star Texture", 2D) = "white"

		_CloudCutoff("Cloud Cutoff",  float) = 0.3
		_Fuzziness("Fuzziness", float) = 0.04

		_SkyScrollSpeed ("Sky Scroll Speed", float) = 0.12
		_CloudScale("Cloud Scale", float) = 0.5
		_BaseNoise ("Base Noise", 2D) = "white"
		_DistortNoise("Distort Noise", 2D) = "white"
		_SecondaryNoise ("Secondary Noise", 2D) = "white"

	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		Pass
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag


			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 uv : TEXCOORD0;
			};

			struct VertexOutput
			{
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float3 uv : TEXCOORD0;
			};

			float _SunRadius;
			float _MoonRadius;
			float _MoonOffset;

			float4 _DayTopColor, _DayBottomColor;
			float4 _NightTopColor, _NightBottomColor;
			float4 _HorizonColorDay;
			float4 _HorizonColorNight;

			sampler2D _Stars;

			sampler2D _BaseNoise, _DistortNoise, _SecondaryNoise;
			float _SkyScrollSpeed, _CloudScale;
			float _CloudCutoff, _Fuzziness;

			VertexOutput vert(VertexInput v)
			{
				VertexOutput o;
				o.uv = v.uv;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			float4 frag(VertexOutput o) : SV_Target
			{
				// Sun
				float sun = distance(o.uv.xyz, _WorldSpaceLightPos0);
				float sunDisc = 1 - saturate(sun / _SunRadius);
				sunDisc = saturate(sunDisc * 50);

				// Sun Sky Color
				float4 gradientDay = lerp(_DayBottomColor, _DayTopColor, saturate(o.uv.y));
				// Moon Sky Color
				float4 gradientNight = lerp(_NightBottomColor, _NightTopColor, saturate(o.uv.y));

				float4 gradientSky = lerp(gradientNight, gradientDay, saturate(_WorldSpaceLightPos0.y));


				// Moon
				float moon = distance(o.uv.xyz, -_WorldSpaceLightPos0);
				float crescentMoon = distance(float3(o.uv.x + _MoonOffset, o.uv.yz), -_WorldSpaceLightPos0);
				float crescentMoonDisc = 1 - (crescentMoon / _MoonRadius);
				crescentMoonDisc = saturate(crescentMoonDisc * 50);

				float moonDisc = 1 - (moon / _MoonRadius);
				moonDisc = saturate(moonDisc * 50);
				float newMoonDisc = saturate(moonDisc - crescentMoonDisc);


				// Horizon Day
				float horizon = abs(o.uv.y);
				float4 horizonGlowDay = saturate((1 - horizon * 3) * saturate(_WorldSpaceLightPos0.y) * .0625) * _HorizonColorDay;

				// Horizon Day
				float4 horizonGlowNight = saturate((1 - horizon * 3) * saturate(-_WorldSpaceLightPos0.y) * 1) * _HorizonColorNight;

				float4 horizonGlow = horizonGlowDay + horizonGlowNight;

				// Stars
				float2 uvSky = o.uv.xz / o.uv.y;
				float4 stars = tex2D(_Stars, uvSky - _Time.x * .18);
				stars *= saturate(-_WorldSpaceLightPos0.y);
				stars = step(.55, stars);

				// Clouds
				uvSky = o.uv.xz / (o.uv.y);
				float scrollSpeed = _SkyScrollSpeed * _Time.x;
				float baseNoise = tex2D(_BaseNoise, (uvSky - _Time.x) * _CloudScale * .2);

				float noise1 = tex2D(_DistortNoise, ((uvSky + baseNoise) - scrollSpeed) * _CloudScale * .1);

				float noise2 = tex2D(_SecondaryNoise, ((uvSky + (noise1 * 0.1)) - scrollSpeed * _CloudScale) * 0.5);

				float finalNoise = saturate(noise1 * noise2) * 3 * saturate(o.worldPos.y);
				float clouds = saturate(smoothstep(_CloudCutoff, _CloudCutoff + _Fuzziness, finalNoise));
				//return baseNoise;
				//return finalNoise;

				return (sunDisc + newMoonDisc) + gradientSky + horizonGlow + stars + clouds;
			}
			ENDCG
		}
	}
}
