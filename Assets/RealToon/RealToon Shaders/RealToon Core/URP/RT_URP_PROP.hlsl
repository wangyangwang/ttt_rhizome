//RealToon URP - RT_URP_PROP
//MJQStudioWorks

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

//===============================================================================
//CBUF
//===============================================================================

CBUFFER_START(UnityPerMaterial)

	//== Others
		uniform float4 _MainTex_ST;

		uniform half4 _MainColor;
		uniform half _MVCOL;
		uniform half _MCIALO;
		uniform half _TexturePatternStyle;
		uniform half4 _HighlightColor;
		uniform half _HighlightColorPower;
		uniform half _EnableTextureTransparent;
	//==


	//== N_F_CO_ON
		uniform float4 _OutlineWidthControl_ST;
		uniform half _OutlineWidth;
		uniform float3 _OEM;
		uniform int _OutlineExtrudeMethod;
		uniform half3 _OutlineOffset;
		uniform half _OutlineZPostionInCamera;
		uniform half4 _OutlineColor;
		uniform half _MixMainTexToOutline;
		uniform half _NoisyOutlineIntensity;
		uniform half _DynamicNoisyOutline;
		uniform half _LightAffectOutlineColor;
		uniform half _OutlineWidthAffectedByViewDistance;
		uniform half _FarDistanceMaxWidth;
		uniform half _VertexColorBlueAffectOutlineWitdh;
	//==


	//== N_F_MC_ON
		uniform half _MCapIntensity;

		uniform float4 _MCap_ST;

		uniform half _SPECMODE;
		uniform half _SPECIN;

		uniform float4 _MCapMask_ST;
	//==


	//== Transparency
		uniform float4 _MaskTransparency_ST;
		uniform half _Opacity;
		uniform half _TransparentThreshold;
	//==


	//== N_F_CO_ON
		uniform half _Cutout;
		uniform half _AlphaBaseCutout;
		uniform half _UseSecondaryCutout;

		uniform float4 _SecondaryCutout_ST;
	//==


	//== N_F_NM_ON
		uniform float4 _NormalMap_ST;

		uniform half _NormalMapIntensity;
	//==


	//== N_F_CA_ON
		uniform half _Saturation;
	//== 


	//== N_F_SL_ON
		uniform half _SelfLitIntensity;
		uniform half4 _SelfLitColor;
		uniform half _SelfLitPower;
		uniform half _TEXMCOLINT;
		uniform half _SelfLitHighContrast;

		uniform float4 _MaskSelfLit_ST;
	//==


	//== N_F_GLO_ON
		uniform half _GlossIntensity;
		uniform half _Glossiness;
		uniform half _GlossSoftness;
		uniform half4 _GlossColor;
		uniform half _GlossColorPower;

		uniform float4 _MaskGloss_ST;
	//==


	//== N_F_GLO_ON -> N_F_GLOT_ON
		uniform float4 _GlossTexture_ST;

		uniform half _GlossTextureSoftness;
		uniform half _PSGLOTEX;
		uniform half _GlossTextureRotate;
		uniform half _GlossTextureFollowObjectRotation;
		uniform half _GlossTextureFollowLight;
	//==


	//== Others
		uniform half4 _OverallShadowColor;
		uniform half _OverallShadowColorPower;

		uniform half _SelfShadowShadowTAtViewDirection;

		uniform half _ShadowHardness;
		uniform half _SelfShadowRealtimeShadowIntensity;
	//==


	//== N_F_SS_ON
		uniform half _SelfShadowThreshold;
		uniform half _VertexColorGreenControlSelfShadowThreshold;
		uniform half _SelfShadowHardness;
		uniform half _SelfShadowAffectedByLightShadowStrength;
	//==


	//== N_F_SS_ON -> Transparency
		uniform half _SelfShadowIntensity;
		uniform half4 _SelfShadowColor;
		uniform half _SelfShadowColorPower;
	//==


	//== Others
		uniform half4 _SelfShadowRealTimeShadowColor;
		uniform half _SelfShadowRealTimeShadowColorPower;
	//==


	//== N_F_SON_ON
		uniform half _SmoothObjectNormal;
		uniform half _VertexColorRedControlSmoothObjectNormal;
		uniform float4 _XYZPosition;
		uniform half _XYZHardness;
		uniform half _ShowNormal;
	//==


	//== N_F_SCT_ON
		uniform float4 _ShadowColorTexture_ST;

		uniform half _ShadowColorTexturePower;
	//==


	//== N_F_ST_ON
		uniform half _ShadowTIntensity;

		uniform float4 _ShadowT_ST;

		uniform half _ShadowTLightThreshold;
		uniform half _ShadowTShadowThreshold;
		uniform half4 _ShadowTColor;
		uniform half _ShadowTColorPower;
		uniform half _ShadowTHardness;
		uniform half _STIL;
		uniform half _ShowInAmbientLightShadowIntensity;
		uniform half _ShowInAmbientLightShadowThreshold;
		uniform half _LightFalloffAffectShadowT;
	//==


	//==  N_F_PT_ON
		uniform float4 _PTexture_ST;

		uniform half _PTexturePower;
	//==


	//==  N_F_RELGI_ON
		uniform half _GIFlatShade;
		uniform half _GIShadeThreshold;
		uniform half _EnvironmentalLightingIntensity;
	//==
		

	//== Others
		uniform half _LightAffectShadow;
		uniform half _LightIntensity;
		uniform half _DirectionalLightIntensity;
		uniform half _PointSpotlightIntensity;
		uniform half _LightFalloffSoftness;
	//==


	//== N_F_CLD_ON
		uniform half _CustomLightDirectionIntensity;
		uniform half4 _CustomLightDirection;
		uniform half _CustomLightDirectionFollowObjectRotation;
	//==


	//== N_F_R_ON
		uniform half _ReflectionIntensity;
		uniform half _ReflectionRoughtness;
		uniform half _RefMetallic;

		uniform float4 _MaskReflection_ST;
	//==


	//== N_F_FR_ON
		float4 _FReflection_ST;
	//==


	//== N_F_RL_ON
		uniform half _RimLightUnfill;
		uniform half _RimLightSoftness;
		uniform half _LightAffectRimLightColor;
		uniform half4 _RimLightColor;
		uniform half _RimLightColorPower;
		uniform half _RimLightInLight;
	//==


	//== Others
		uniform half _ReduceShadowSpotDirectionalLight;
		uniform sampler3D _DitherMaskLOD;
	//==

