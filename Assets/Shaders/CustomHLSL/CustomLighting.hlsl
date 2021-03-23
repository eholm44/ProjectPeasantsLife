#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

void MainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out float ShadowAtten)
{

    Direction = float3(0.5, 0.5, 0);
    Color = 1;
    ShadowAtten = 1;
#ifndef SHADERGRAPH_PREVIEW

    float4 shadowCoord = float4(0, 0, 0, 0);
    Light mainLight = GetMainLight(shadowCoord);
    Direction = mainLight.direction;
    Color = mainLight.color;

	#if !defined(_MAIN_LIGHT_SHADOWS) || defined(_RECEIVE_SHADOWS_OFF)
		ShadowAtten = 1.0h;
    #else
	    ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	    float shadowStrength = GetMainLightShadowStrength();
	    ShadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture,
	    sampler_MainLightShadowmapTexture),
	    shadowSamplingData, shadowStrength, false);
    #endif
#endif
}


void DirectSpecular_float(float Smoothness, float3 Direction, float3 WorldNorm, float3 WorldView, out float3 Out)
{
    float4 White = 1;
    Out = 0;

#ifndef SHADERGRAPH_PREVIEW
    Smoothness = exp2(10 * Smoothness + 1);
    WorldNorm = normalize(WorldNorm);
    WorldView = SafeNormalize(WorldView);
    Out = LightingSpecular(White, Direction, WorldNorm, WorldView, White, Smoothness);
#endif
}

void AdditionalLights_float(float Smoothness, float3 WorldPos, float3 WorldNorm, float3 WorldView, out float3 Diffuse, out float3 Specular)
{
    float3 diffuseColor = 0;
    float3 specularColor = 0;
    float4 White = 1;

#ifndef SHADERGRAPH_PREVIEW
    Smoothness = exp2(10 * Smoothness + 1);
    WorldNorm = normalize(WorldNorm);
    WorldView = SafeNormalize(WorldView);
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
        Light light = GetAdditionalLight(i, WorldPos);
        half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
        diffuseColor += LightingLambert(attenuatedLightColor, light.direction, WorldNorm);
        specularColor += LightingSpecular(attenuatedLightColor, light.direction, WorldNorm, WorldView, White, Smoothness);
    }
#endif

    Diffuse = diffuseColor;
    Specular = specularColor;
}

#endif