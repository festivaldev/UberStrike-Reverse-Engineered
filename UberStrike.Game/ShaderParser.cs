using UnityEngine;

public static class ShaderParser {
	public static readonly string TestShaderString = " Shader Alex/TerrainThreeLayers No Lit {\nProperties {\n\t_Color (Main Color, Color) = (1,1,1,1)\n\t_BaseMap (BaseMap (RGB), 2D) = white {}\n\t_LightMap (LightMap (RGB), 2D) = white {}\n\t_Exposure (Exposure (0 ~ 5), Range(0, 2)) = 1\n\t_Bias (Bias between Base and Detail, Range(0, 1)) = 0.5\n\n\t_Control (Control 1 (RGB), 2D) = black {}\n\n\t_Splat0 (Layer 0 (R 1), 2D) = white {}\n\t_Splat1 (Layer 1 (G 1), 2D) = white {}\n\t_Splat2 (Layer 2 (B 1), 2D) = white {}\n}\n\nCategory {\n\n\tSubShader {\n\t\tPass {\nCGPROGRAM\n#pragma vertex LightmapSplatVertex\n#pragma fragment LightmapSplatFragment\n#pragma fragmentoption ARB_fog_exp2\n#pragma fragmentoption ARB_precision_hint_fastest\n\n#include UnityCG.cginc\n\n// Six splatmaps\nuniform float4 _Splat0_ST,_Splat1_ST,_Splat2_ST;\n\nstruct v2f {\n\t//float4 pos : POSITION;\n\t//float fog : FOGC;\n\tV2F_POS_FOG;\n\t\n\t// first uv for control, then 1 uv for every 2 splatmaps\n\tfloat4 uv[3] : TEXCOORD0;\n\tfloat4 color : COLOR;\n};\n\n\nuniform sampler2D _Control;\nuniform sampler2D _Splat0,_Splat1,_Splat2;\n\nvoid CalculateSplatUV (float2 baseUV, inout v2f o) \n{\n\to.uv[0].xy = baseUV;\n\t\n\to.uv[1].xy = TRANSFORM_TEX (baseUV, _Splat0);\n\to.uv[1].zw = TRANSFORM_TEX (baseUV, _Splat1);\n\to.uv[2].xy = TRANSFORM_TEX (baseUV, _Splat2);\n}\n\nhalf4 CalculateSplat (v2f i)\n{\n\thalf4 color = half4(0,0,0,0);\n\n\thalf4 control = tex2D (_Control, i.uv[0].xy);\n\t\n\tcolor += control.r * tex2D (_Splat0, i.uv[1].xy);\n\tcolor += control.g * tex2D (_Splat1, i.uv[1].zw);\n\tcolor += control.b * tex2D (_Splat2, i.uv[2].xy);\n\t\n\treturn color;\t\n}\n\nuniform sampler2D _BaseMap;\nuniform sampler2D _LightMap;\nuniform float _Exposure;\nuniform float _Bias;\n\nuniform float4 _Color;\n\nhalf4 LightmapSplatFragment (v2f i) : COLOR\n{\n\thalf4 col = CalculateSplat (i);\n\thalf4 base = tex2D (_BaseMap, i.uv[0].xy);\n\t\n\tcol = lerp(base, col, _Bias);\n\tcol *= tex2D (_LightMap, i.uv[0].xy);\n\tcol *= float4 (_Exposure,_Exposure,_Exposure, 0);\n\t\n\tcol.a = _Color.a;\n\t\n\treturn col;\n}\n\nv2f LightmapSplatVertex (appdata_base v)\n{\n\tv2f o;\n\t\n\tPositionFog( v.vertex, o.pos, o.fog );\n\tCalculateSplatUV (v.texcoord, o);\n\n\treturn o;\n}\n\nENDCG\n\t\t}\n\t\t\n \t}\n\n }\nFallback Off\n}\n";

	public static string GetProperties(Material mat) {
		var name = mat.shader.name;
		var num = name.Substring(name.IndexOf("Properties")).IndexOf("{");
		var num2 = 0;
		var num3 = 0;

		for (var i = num; i < name.Length; i++) {
			if (name[i] == '{') {
				num3++;
			} else if (name[i] == '}') {
				if (num3 == 0) {
					num2 = i;

					break;
				}

				num3--;
			}
		}

		return name.Substring(num, num2 - num);
	}
}