CBUFFER_END


//===============================================================================
//Non CBUF
//===============================================================================


TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

TEXTURE2D(_MaskTransparency);
SAMPLER(sampler_MaskTransparency);

#if N_F_O_ON
	TEXTURE2D(_OutlineWidthControl);
	SAMPLER(sampler_OutlineWidthControl);
#endif

#if N_F_MC_ON
	TEXTURE2D(_MCap);
	SAMPLER(sampler_MCap);

	TEXTURE2D(_MCapMask);
	SAMPLER(sampler_MCapMask);
#endif

#if N_F_CO_ON
	TEXTURE2D(_SecondaryCutout);
	SAMPLER(sampler_SecondaryCutout);
#endif

#if N_F_NM_ON
	TEXTURE2D(_NormalMap);
	SAMPLER(sampler_NormalMap);
#endif

#if N_F_SL_ON
	TEXTURE2D(_MaskSelfLit);
	SAMPLER(sampler_MaskSelfLit);
#endif

#if N_F_GLO_ON
	TEXTURE2D(_MaskGloss);
	SAMPLER(sampler_MaskGloss);
#endif

#if N_F_GLO_ON
	#if N_F_GLOT_ON
		TEXTURE2D(_GlossTexture);
		SAMPLER(sampler_GlossTexture);
	#endif
#endif

#if N_F_SCT_ON
	TEXTURE2D(_ShadowColorTexture);
	SAMPLER(sampler_ShadowColorTexture);
#endif

#if N_F_ST_ON
	TEXTURE2D(_ShadowT);
	SAMPLER(sampler_ShadowT);
#endif

#if N_F_PT_ON
	TEXTURE2D(_PTexture);
	SAMPLER(sampler_PTexture);
#endif

#if N_F_R_ON
	TEXTURE2D(_MaskReflection);
	SAMPLER(sampler_MaskReflection);
#endif

#if N_F_R_ON
	#if N_F_FR_ON
		TEXTURE2D(_FReflection);
		SAMPLER(sampler_FReflection);
	#endif
#endif