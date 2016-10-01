// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/Flare"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Height ("height",float) = 0
	}
	SubShader
	{
		Tags { "Queue" = "Transparent+1" "RenderType"="Transparent" }
		Blend One One
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
			float _Height;
			
			v2f vert (appdata v)
			{
				v2f o;
//				mat4x4 v = mul( unity_ObjectToWorld, v.vertex );
//				 float tmp = _World2Object[3][3];
				 float4x4 I_Object2World = unity_WorldToObject ;
//				 I_Object2World[3][3] = tmp;
				 v.vertex = mul(I_Object2World, v.vertex);

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);// + mul( unity_WorldToObject, float4( 0,_Height,0,0) ));
//			    o.vertex += mul( unity_WorldToObject, float4( 0,0,0,1 ) );
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col*col.a;
			}
			ENDCG
		}
	}
}
