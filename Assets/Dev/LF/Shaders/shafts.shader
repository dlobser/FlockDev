// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "OceansVR/Effect/shafts"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Shadow ("Shadow", Float) = 1
		_Tile ("Scale", Float) = 12

		_ShadowTex ("Shadow", 2D) = "white" {}
		_ShadowTexMult ("shading", Color) = (1,1,1,1)
    }
    SubShader
    {
        Pass
        {

			Tags { "Queue" = "Transparent" "RenderType"="Transparent"} 

			ZWrite Off // don't write to depth buffer 
			Cull Off
			Blend One One // use alpha blending
			LOD 100

        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct appdata
			{
				float4 vertex : POSITION;
				float3 normal: NORMAL;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
            	fixed4 diff : TANGENT;
            	fixed4 color : COLOR;

			};

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                fixed4 diff : TANGENT;
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _MainTex_ST;

            sampler2D _ShadowTex;
			fixed4 _ShadowTex_ST;
            float4 _Color;
            float _Shadow;
            fixed4 _ShadowTexMult;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex + float4(sin(v.vertex.x*2+_Time.z)*.05*v.vertex.y,v.vertex.y*sin(v.vertex.y+_Time.z)*.05,0,0) );//UnityObjectToClipPos(v.vertex);

//                float4 v2 = float4(0,sin(o.vertex.y+_Time.z),0,0);
				o.vertex = o.vertex;//+v2;// mul(UNITY_MATRIX_MVP,vert );

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
               	o.uv2 = TRANSFORM_TEX(v.uv2, _ShadowTex);

               	o.color = v.color;

                float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float nl = max(0, min(1.0,(dot(normalize(v.normal), viewDir ))));
                o.diff =nl;//nl;//(min(1.0,max(0.0,nl)*1.) );


//
//                nl = max(0, dot(worldNormal*-1, _WorldSpaceLightPos0.xyz))+.1;
//                o.diff2 = min(1.0,max(0.0,lerp(_Shadow,1.0,nl)*5.));

//                 v2f o;
//                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
//                o.uv = v.texcoord;
//                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
//                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
//                o.diff = .25+(min(1.0,max(0.0,nl)*5.) * _LightColor0);

                // the only difference from previous shader:
                // in addition to the diffuse lighting from the main light,
                // add illumination from ambient or light probes
                // ShadeSH9 function from UnityCG.cginc evaluates it,
                // using world space normal
//                o.diff.rgb += ShadeSH9(half4(worldNormal,1));
                return o;
            }
            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 shadow = tex2D(_ShadowTex, i.uv2 + float2(0,_Time.x *.4));
                float shad = shadow.r-.3;
//                col *= i.diff;
//                col.a = i.diff.x;
                return  i.diff * i.color.b * _Color * shad*shad;//col * _Color * _Color.a*5. * lerp(_ShadowTexMult,float4(1,1,1,1),i.color.b);//fixed4(col.rgb,i.diff.x);
            }
            ENDCG
        }
    }
}