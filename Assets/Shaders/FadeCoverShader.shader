Shader "Custom/FadeCoverShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "black"{}
		_DistTex("Distortion Texture", 2D) = "grey"{}
		_DistMask("Distortion Mask", 2D) = "black"{}

		_AlphaValue("Alpha", float) = 0

		_EffectsLayerTex("Effect Texture", 2D) = "black"{}
		_EffectsLayerColor("Effect Color", Color) = (1,1,1,1)
		_EffectsLayerMotion("Motion Texture", 2D) = "black"{}
		_EffectsLayerMotionSpeedYAxis("Effect Speed Y Axis", float) = 0
		_EffectsLayerMotionSpeedXAxis("Effect Speed X Axis", float) = 0
		_EffectsLayerRotation("Effect Rotation", float) = 0
		_EffectsLayerPivotScale("Effect Scale", Vector) = (0.5,0.5,1,1)
		_EffectsLayerTranslation("Effect Translation", Vector) = (0,0,0,0)
		_EffectsLayerForeground("Foreground", float) = 0
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
		float2 effectuv : TEXCOORD1;

	};

	sampler2D _MainTex;
	sampler2D _DistTex;
	sampler2D _DistMask;

	sampler2D _EffectsLayerTex;
	sampler2D _EffectsLayerMotion;
	float _EffectsLayerMotionSpeedYAxis;
	float _EffectsLayerMotionSpeedXAxis;
	float _EffectsLayerRotation;
	float4 _EffectsLayerPivotScale;
	half4 _EffectsLayerColor;
	float _EffectsLayerForeground;
	float2 _EffectsLayerTranslation;

	float _AlphaValue;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		float2x2 rotationMatrix;
		float sinTheta;
		float cosTheta;

		o.effectuv = o.uv - _EffectsLayerPivotScale.xy;
		sinTheta = sin(_EffectsLayerRotation * _Time);
		cosTheta = cos(_EffectsLayerRotation * _Time);
		rotationMatrix = float2x2(cosTheta, -sinTheta, sinTheta, cosTheta);
		o.effectuv = (mul((o.effectuv - _EffectsLayerTranslation.xy) *
			(1 / _EffectsLayerPivotScale.zw), rotationMatrix) + _EffectsLayerPivotScale.xy);

		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		float2 distScroll = float2(_Time.x, _Time.x);
		fixed2 dist = (tex2D(_DistTex, i.uv + distScroll).rg - 0.5) * 2;
		fixed distMask = tex2D(_DistMask, i.uv)[0];

		fixed4 col = tex2D(_MainTex, i.uv + dist * distMask * 0.025);
		fixed bg = col.a;

		fixed4 motion = tex2D(_EffectsLayerMotion, i.uv);

		if (_EffectsLayerMotionSpeedYAxis)
		{
			motion.y -= _Time.x * _EffectsLayerMotionSpeedYAxis;
		}
		else if (_EffectsLayerMotionSpeedXAxis)
		{
			motion.x -= _Time.x * _EffectsLayerMotionSpeedXAxis;
		}
		else
		{
			motion = fixed4(i.effectuv.rg, motion.b, motion.a);
		}

		fixed4 effect = tex2D(_EffectsLayerTex, motion.xy) * motion.a;
		effect *= _EffectsLayerColor;

		col += effect * effect.a * max(bg, _EffectsLayerForeground);

		col.a = _AlphaValue;
		return col;
	}


		ENDCG
	}
		}
			Fallback OFF

}
