Shader "Unlit/Waterfall"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_WaterSpeed ("WaterSpeed", float) = 1
		_WaterForce ("WaterForce", float) = 1	
		_WaterDropOff ("WaterDropOff", Range(2,32)) = 1
		_WaterBend ("WaterBend", Range(0,4)) = 1
	}
	SubShader
	{
		Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" "IgnoreProjector"="True" }
		LOD 100


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
			float _WaterForce;
			float _WaterDropOff;
			float _WaterBend;

			v2f vert (appdata v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				fixed4 vert = v.vertex;
			
				// -(pow(2,(.8-o.uv.y) * 5))
				// + (.8-(pow(x+(_WaterBend * .05),_WaterBend+x))*15)
				//(-pow(o.uv.x-.5,2) * 10) +
				float x = (1-o.uv.y) * 1;
				vert.y = -(x*.1* pow(_WaterDropOff, x) * 20);
				vert.yz += (-pow(o.uv.x-.5,2) * 10 * (_WaterDropOff/32) * _WaterBend);
				vert.z += 5;

				vert.yz /= (_WaterDropOff*0.1);
				//vert.y = min(vert.y, 20);
				o.vertex = UnityObjectToClipPos(vert);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
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
