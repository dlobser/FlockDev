// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/TextureTest" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Color ("Color",Color)= (1,1,1,1)
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				fixed4 col2 = col*_Color;
				fixed4 colR = fixed4(col2.r,col2.r,col2.r,1.0);
				fixed4 colG = fixed4(col2.g,col2.g,col2.g,1.0);
				fixed4 colB = fixed4(col2.b,col2.b,col2.b,1.0);
				fixed4 col3 = colR+colG+colB;
				UNITY_APPLY_FOG(i.fogCoord, col);
				UNITY_APPLY_FOG(i.fogCoord, col3);
				UNITY_OPAQUE_ALPHA(col.a);
				return col3;
			}
		ENDCG
	}
}

}
