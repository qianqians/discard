struct Constants
{
	float4x4 ViewProjMat;
	float4 ViewPos;
	float4 LightPos;
};

cbuffer cbConstants
{
	Constants g_Constants;
};

struct VSInput
{
	float3 a_position : ATTRIB0;
	float3 a_tangent : ATTRIB1;
	float3 a_normal : ATTRIB2;
	float2 a_texcoord0 : ATTRIB3;
};

struct PSInput
{
	float4 pos : SV_POSITION;
	float3 v_normal : NORMAL0;
	float3 v_tangent : TANGENT;
	float3 v_bitangent : BINORMAL;
	float3 v_position : POSITION0;
	float2 v_texcoord0  : TEXCOORD0;
};

void main(in  VSInput VSIn,
	      out PSInput PSIn)
{
	PSIn.v_normal = VSIn.a_normal;
	PSIn.v_texcoord0 = VSIn.a_texcoord0;

	float3 T = normalize(VSIn.a_tangent);
	float3 N = normalize(VSIn.a_normal);
    T = normalize(T - dot(T, N) * N);
	PSIn.v_bitangent = normalize(cross(N, T));

	PSIn.v_tangent = VSIn.a_tangent;
	PSIn.v_position = VSIn.a_position;

	PSIn.pos = mul(float4(VSIn.a_position, 1.0), g_Constants.ViewProjMat);
}
