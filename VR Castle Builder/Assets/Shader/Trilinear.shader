Shader "Trilinear" {

	//             Copyright 2023 Antonio Noack (1997, Jena/Germany)
	//                            Apache 2.0 License
	// Full text: https://github.com/AntonioNoack/RemsEngine/blob/master/LICENSE
	//                       Do not remove this license!

	Properties {

		_Color("Color", Color) = (1.0,1.0,1.0,1.0)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		
		_RoughnessMin("Roughness Min", Range(0.0,1.0)) = 0.0
		_RoughnessMax("Roughness Max", Range(0.0,1.0)) = 0.5
		_RoughnessMap("Roughness Map", 2D) = "white" {}

		_MetallicMin("Metallic Min", Range(0.0,1.0)) = 0.0
		_MetallicMax("Metallic Max", Range(0.0,1.0)) = 0.0
		_MetallicMap("Metallic Map", 2D) = "white" {}

		_BumpScale("Normal Strength", Float) = 1.0
		_BumpMap("Normal Map", 2D) = "bump" {}

		_BlendPower("Blend Power", Range(0.0, 5.0)) = 4.0
		_BlendY("Blend Y", Range(0.0,1.0)) = 0.625

		_LocalSpaceScale("Local Space Scale", Float) = 100.0
		[Toggle] _LocalSpace("Local Space", Float) = 0
		[Toggle] _CylinderProjection("Cylinder Projection", Float) = 1

	}
	SubShader {
		Tags {
			"RenderType"="Opaque"
			"PerformanceChecks"="False"
		}
		LOD 300
		
		CGPROGRAM
		#pragma target 3.0

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert fullforwardshadows nodynlightmap
		#include "UnityCG.cginc"
		
		// Instanced Properties
		// https://docs.unity3d.com/Manual/GPUInstancing.html
		UNITY_INSTANCING_BUFFER_START (MyProperties)
		UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
#define _Color_arr MyProperties
		UNITY_INSTANCING_BUFFER_END(MyProperties)

		// Non-instanced properties
		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _BumpMap;
		half _BumpScale;

		fixed _RoughnessMin, _RoughnessMax;
		sampler2D _RoughnessMap;

		fixed _MetallicMin, _MetallicMax;
		sampler2D _MetallicMap;

		fixed _BlendPower, _BlendY;
		float _LocalSpace, _LocalSpaceScale;
		float _CylinderProjection;
		
		struct Input {
			float3 worldPos;
			float3 normal;
			float3 vertex;
			float4 tangent;
			UNITY_VERTEX_INPUT_INSTANCE_ID	
		};

		half dot2(half3 a){
			return dot(a,a);
		}

		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.vertex = v.vertex;
			o.normal = v.normal;
			o.tangent = v.tangent;
		}

		float3 getMatrixScale(float3x3 m){
			float lx = length(float3(m._m00, m._m01, m._m02));
			float ly = length(float3(m._m10, m._m11, m._m12));
			float lz = length(float3(m._m20, m._m21, m._m22));
			return float3(lx,ly,lz);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
		
			fixed3 blendNormal;
			half3 objectScale;
			if(_LocalSpace) {
				blendNormal = normalize(IN.normal);
			} else {
				half3x3 m = (half3x3)UNITY_MATRIX_M;
				objectScale = half3(
					dot2(half3(m[0][0], m[1][0], m[2][0])),
					dot2(half3(m[0][1], m[1][1], m[2][1])),
					dot2(half3(m[0][2], m[1][2], m[2][2]))
				);
				blendNormal = normalize(mul(unity_ObjectToWorld, fixed4(IN.normal / (objectScale*objectScale), 0.0)));
			}

			float3 weights = 0;
			if(_CylinderProjection){
				// weights for sides only
				float2 xzBlend = abs(normalize(blendNormal.xz));
				weights.xz = max(0.0, xzBlend - 0.675);
				weights.xz /= max(0.00001, dot(weights.xz, float2(1,1)));
				// weights for top
				weights.y = saturate((abs(blendNormal.y) - _BlendY) * exp(_BlendPower));
				weights.xz *= (1.0 - weights.y);
			} else {
				weights = pow(abs(blendNormal), 1.0 + _BlendPower * 10.0);
				weights /= dot(weights, float3(1,1,1));
			}

			float3 pos = _LocalSpace ? IN.vertex * _LocalSpaceScale : IN.worldPos;			
			float2 posX = pos.zy;
			float2 posY = pos.xz;
			float2 posZ = float2(-pos.x, pos.y);	

			float2 xUV = posX * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 yUV = posY * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 zUV = posZ * _MainTex_ST.xy + _MainTex_ST.zw;

			// ALBEDO

			fixed4 tex = tex2D(_MainTex, xUV) * weights.x + tex2D(_MainTex, yUV) * weights.y + tex2D(_MainTex, zUV) * weights.z;
			// instance colors, could be disabled
			fixed4 tint = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);
			o.Albedo = max(fixed3(0.0, 0.0, 0.0), tex.rgb * tint.rgb);
			o.Alpha = tex.a * tint.a;
			
			// NORMAL

			fixed3 bumpX = UnpackScaleNormal(tex2D(_BumpMap, xUV), _BumpScale * weights.x);
			fixed3 bumpY = UnpackScaleNormal(tex2D(_BumpMap, yUV), _BumpScale * weights.y);
			fixed3 bumpZ = UnpackScaleNormal(tex2D(_BumpMap, zUV), _BumpScale * weights.z);
			
			if (_LocalSpace) {
				o.Normal = bumpX * weights.x + bumpY * weights.y + bumpZ * weights.z;
			} else {
				float3 scale = sqrt(objectScale);
				float3 tmpNormal = mul(unity_ObjectToWorld, float4(IN.normal / scale, 0.0));
				blendNormal =
					abs(tmpNormal.x) * float3(sign(tmpNormal.x) * bumpX.z, bumpX.y, bumpX.x) +
					abs(tmpNormal.y) * float3(bumpY.x, sign(tmpNormal.y) * bumpY.z, bumpY.y) +
					abs(tmpNormal.z) * float3(-bumpZ.x, bumpZ.y, sign(tmpNormal.z) * bumpZ.z);
				float3 nor = normalize(IN.normal), tan = normalize(IN.tangent.xyz);
				float3 binormal = cross(nor, tan) * IN.tangent.w;
				float3x3 defaultNTB = float3x3(tan, binormal, nor);
				// here I would have expected to have to use transpose(defaultNTB), but it looks like the non-transpose is correct; a transformation looks "correct", when all surfaces (setting o.Albedo = o.Normal) are blue and testing shows the correct behaviour for light
				o.Normal = normalize(mul(defaultNTB, mul(unity_WorldToObject, float4(blendNormal, 0.0)).xyz * scale));
			}

			// METALLIC & GLOSSY
			
			o.Metallic = lerp(_MetallicMin, _MetallicMax, dot(weights, float3(tex2D(_MetallicMap, xUV).r, tex2D(_MetallicMap, yUV).r, tex2D(_MetallicMap, zUV).r)));			  
			o.Smoothness = 1.0 - lerp(_RoughnessMin, _RoughnessMax, dot(weights, float3(tex2D(_RoughnessMap, xUV).r, tex2D(_RoughnessMap, yUV).r, tex2D(_RoughnessMap, zUV).r)));

		}
		ENDCG
	} 
	FallBack "Diffuse"
	
}
