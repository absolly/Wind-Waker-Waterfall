Shader "Unlit/Ocean"
{
	Properties
	{
		_BaseColor ("BaseColor", Color) = (0,0,0,0)
		_FoamColor ("FoamColor", Color) = (0,0,0,0)
		_MainTex ("Texture", 2D) = "white" {}
		_LayerSpeed ("Foam layer speed", Vector) = (0,0,0,0)
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
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _BaseColor;
			float4 _FoamColor;
			fixed4 _LayerSpeed;

			float blendOverlay(float base, float blend) {
				return base<0.5?(2.0*base*blend):(1.0-2.0*(1.0-base)*(1.0-blend));
			}

			float3 blendOverlay(float3 base, float3 blend) {
				return float3(blendOverlay(base.r,blend.r),blendOverlay(base.g,blend.g),blendOverlay(base.b,blend.b));
			}

			float3 blendOverlay(float3 base, float3 blend, float opacity) {
				return (blendOverlay(base, blend) * opacity + base * (1.0 - opacity));
			}


			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float4 col1 = tex2D(_MainTex, 2 * fixed2(i.uv.x + _LayerSpeed.x * _Time.y, i.uv.y + _LayerSpeed.y * _Time.y));
				float4 col2 = tex2D(_MainTex, fixed2(i.uv.x + _LayerSpeed.z * _Time.y, i.uv.y + _LayerSpeed.w * _Time.y));
				float foamIntensity = 0.5 * col1.x + col2.x;
				fixed4 col = _BaseColor + (foamIntensity * _FoamColor);
				// apply fog	
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
