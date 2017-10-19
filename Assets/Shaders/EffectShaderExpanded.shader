Shader "Custom/EffectShaderExpanded"
{
	Properties
	{ 
		//[Toggle(REDIFY_ON)] _Redify("Red?", Int) = 0
		_MainTex("Main Texture", 2D) = "black"{}
		_DistTex("Distortion Texture", 2D) = "grey"{}
		_DistMask("Distortion Mask", 2D) = "black"{}
		_AlphaValue("Alpha", float) = 0
		_ScrollXSpeed("X Scroll Speed", float) = 2
		_ScrollYSpeed("Y Scroll Speed", float) = 2


		_EffectsLayer1Tex("", 2D) = "black"{}
		_EffectsLayer1Color("", Color) = (1,1,1,1)
		_EffectsLayer1Motion("", 2D) = "black"{}
		_EffectsLayer1DistTex("", 2D) = "grey"{}
		_EffectsLayer1DistMask("", 2D) = "black"{}
		_EffectsLayer1DoDistort("", float) = 0
		_EffectsLayer1MotionSpeedYAxis("", float) = 0
		_EffectsLayer1MotionSpeedXAxis("", float) = 0
		_EffectsLayer1ScrollSpeedXAxis("", float) = 0
		_EffectsLayer1ScrollSpeedYAxis("", float) = 0
		_EffectsLayer1Rotation("", float) = 0
		_EffectsLayer1PivotScale("", Vector) = (0.5,0.5,1,1)
		_EffectsLayer1Translation("", Vector) = (0,0,0,0)
		_EffectsLayer1Foreground("", float) = 0


		_EffectsLayer2Tex("", 2D) = "black"{}
		_EffectsLayer2Color("", Color) = (1,1,1,1)
		_EffectsLayer2Motion("", 2D) = "black"{}
		_EffectsLayer2DistTex("", 2D) = "grey"{}
		_EffectsLayer2DistMask("", 2D) = "black"{}
		_EffectsLayer2DoDistort("", float) = 0
		_EffectsLayer2MotionSpeedYAxis("", float) = 0
		_EffectsLayer2MotionSpeedXAxis("", float) = 0
		_EffectsLayer2ScrollSpeedXAxis("", float) = 0
		_EffectsLayer2ScrollSpeedYAxis("", float) = 0
		_EffectsLayer2Rotation("", float) = 0
		_EffectsLayer2PivotScale("", Vector) = (0.5,0.5,1,1)
		_EffectsLayer2Translation("", Vector) = (0,0,0,0)
		_EffectsLayer2Foreground("", float) = 0


		_EffectsLayer3Tex("", 2D) = "black"{}
		_EffectsLayer3Color("", Color) = (1,1,1,1)
		_EffectsLayer3Motion("", 2D) = "black"{}
		_EffectsLayer3DistTex("", 2D) = "grey"{}
		_EffectsLayer3DistMask("", 2D) = "black"{}
		_EffectsLayer3DoDistort("", float) = 0
		_EffectsLayer3MotionSpeedYAxis("", float) = 0
		_EffectsLayer3MotionSpeedXAxis("", float) = 0
		_EffectsLayer3ScrollSpeedXAxis("", float) = 0
		_EffectsLayer3ScrollSpeedYAxis("", float) = 0
		_EffectsLayer3Rotation("", float) = 0
		_EffectsLayer3PivotScale("", Vector) = (0.5,0.5,1,1)
		_EffectsLayer3Translation("", Vector) = (0,0,0,0)
		_EffectsLayer3Foreground("", float) = 0
	}
		SubShader{
		Tags{ "Queue" = "Transparent"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane" }
		LOD 200
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha


		Pass
	{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#pragma shader_feature EFFECTLAYER1OFF EFFECTLAYER1ON
#pragma shader_feature EFFECTLAYER2OFF EFFECTLAYER2ON
#pragma shader_feature EFFECTLAYER3OFF EFFECTLAYER3ON

#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		float2 uv : TEXCOORD0;
#if EFFECTLAYER1ON
		float2 effect1uv : TEXCOORD1;
#endif // EFFECTLAYER1ON
#if EFFECTLAYER2ON
		float2 effect2uv : TEXCOORD2;
#endif // EFFECTLAYER2ON
#if EFFECTLAYER3ON
		float2 effect3uv : TEXCOORD3;
#endif // EFFECTLAYER3ON


	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	sampler2D _DistTex;
	sampler2D _DistMask;

	float _AlphaValue; 
	float _ScrollXSpeed;
	float _ScrollYSpeed;

	sampler2D	_EffectsLayer1Tex;
	float4		_EffectsLayer1Tex_ST;
	sampler2D	_EffectsLayer1Motion;
	sampler2D	_EffectsLayer1DistTex;
	sampler2D	_EffectsLayer1DistMask;
	float		_EffectsLayer1DoDistort;
	float		_EffectsLayer1MotionSpeedYAxis;
	float		_EffectsLayer1MotionSpeedXAxis;
	float		_EffectsLayer1ScrollSpeedXAxis;
	float		_EffectsLayer1ScrollSpeedYAxis;
	float		_EffectsLayer1Rotation;
	float4		_EffectsLayer1PivotScale;
	half4		_EffectsLayer1Color;
	float		_EffectsLayer1Foreground;
	float2		_EffectsLayer1Translation;

	sampler2D	_EffectsLayer2Tex;
	float4		_EffectsLayer2Tex_ST;
	sampler2D	_EffectsLayer2Motion;
	sampler2D	_EffectsLayer2DistTex;
	sampler2D	_EffectsLayer2DistMask;
	float		_EffectsLayer2DoDistort;
	float		_EffectsLayer2MotionSpeedYAxis;
	float		_EffectsLayer2MotionSpeedXAxis;
	float		_EffectsLayer2ScrollSpeedXAxis;
	float		_EffectsLayer2ScrollSpeedYAxis;
	float		_EffectsLayer2Rotation;
	float4		_EffectsLayer2PivotScale;
	half4		_EffectsLayer2Color;
	float		_EffectsLayer2Foreground;
	float2		_EffectsLayer2Translation;

	sampler2D	_EffectsLayer3Tex;
	float4		_EffectsLayer3Tex_ST;
	sampler2D	_EffectsLayer3Motion;
	sampler2D	_EffectsLayer3DistTex;
	sampler2D	_EffectsLayer3DistMask;
	float		_EffectsLayer3DoDistort;
	float		_EffectsLayer3MotionSpeedYAxis;
	float		_EffectsLayer3MotionSpeedXAxis;
	float		_EffectsLayer3ScrollSpeedXAxis;
	float		_EffectsLayer3ScrollSpeedYAxis;
	float		_EffectsLayer3Rotation;
	float4		_EffectsLayer3PivotScale;
	half4		_EffectsLayer3Color;
	float		_EffectsLayer3Foreground;
	float2		_EffectsLayer3Translation;


	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
		o.uv.x += _Time.x * _ScrollXSpeed;
		o.uv.y += _Time * _ScrollYSpeed;
		float2x2 rotationMatrix;
		float sinTheta;
		float cosTheta;

#if EFFECTLAYER1ON
		o.effect1uv = o.uv - _EffectsLayer1PivotScale.xy;

		o.effect1uv.x += _Time.x * _EffectsLayer1ScrollSpeedXAxis;
		o.effect1uv.y += _Time.x * _EffectsLayer1ScrollSpeedYAxis;

		sinTheta = sin(_EffectsLayer1Rotation * _Time);
		cosTheta = cos(_EffectsLayer1Rotation * _Time);
		rotationMatrix = float2x2(cosTheta, -sinTheta, sinTheta, cosTheta);
		o.effect1uv = (mul((o.effect1uv - _EffectsLayer1Translation.xy) *
			(1 / _EffectsLayer1PivotScale.zw), rotationMatrix) + _EffectsLayer1PivotScale.xy)
			* _EffectsLayer1Tex_ST.xy + _EffectsLayer1Tex_ST.zw;
#endif // EFFECTLAYER1ON
#if EFFECTLAYER2ON
		o.effect2uv = o.uv - _EffectsLayer2PivotScale.xy;

		o.effect2uv.x += _Time.x * _EffectsLayer2ScrollSpeedXAxis;
		o.effect2uv.y += _Time.x * _EffectsLayer2ScrollSpeedYAxis;

		sinTheta = sin(_EffectsLayer2Rotation * _Time);
		cosTheta = cos(_EffectsLayer2Rotation * _Time);
		rotationMatrix = float2x2(cosTheta, -sinTheta, sinTheta, cosTheta);
		o.effect2uv = (mul((o.effect2uv - _EffectsLayer2Translation.xy) *
			(1 / _EffectsLayer2PivotScale.zw), rotationMatrix) + _EffectsLayer2PivotScale.xy)
			* _EffectsLayer2Tex_ST.xy + _EffectsLayer2Tex_ST.zw;
#endif // EFFECTLAYER2ON
#if EFFECTLAYER3ON
		o.effect3uv = o.uv - _EffectsLayer3PivotScale.xy;

		o.effect3uv.x += _Time.x * _EffectsLayer3ScrollSpeedXAxis;
		o.effect3uv.y += _Time.x * _EffectsLayer3ScrollSpeedYAxis;

		sinTheta = sin(_EffectsLayer3Rotation * _Time);
		cosTheta = cos(_EffectsLayer3Rotation * _Time);
		rotationMatrix = float2x2(cosTheta, -sinTheta, sinTheta, cosTheta);
		o.effect3uv = (mul((o.effect3uv - _EffectsLayer3Translation.xy) *
			(1 / _EffectsLayer3PivotScale.zw), rotationMatrix) + _EffectsLayer3PivotScale.xy)
		*_EffectsLayer3Tex_ST.xy + _EffectsLayer3Tex_ST.zw;
#endif // EFFECTLAYER3ON


		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		float2 distScroll = float2(_Time.x, _Time.x);
		fixed2 dist = (tex2D(_DistTex, i.uv + distScroll).rg - 0.5) * 2;
		fixed distMask = tex2D(_DistMask, i.uv)[0];

		fixed4 col = tex2D(_MainTex, i.uv + dist * distMask * 0.025);
		fixed bg = col.a;


#if EFFECTLAYER1ON
		fixed4 motion1 = tex2D(_EffectsLayer1Motion, i.uv);

		if (_EffectsLayer1MotionSpeedYAxis)
		{
			motion1.y -= _Time.x * _EffectsLayer1MotionSpeedYAxis;
		}
		else if (_EffectsLayer1MotionSpeedXAxis)
		{
			motion1.x -= _Time.x * _EffectsLayer1MotionSpeedXAxis;
		}
		else
		{
			motion1 = fixed4(i.effect1uv.rg, motion1.b, motion1.a);
		}
		//distorts the effect layer
		fixed4 effect1;
		//if (_EffectsLayer1DoDistort)
		//{
		//	fixed2 dist1 = (tex2D(_EffectsLayer1DistTex, i.uv + distScroll).rg - 0.5) * 2;
		//	fixed dist1Mask = tex2D(_EffectsLayer1DistMask, i.uv)[0];
		//
		//	effect1 = tex2D(_EffectsLayer1Tex, i.effect1uv.xy + dist1 * dist1Mask * 0.025);
		//}
		//else
		//{
			effect1 = tex2D(_EffectsLayer1Tex, motion1.xy) * motion1.a;
		//}
		effect1 *= _EffectsLayer1Color;

		col += effect1 * effect1.a * max(bg, _EffectsLayer1Foreground);
#endif // EFFECTLAYER1ON
#if EFFECTLAYER2ON
		fixed4 motion2 = tex2D(_EffectsLayer2Motion, i.uv);

		if (_EffectsLayer2MotionSpeedYAxis)
		{
			motion2.y -= _Time.x * _EffectsLayer2MotionSpeedYAxis;
		}
		else if (_EffectsLayer2MotionSpeedXAxis)
		{
			motion2.x -= _Time.x * _EffectsLayer2MotionSpeedXAxis;
		}
		else
		{
			motion2 = fixed4(i.effect2uv.rg, motion2.b, motion2.a);
		}

		fixed4 effect2 = tex2D(_EffectsLayer2Tex, motion2.xy) * motion2.a;
		effect2 *= _EffectsLayer2Color;

		col += effect2 * effect2.a * max(bg, _EffectsLayer2Foreground);
#endif // EFFECTLAYER2ON
#if EFFECTLAYER3ON
		fixed4 motion3 = tex2D(_EffectsLayer3Motion, i.uv);

		if (_EffectsLayer3MotionSpeedYAxis)
		{
			motion3.y -= _Time.x * _EffectsLayer3MotionSpeedYAxis;
		}
		else if (_EffectsLayer3MotionSpeedXAxis)
		{
			motion3.x -= _Time.x * _EffectsLayer3MotionSpeedXAxis;
		}
		else
		{
			motion3 = fixed4(i.effect3uv.rg, motion3.b, motion3.a);
		}

		fixed4 effect3 = tex2D(_EffectsLayer3Tex, motion3.xy) * motion3.a;
		effect3 *= _EffectsLayer3Color;

		col += effect3 * effect3.a * max(bg, _EffectsLayer3Foreground);
#endif // EFFECTLAYER3ON

		if (_AlphaValue != -1)
		{
			col.a = _AlphaValue;
		}
		return col;
	}


		ENDCG
	}
		}
			CustomEditor "EffectEditor"
			FallBack "Diffuse"
}
