Shader "Custom/EffectShaderExpanded"
{
	Properties
	{ 
		//[Toggle(REDIFY_ON)] _Redify("Red?", Int) = 0
		_MainTex("Main Texture", 2D) = "black"{}
		_DistTex("Distortion Texture", 2D) = "grey"{}
		_DistMask("Distortion Mask", 2D) = "black"{}
		_AlphaValue("Alpha", float) = 0


		_EffectsLayer1Tex("", 2D) = "black"{}
		_EffectsLayer1Color("", Color) = (1,1,1,1)
		_EffectsLayer1Motion("", 2D) = "black"{}
		_EffectsLayer1MotionSpeedYAxis("", float) = 0
		_EffectsLayer1MotionSpeedXAxis("", float) = 0
		_EffectsLayer1Rotation("", float) = 0
		_EffectsLayer1PivotScale("", Vector) = (0.5,0.5,1,1)
		_EffectsLayer1Translation("", Vector) = (0,0,0,0)
		_EffectsLayer1Foreground("", float) = 0


		_EffectsLayer2Tex("", 2D) = "black"{}
		_EffectsLayer2Color("", Color) = (1,1,1,1)
		_EffectsLayer2Motion("", 2D) = "black"{}
		_EffectsLayer2MotionSpeedYAxis("", float) = 0
		_EffectsLayer2MotionSpeedXAxis("", float) = 0
		_EffectsLayer2Rotation("", float) = 0
		_EffectsLayer2PivotScale("", Vector) = (0.5,0.5,1,1)
		_EffectsLayer2Translation("", Vector) = (0,0,0,0)
		_EffectsLayer2Foreground("", float) = 0
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


	};

	sampler2D _MainTex;
	sampler2D _DistTex;
	sampler2D _DistMask;

	float _AlphaValue;

	sampler2D	_EffectsLayer1Tex;
	sampler2D	_EffectsLayer1Motion;
	float		_EffectsLayer1MotionSpeedYAxis;
	float		_EffectsLayer1MotionSpeedXAxis;
	float		_EffectsLayer1Rotation;
	float4		_EffectsLayer1PivotScale;
	half4		_EffectsLayer1Color;
	float		_EffectsLayer1Foreground;
	float2		_EffectsLayer1Translation;

	sampler2D	_EffectsLayer2Tex;
	sampler2D	_EffectsLayer2Motion;
	float		_EffectsLayer2MotionSpeedYAxis;
	float		_EffectsLayer2MotionSpeedXAxis;
	float		_EffectsLayer2Rotation;
	float4		_EffectsLayer2PivotScale;
	half4		_EffectsLayer2Color;
	float		_EffectsLayer2Foreground;
	float2		_EffectsLayer2Translation;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		float2x2 rotationMatrix;
		float sinTheta;
		float cosTheta;

#if EFFECTLAYER1ON
		o.effect1uv = o.uv - _EffectsLayer1PivotScale.xy;
		sinTheta = sin(_EffectsLayer1Rotation * _Time);
		cosTheta = cos(_EffectsLayer1Rotation * _Time);
		rotationMatrix = float2x2(cosTheta, -sinTheta, sinTheta, cosTheta);
		o.effect1uv = (mul((o.effect1uv - _EffectsLayer1Translation.xy) *
			(1 / _EffectsLayer1PivotScale.zw), rotationMatrix) + _EffectsLayer1PivotScale.xy);
#endif // EFFECTLAYER1ON
#if EFFECTLAYER2ON
		o.effect2uv = o.uv - _EffectsLayer2PivotScale.xy;
		sinTheta = sin(_EffectsLayer2Rotation * _Time);
		cosTheta = cos(_EffectsLayer2Rotation * _Time);
		rotationMatrix = float2x2(cosTheta, -sinTheta, sinTheta, cosTheta);
		o.effect2uv = (mul((o.effect2uv - _EffectsLayer2Translation.xy) *
			(1 / _EffectsLayer2PivotScale.zw), rotationMatrix) + _EffectsLayer2PivotScale.xy);
#endif // EFFECTLAYER2ON


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

		fixed4 effect1 = tex2D(_EffectsLayer1Tex, motion1.xy) * motion1.a;
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

		col.a = _AlphaValue;
		return col;
	}


		ENDCG
	}
		}
			CustomEditor "EffectEditor"
			FallBack "Diffuse"
}
