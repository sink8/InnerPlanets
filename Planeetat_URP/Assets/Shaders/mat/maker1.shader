Shader "Unlit/NewUnlitShader"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Pass
		{
			Blend One One
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
#define hlsl_atan(x,y) atan2(x, y)
#define mod(x,y) ((x)-(y)*floor((x)/(y)))
inline float4 textureLod(sampler2D tex, float2 uv, float lod) {
    return tex2D(tex, uv);
}
inline float2 tofloat2(float x) {
    return float2(x, x);
}
inline float2 tofloat2(float x, float y) {
    return float2(x, y);
}
inline float3 tofloat3(float x) {
    return float3(x, x, x);
}
inline float3 tofloat3(float x, float y, float z) {
    return float3(x, y, z);
}
inline float3 tofloat3(float2 xy, float z) {
    return float3(xy.x, xy.y, z);
}
inline float3 tofloat3(float x, float2 yz) {
    return float3(x, yz.x, yz.y);
}
inline float4 tofloat4(float x, float y, float z, float w) {
    return float4(x, y, z, w);
}
inline float4 tofloat4(float x) {
    return float4(x, x, x, x);
}
inline float4 tofloat4(float x, float3 yzw) {
    return float4(x, yzw.x, yzw.y, yzw.z);
}
inline float4 tofloat4(float2 xy, float2 zw) {
    return float4(xy.x, xy.y, zw.x, zw.y);
}
inline float4 tofloat4(float3 xyz, float w) {
    return float4(xyz.x, xyz.y, xyz.z, w);
}
inline float4 tofloat4(float2 xy, float z, float w) {
    return float4(xy.x, xy.y, z, w);
}
inline float2x2 tofloat2x2(float2 v1, float2 v2) {
    return float2x2(v1.x, v1.y, v2.x, v2.y);
}
// EngineSpecificDefinitions
float rand(float2 x) {
    return frac(cos(mod(dot(x, tofloat2(13.9898, 8.141)), 3.14)) * 43758.5453);
}
float2 rand2(float2 x) {
    return frac(cos(mod(tofloat2(dot(x, tofloat2(13.9898, 8.141)),
						      dot(x, tofloat2(3.4562, 17.398))), tofloat2(3.14))) * 43758.5453);
}
float3 rand3(float2 x) {
    return frac(cos(mod(tofloat3(dot(x, tofloat2(13.9898, 8.141)),
							  dot(x, tofloat2(3.4562, 17.398)),
                              dot(x, tofloat2(13.254, 5.867))), tofloat3(3.14))) * 43758.5453);
}
float param_rnd(float minimum, float maximum, float seed) {
	return minimum+(maximum-minimum)*rand(tofloat2(seed));
}
float3 rgb2hsv(float3 c) {
	float4 K = tofloat4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 p = c.g < c.b ? tofloat4(c.bg, K.wz) : tofloat4(c.gb, K.xy);
	float4 q = c.r < p.x ? tofloat4(p.xyw, c.r) : tofloat4(c.r, p.yzx);
	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return tofloat3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}
float3 hsv2rgb(float3 c) {
	float4 K = tofloat4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
uniform sampler2D texture_1;
static const float texture_1_size = 128.0;
float2 transform2_clamp(float2 uv) {
	return clamp(uv, tofloat2(0.0), tofloat2(1.0));
}
float2 transform2(float2 uv, float2 translate, float rotate, float2 scale) {
 	float2 rv;
	uv -= translate;
	uv -= tofloat2(0.5);
	rv.x = cos(rotate)*uv.x + sin(rotate)*uv.y;
	rv.y = -sin(rotate)*uv.x + cos(rotate)*uv.y;
	rv /= scale;
	rv += tofloat2(0.5);
	return rv;	
}
uniform sampler2D texture_2;
static const float texture_2_size = 256.0;
float pingpong(float a, float b)
{
  return (b != 0.0) ? abs(frac((a - b) / (b * 2.0)) * b * 2.0 - b) : 0.0;
}
float3 blend_normal(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*c1 + (1.0-opacity)*c2;
}
float3 blend_dissolve(float2 uv, float3 c1, float3 c2, float opacity) {
	if (rand(uv) < opacity) {
		return c1;
	} else {
		return c2;
	}
}
float3 blend_multiply(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*c1*c2 + (1.0-opacity)*c2;
}
float3 blend_screen(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*(1.0-(1.0-c1)*(1.0-c2)) + (1.0-opacity)*c2;
}
float blend_overlay_f(float c1, float c2) {
	return (c1 < 0.5) ? (2.0*c1*c2) : (1.0-2.0*(1.0-c1)*(1.0-c2));
}
float3 blend_overlay(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_overlay_f(c1.x, c2.x), blend_overlay_f(c1.y, c2.y), blend_overlay_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float3 blend_hard_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*0.5*(c1*c2+blend_overlay(uv, c1, c2, 1.0)) + (1.0-opacity)*c2;
}
float blend_soft_light_f(float c1, float c2) {
	return (c2 < 0.5) ? (2.0*c1*c2+c1*c1*(1.0-2.0*c2)) : 2.0*c1*(1.0-c2)+sqrt(c1)*(2.0*c2-1.0);
}
float3 blend_soft_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_soft_light_f(c1.x, c2.x), blend_soft_light_f(c1.y, c2.y), blend_soft_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_burn_f(float c1, float c2) {
	return (c1==0.0)?c1:max((1.0-((1.0-c2)/c1)),0.0);
}
float3 blend_burn(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_burn_f(c1.x, c2.x), blend_burn_f(c1.y, c2.y), blend_burn_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_dodge_f(float c1, float c2) {
	return (c1==1.0)?c1:min(c2/(1.0-c1),1.0);
}
float3 blend_dodge(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_dodge_f(c1.x, c2.x), blend_dodge_f(c1.y, c2.y), blend_dodge_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float3 blend_lighten(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*max(c1, c2) + (1.0-opacity)*c2;
}
float3 blend_darken(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*min(c1, c2) + (1.0-opacity)*c2;
}
float3 blend_difference(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*clamp(c2-c1, tofloat3(0.0), tofloat3(1.0)) + (1.0-opacity)*c2;
}
float3 blend_additive(float2 uv, float3 c1, float3 c2, float oppacity) {
	return c2 + c1 * oppacity;
}
float3 blend_addsub(float2 uv, float3 c1, float3 c2, float oppacity) {
	return c2 + (c1 - .5) * 2.0 * oppacity;
}
float blend_linear_light_f(float c1, float c2) {
	return (c1 + 2.0 * c2) - 1.0;
}
float3 blend_linear_light(float2 uv, float3 c1, float3 c2, float opacity) {
return opacity*tofloat3(blend_linear_light_f(c1.x, c2.x), blend_linear_light_f(c1.y, c2.y), blend_linear_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_vivid_light_f(float c1, float c2) {
	return (c1 < 0.5) ? 1.0 - (1.0 - c2) / (2.0 * c1) : c2 / (2.0 * (1.0 - c1));
}
float3 blend_vivid_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_vivid_light_f(c1.x, c2.x), blend_vivid_light_f(c1.y, c2.y), blend_vivid_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_pin_light_f( float c1, float c2) {
	return (2.0 * c1 - 1.0 > c2) ? 2.0 * c1 - 1.0 : ((c1 < 0.5 * c2) ? 2.0 * c1 : c2);
}
float3 blend_pin_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_pin_light_f(c1.x, c2.x), blend_pin_light_f(c1.y, c2.y), blend_pin_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_hard_lerp_f(float c1, float c2) {
	return floor(c1 + c2);
}
float3 blend_hard_lerp(float2 uv, float3 c1, float3 c2, float opacity) {
		return opacity*tofloat3(blend_hard_lerp_f(c1.x, c2.x), blend_hard_lerp_f(c1.y, c2.y), blend_hard_lerp_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_exclusion_f(float c1, float c2) {
	return c1 + c2 - 2.0 * c1 * c2;
}
float3 blend_exclusion(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_exclusion_f(c1.x, c2.x), blend_exclusion_f(c1.y, c2.y), blend_exclusion_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
uniform sampler2D texture_3;
static const float texture_3_size = 1024.0;
static const float p_o17438_fresnel_amount = 1.000000000;
float3 o17438_input_offset(float2 uv, float _seed_variation_) {
return tofloat3(0.0);
}
float3 o17438_input_scale(float2 uv, float _seed_variation_) {
return tofloat3(1.0);
}
static const float p_o17479_amount1 = 1.000000000;
static const float p_o17477_amount1 = 1.000000000;
static const float p_o17466_amount1 = 0.250000000;
static const float p_o17461_default_in1 = 0.000000000;
static const float p_o17461_default_in2 = 0.000000000;
static const float p_o17446_default_in1 = 0.000000000;
static const float p_o17446_default_in2 = 50.000000000;
static const float p_o17457_default_in1 = 0.000000000;
static const float p_o17457_default_in2 = 0.000000000;
static const float p_o17447_amount = 0.255000000;
static const float p_o17447_eps = 0.100000000;
static const float p_o17452_amount = 0.190000000;
static const float p_o17452_eps = 0.100000000;
static const float p_o17456_translate_x = 0.000000000;
static const float p_o17456_rotate = 90.000000000;
static const float p_o17456_scale_x = 1.000000000;
static const float p_o17456_scale_y = -1.000000000;
float o17452_input_d(float2 uv, float _seed_variation_) {
float4 o17458_0 = textureLod(texture_1, frac(transform2((uv), tofloat2(p_o17456_translate_x*(2.0*1.0-1.0), (_Time.y*0.2)*(2.0*1.0-1.0)), p_o17456_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o17456_scale_x*(2.0*1.0-1.0), p_o17456_scale_y*(2.0*1.0-1.0)))), 0.0);
float4 o17456_0_1_rgba = o17458_0;
return (dot((o17456_0_1_rgba).rgb, tofloat3(1.0))/3.0);
}
float2 o17452_slope(float2 uv, float epsilon, float _seed_variation_) {
	return tofloat2(o17452_input_d(frac(uv+tofloat2(epsilon, 0.0)), _seed_variation_)-o17452_input_d(frac(uv-tofloat2(epsilon, 0.0)), _seed_variation_), o17452_input_d(frac(uv+tofloat2(0.0, epsilon)), _seed_variation_)-o17452_input_d(frac(uv-tofloat2(0.0, epsilon)), _seed_variation_));
}float o17447_input_d(float2 uv, float _seed_variation_) {
float2 o17452_0_slope = o17452_slope((uv), p_o17452_eps, _seed_variation_);
float2 o17452_0_warp = o17452_0_slope;float4 o17468_0 = textureLod(texture_2, (uv)+p_o17452_amount*o17452_0_warp, 0.0);
float4 o17452_0_1_rgba = o17468_0;
return (dot((o17452_0_1_rgba).rgb, tofloat3(1.0))/3.0);
}
float2 o17447_slope(float2 uv, float epsilon, float _seed_variation_) {
	return tofloat2(o17447_input_d(frac(uv+tofloat2(epsilon, 0.0)), _seed_variation_)-o17447_input_d(frac(uv-tofloat2(epsilon, 0.0)), _seed_variation_), o17447_input_d(frac(uv+tofloat2(0.0, epsilon)), _seed_variation_)-o17447_input_d(frac(uv-tofloat2(0.0, epsilon)), _seed_variation_));
}static const float p_o17444_translate_x = 0.000000000;
static const float p_o17444_rotate = 0.000000000;
static const float p_o17444_scale_x = 1.000000000;
static const float p_o17444_scale_y = -1.000000000;
static const float p_o17460_default_in1 = 0.000000000;
static const float p_o17460_default_in2 = 5.000000000;
static const float p_o17467_translate_x = 0.016250000;
static const float p_o17467_translate_y = -0.002200000;
static const float p_o17462_count = 5.000000000;
static const float p_o17462_width = 10.000000000;
float4 o17462_input_in(float2 uv, float _seed_variation_) {
float2 o17447_0_slope = o17447_slope((uv), p_o17447_eps, _seed_variation_);
float2 o17447_0_warp = o17447_0_slope;float4 o17444_0_1_rgba = tofloat4((frac(transform2(((uv)+p_o17447_amount*o17447_0_warp), tofloat2(p_o17444_translate_x*(2.0*1.0-1.0), (-_Time.y*0.5)*(2.0*1.0-1.0)), p_o17444_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o17444_scale_x*(2.0*1.0-1.0), p_o17444_scale_y*(2.0*1.0-1.0))))), 0.0, 1.0);
float4 o17447_0_1_rgba = o17444_0_1_rgba;
float o17445_1_1_f = o17447_0_1_rgba.g;
float o17457_0_clamp_false = frac(o17445_1_1_f);
float o17457_0_clamp_true = clamp(o17457_0_clamp_false, 0.0, 1.0);
float o17457_0_1_f = o17457_0_clamp_false;
float o17446_0_clamp_false = pow(o17457_0_1_f,p_o17446_default_in2);
float o17446_0_clamp_true = clamp(o17446_0_clamp_false, 0.0, 1.0);
float o17446_0_2_f = o17446_0_clamp_false;
float o17460_0_clamp_false = pow(o17457_0_1_f,p_o17460_default_in2);
float o17460_0_clamp_true = clamp(o17460_0_clamp_false, 0.0, 1.0);
float o17460_0_2_f = o17460_0_clamp_false;
float o17461_0_clamp_false = o17446_0_2_f+o17460_0_2_f;
float o17461_0_clamp_true = clamp(o17461_0_clamp_false, 0.0, 1.0);
float o17461_0_1_f = o17461_0_clamp_false;
return tofloat4(tofloat3(o17461_0_1_f), 1.0);
}
float4 supersample_o17462(float2 uv, float size, int count, float width, float _seed_variation_) {
	float4 rv = tofloat4(0.0);
	float2 step_size = tofloat2(width)/size/float(count);
	uv -= tofloat2(0.5)/size;
	for (int x = 0; x < count; ++x) {
		for (int y = 0; y < count; ++y) {
			rv += o17462_input_in(uv+(tofloat2(float(x), float(y))+tofloat2(0.5))*step_size, _seed_variation_);
		}
	}
	return rv/float(count*count);
}static const float p_o17478_value = 0.013700000;
static const float p_o17478_width = 0.022400000;
static const float4 p_o17480_color = tofloat4(1.000000000, 0.237948000, 0.743390977, 1.000000000);
		
			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				float _seed_variation_ = 0.0;
				float2 uv = i.uv;
float2 o17447_0_slope = o17447_slope((uv), p_o17447_eps, _seed_variation_);
float2 o17447_0_warp = o17447_0_slope;float4 o17444_0_1_rgba = tofloat4((frac(transform2(((uv)+p_o17447_amount*o17447_0_warp), tofloat2(p_o17444_translate_x*(2.0*1.0-1.0), (-_Time.y*0.5)*(2.0*1.0-1.0)), p_o17444_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o17444_scale_x*(2.0*1.0-1.0), p_o17444_scale_y*(2.0*1.0-1.0))))), 0.0, 1.0);
float4 o17447_0_1_rgba = o17444_0_1_rgba;
float o17445_1_1_f = o17447_0_1_rgba.g;
float o17457_0_clamp_false = frac(o17445_1_1_f);
float o17457_0_clamp_true = clamp(o17457_0_clamp_false, 0.0, 1.0);
float o17457_0_1_f = o17457_0_clamp_false;
float o17446_0_clamp_false = pow(o17457_0_1_f,p_o17446_default_in2);
float o17446_0_clamp_true = clamp(o17446_0_clamp_false, 0.0, 1.0);
float o17446_0_2_f = o17446_0_clamp_false;
float o17460_0_clamp_false = pow(o17457_0_1_f,p_o17460_default_in2);
float o17460_0_clamp_true = clamp(o17460_0_clamp_false, 0.0, 1.0);
float o17460_0_2_f = o17460_0_clamp_false;
float o17461_0_clamp_false = o17446_0_2_f+o17460_0_2_f;
float o17461_0_clamp_true = clamp(o17461_0_clamp_false, 0.0, 1.0);
float o17461_0_1_f = o17461_0_clamp_false;
float4 o17462_0_1_rgba = supersample_o17462(((uv)-tofloat2(p_o17467_translate_x, p_o17467_translate_y)), 256.000000000, int(p_o17462_count), p_o17462_width, _seed_variation_);
float4 o17467_0_1_rgba = o17462_0_1_rgba;
float4 o17466_0_b = tofloat4(tofloat3(o17461_0_1_f), 1.0);
float4 o17466_0_l;
float o17466_0_a;

o17466_0_l = o17467_0_1_rgba;
o17466_0_a = p_o17466_amount1*1.0;
o17466_0_b = tofloat4(blend_lighten((uv), o17466_0_l.rgb, o17466_0_b.rgb, o17466_0_a*o17466_0_l.a), min(1.0, o17466_0_b.a+o17466_0_a*o17466_0_l.a));

float4 o17466_0_2_rgba = o17466_0_b;
float4 o17475_0 = textureLod(texture_3, (uv), 0.0);
float3 o17478_0_false = clamp((o17475_0.rgb-tofloat3(p_o17478_value))/max(0.0001, p_o17478_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o17478_0_true = tofloat3(1.0)-o17478_0_false;float4 o17478_0_1_rgba = tofloat4(o17478_0_true, o17475_0.a);
float4 o17477_0_b = o17466_0_2_rgba;
float4 o17477_0_l;
float o17477_0_a;

o17477_0_l = o17466_0_2_rgba;
o17477_0_a = p_o17477_amount1*(dot((o17478_0_1_rgba).rgb, tofloat3(1.0))/3.0);
o17477_0_b = tofloat4(blend_additive((uv), o17477_0_l.rgb, o17477_0_b.rgb, o17477_0_a*o17477_0_l.a), min(1.0, o17477_0_b.a+o17477_0_a*o17477_0_l.a));

float4 o17477_0_1_rgba = o17477_0_b;
float4 o17480_0_1_rgba = p_o17480_color;
float4 o17479_0_b = o17477_0_1_rgba;
float4 o17479_0_l;
float o17479_0_a;

o17479_0_l = o17480_0_1_rgba;
o17479_0_a = p_o17479_amount1*1.0;
o17479_0_b = tofloat4(blend_multiply((uv), o17479_0_l.rgb, o17479_0_b.rgb, o17479_0_a*o17479_0_l.a), min(1.0, o17479_0_b.a+o17479_0_a*o17479_0_l.a));

float4 o17479_0_2_rgba = o17479_0_b;

				// sample the generated texture
				fixed4 col = o17479_0_2_rgba;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}



