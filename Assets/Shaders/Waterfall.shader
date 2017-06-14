Shader "Unlit/Waterfall"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_WaterSpeed ("WaterSpeed", float) = 1
		_WaterDropOff ("WaterDropOff", Range(2,32)) = 10
		_WaterBend ("WaterBend", Range(0,80)) = 30
		_WaterfallLength ("WaterfallLength", float) = 10
	}
	SubShader
	{
		//Alpha cutout
		Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" "IgnoreProjector"="True" }
		LOD 100

		//the waterfall planes should be visible on both sides 
		Cull Off
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
			float _WaterSpeed;
			float _WaterDropOff;
			float _WaterBend;
			float _WaterfallLength;

			v2f vert (appdata v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//store the vertex in a temp variable before we change it
				fixed4 vert = v.vertex;

				//make the waterfall go in the z+ direction
				float z = (1-o.uv.y);
				//waterfall dropoff formula
				vert.y = -(z * pow(_WaterDropOff, z) * 2);

				//offset the mesh before scaling so we scale in one direction
				vert.z += 5;
				//the waterfall should be longer if the dropoff is smaller otherwise it won't hit the water.
				vert.yz *= (_WaterfallLength/_WaterDropOff);

				//add the waterbend after the scaling because we don't want the bend to scale with it
				//Waterbend, x^2 * (WaterDropoff/MaxWaterDropoff) * WaterBend param
				vert.yz -= pow(o.uv.x-.5,2) * _WaterBend;

				o.vertex = UnityObjectToClipPos(vert);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//Scrolling water texture
				fixed4 col = tex2D(_MainTex, i.uv + float2(0,_Time.y * _WaterSpeed));
				clip(col.a - 0.9);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
