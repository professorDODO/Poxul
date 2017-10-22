// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Projector/TextureHole" 
{	
	Properties
	{
		_Cutout ("Cutout", Range (0, 1)) = 0.5
		_HoleTex ("Hole Texture", 2D) = ""
		_MainTex ("Main Texture", 2D) = ""
	}

SubShader
{
	Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "LightMode"="ForwardBase"}
    Blend SrcAlpha OneMinusSrcAlpha

	Pass
	{
		CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

		#include "UnityCG.cginc"
		#include "UnityLightingCommon.cginc"

        struct appdata
        {
            float4 vertex : POSITION; 
            float2 uv : TEXCOORD1;
			float3 normal : NORMAL;
        };

       	struct v2f 
		{
			float4 vertex : SV_POSITION;
			fixed4 diff : COLOR0; 

			float2 uv : TEXCOORD0;
			float4 uvShadow : TEXCOORD1;
			//float4 uvFalloff : TEXCOORD2;
		};

		struct fragmentOutput
        {
            float4 color : SV_Target;
        };

		float _Cutout;

		sampler2D _MainTex;
		sampler2D _HoleTex;

		float4x4 unity_Projector;
		//float4x4 unity_ProjectorClip;

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);

            o.uv = v.uv;
			o.uvShadow = mul(unity_Projector, v.vertex);
			//o.uvFalloff = mul(unity_ProjectorClip, v.vertex);

			half3 worldNormal = UnityObjectToWorldNormal(v.normal);
			half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
			o.diff = nl * _LightColor0;

            return o;
        }
           

        fragmentOutput frag (v2f i)
        {
            fragmentOutput o;
			
			clip(tex2D(_HoleTex, i.uvShadow).r - _Cutout);

			o.color = tex2D(_MainTex, i.uv);
			o.color *= i.diff;
			o.color.a = 1;

            return o;
        }
        ENDCG
	}   
}

}
