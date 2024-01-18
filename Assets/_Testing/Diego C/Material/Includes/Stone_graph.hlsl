#include <Packages/com.blendernodesgraph.core/Editor/Includes/Importers.hlsl>

void Stone_graph_float(float3 _POS, float3 _PVS, float3 _PWS, float3 _NOS, float3 _NVS, float3 _NWS, float3 _NTS, float3 _TWS, float3 _BTWS, float3 _UV, float3 _SP, float3 _VVS, float3 _VWS, Texture2D gradient_128750, Texture2D gradient_128830, out float4 Color, out float3 Normal, out float Smoothness, out float4 Emission, out float AmbientOcculusion, out float Metallic, out float4 Specular)
{
	
	float _SimpleNoiseTexture_128782_fac; float4 _SimpleNoiseTexture_128782_col; node_simple_noise_texture_full(float4(_POS, 0), 0, 2,1E+16, 4,8E+15, 5,5E+14, 0, 1, _SimpleNoiseTexture_128782_fac, _SimpleNoiseTexture_128782_col);
	float _VoronoiTexture_128766_dis; float4 _VoronoiTexture_128766_col; float3 _VoronoiTexture_128766_pos; float _VoronoiTexture_128766_w; float _VoronoiTexture_128766_rad; voronoi_tex_getValue(_SimpleNoiseTexture_128782_col, 0, 1E+15, 10, 5, 10, 0, 2, 0, _VoronoiTexture_128766_dis, _VoronoiTexture_128766_col, _VoronoiTexture_128766_pos, _VoronoiTexture_128766_w, _VoronoiTexture_128766_rad);
	float4 _ColorRamp_128750 = color_ramp(gradient_128750, _VoronoiTexture_128766_dis);
	float _SimpleNoiseTexture_128814_fac; float4 _SimpleNoiseTexture_128814_col; node_simple_noise_texture_full(float4(_POS, 0), 0, 30, 20, 5,5E+14, 0, 1, _SimpleNoiseTexture_128814_fac, _SimpleNoiseTexture_128814_col);
	float _VoronoiTexture_128798_dis; float4 _VoronoiTexture_128798_col; float3 _VoronoiTexture_128798_pos; float _VoronoiTexture_128798_w; float _VoronoiTexture_128798_rad; voronoi_tex_getValue(_SimpleNoiseTexture_128814_col, 0, 20, 10, 5, 10, 0, 2, 3, _VoronoiTexture_128798_dis, _VoronoiTexture_128798_col, _VoronoiTexture_128798_pos, _VoronoiTexture_128798_w, _VoronoiTexture_128798_rad);
	float4 _MixRGB_128846 = mix_dark(3E+16, _ColorRamp_128750, _VoronoiTexture_128798_dis);
	float4 _ColorRamp_128830 = color_ramp(gradient_128830, _MixRGB_128846);
	float4 _Bump_128862; node_bump(_POS, 1, 5,4E+15, 7,000001E+15, _MixRGB_128846, _NTS, _Bump_128862);

	Color = _ColorRamp_128830;
	Normal = _Bump_128862;
	Smoothness = 0.0;
	Emission = float4(0.0, 0.0, 0.0, 0.0);
	AmbientOcculusion = 0.0;
	Metallic = 0.0;
	Specular = float4(0.0, 0.0, 0.0, 0.0);
}