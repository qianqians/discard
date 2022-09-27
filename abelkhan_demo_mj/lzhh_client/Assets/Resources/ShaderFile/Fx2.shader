// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/Fx2" {
	Properties{
		//主纹理属性
		_MainTex("mainTexture", 2D) = "white" {}
	//法线贴图纹理属性
		_BumpMap("Bumpmap", 2D) = "bump" {}
	}
		//Shader执行域
		SubShader{
		//标明渲染类型是不透明的物体
		Tags{ "RenderType" = "Opaque" }
		//标明CG程序的开始
		CGPROGRAM
		//声明表面着色器函数
#pragma surface surf Lambert
		//定义着色器函数输入的参数Input
		struct Input {
		//主纹理坐标值
		float2 uv_MainTex;
		//法线贴图坐标值
		float2 uv_BumpMap;
	};
	//声明对主纹理图片的引用
	sampler2D _MainTex;
	//声明对法线贴图的引用
	sampler2D _BumpMap;
	//表面着色器函数
	void surf(Input IN, inout SurfaceOutput o) {
		//赋值颜色信息
		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		//赋值法线信息
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	}
	//标明CG程序的结束
	ENDCG
	}
}
