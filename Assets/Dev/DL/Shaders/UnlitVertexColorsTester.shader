﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "OceansVR/Utility/VertexColorsTester"
{
	Properties
	{
		
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
				fixed4 color : COLOR; 
			};

			struct v2f
			{

				fixed4 color : COLOR; 

				UNITY_FOG_COORDS(2)
				float4 vertex : SV_POSITION;
			};

		
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				UNITY_TRANSFER_FOG(o,o.vertex);

				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				return i.color;
			}
			ENDCG
		}
	}
}
