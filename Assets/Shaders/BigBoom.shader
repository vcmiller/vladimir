// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_CameraToWorld' with 'unity_CameraToWorld'

Shader "Custom/BigBoom"
{
	Properties
	{
		_Res("Resolution", Int) = 64
		_Period("Period", Float) = 1
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
				float4 vertex : SV_POSITION;
				float4 wpos : COLOR0;
			};

			uint _Res;
			uint _Period;

			uniform float _StartTime;
			
			v2f vert (appdata v)
			{
				v2f o; 
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				float4x4 mat = unity_ObjectToWorld; 
				float4 vert = v.vertex;
				float4 wp = mul(mat, vert);
				o.wpos = wp;

				return o;
			}

			float rand(float3 co) {
				return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				float radius = (float)_Res * ((_Time.y - _StartTime) / _Period);
				//return float4(radius, 0, 0, 1);

				int x = i.uv.x * _Res;
				int y = i.uv.y * _Res;


				int2 p = int2(x, y);
				int2 c = int2(_Res / 2, _Res / 2);

				float d = distance(p, c);

				float f = clamp(d, 0, _Res / 2);
				f /= radius / 2;

				if ((d < radius / 2) && (rand(float3(x, y, 0)) < f)) {
					return fixed4(f * 0.7f, 1, f * 0.7f, 1);
				} else {
					discard;
					return fixed4(0, 1, 0, 0);
				}
				
			}
			ENDCG
		}
	}
}
