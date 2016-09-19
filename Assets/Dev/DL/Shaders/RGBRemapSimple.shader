Shader "Holojam/RGBRemapSimple"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ColorR ("Color:R",Color) = (0,0,0,0)
		_ColorG ("Color:G",Color) = (0,0,0,0)
		_ColorB ("Color:B",Color) = (0,0,0,0)
		_ColorMult("Color Multiply",Color) = (1,1,1,1)
		_InHueShift("In Hue Shift",Vector) = (0,0,0,0)
		_OutHueShift("Out Hue Shift",Vector) = (0,0,0,0)
		_Saturation("Saturation",Float) =1
		_InSpeed("In Hue Speed",Vector) = (0,0,0,0)
		_OutSpeed("Out Hue Speed",Vector) =(0,0,0,0)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
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
			float4 _ColorR;
			float4 _ColorG;
			float4 _ColorB;
			float4 _ColorMult;
			float4 _InHueShift;
			float4 _OutHueShift;
			float _Saturation;
			float4 _InSpeed;
			float4 _OutSpeed;

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			float r(float i){
				return (1+i)*.5;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
//
//				float3 hsv = rgbToHsv(col.xyz);
//                hsv.x += _InHueShift + _Time.x * _InSpeed;
// 				hsv.x = frac(hsv.x);
//                fixed4 inShifted = fixed4( half3(hsvToRgb(hsv) ), col.a);
//
                float4 off = _InHueShift + _Time.y * _InSpeed * _InSpeed.a;
                fixed4 inShifted = fixed4(r(sin(col.x+off.x))*col.r,r(sin(col.y+off.y))*col.g,r(sin(col.z+off.z))*col.b,1.0);

				fixed4 colR = _ColorR * inShifted.x;
				fixed4 colG = _ColorG * inShifted.y;
				fixed4 colB = _ColorB * inShifted.z;
				fixed4 col2 = colR+colG+colB;
				col2.a = 1;
//				hsv = rgbToHsv(col2.xyz);
//                hsv.x += _OutHueShift + _Time.x * _OutSpeed;
// 				hsv.x = frac(hsv.x);
// 				hsv.y*=_Saturation;
//                fixed4 outShifted = fixed4( half3(hsvToRgb(hsv) ), col.a)*_ColorMult;
//
                off = _OutHueShift + _Time.y * _OutSpeed * _OutSpeed.a;
                off.a = 1;
                fixed4 outShifted = fixed4(r(sin(col2.x+off.x))*col.r,r(sin(col2.y+off.y))*col.g,r(sin(col2.z+off.z))*col.b,col.a);


				outShifted.a = col.a;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, outShifted);
				return col2;
			}
			ENDCG
		}
	}
}
