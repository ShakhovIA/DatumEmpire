// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Nokdef/Screenspace Alpha"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_MainTextureChannel("Main Texture Channel", Vector) = (1,1,1,0)
		_MainTexturePanning("Main Texture Panning", Vector) = (0,0,0,0)
		_AlphaOverride("Alpha Override", 2D) = "white" {}
		_AlphaOverridePanning("Alpha Override Panning", Vector) = (0,0,0,0)
		_AlphaOverrideChannel("Alpha Override Channel", Vector) = (1,0,0,0)
		_DetailNoise("Detail Noise", 2D) = "white" {}
		_DetailNoisePanning("Detail Noise Panning", Vector) = (0,0,0,0)
		_DetailDistortionChannel("Detail Distortion Channel", Vector) = (1,0,0,0)
		_DistortionStrenght("Distortion Strenght", Float) = 0
		_DetailMultiplyChannel("Detail Multiply Channel", Vector) = (0,0,0,0)
		_DetailAdditiveChannel("Detail Additive Channel", Vector) = (0,0,0,0)
		_Desaturate("Desaturate?", Range( 0 , 1)) = 0

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
		AlphaToMask Off
		Cull Off
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_COLOR


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _MainTex;
			uniform float2 _MainTexturePanning;
			uniform sampler2D _DetailNoise;
			uniform float2 _DetailNoisePanning;
			uniform float4 _DetailDistortionChannel;
			uniform float _DistortionStrenght;
			uniform float4 _MainTextureChannel;
			uniform float _Desaturate;
			uniform float4 _DetailMultiplyChannel;
			uniform float4 _DetailAdditiveChannel;
			uniform sampler2D _AlphaOverride;
			uniform float2 _AlphaOverridePanning;
			uniform float4 _AlphaOverrideChannel;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord2 = screenPos;
				
				o.ase_color = v.color;
				o.ase_texcoord1 = v.ase_texcoord;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 texCoord81 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner80 = ( 1.0 * _Time.y * _DetailNoisePanning + texCoord81);
				float4 tex2DNode79 = tex2D( _DetailNoise, panner80 );
				float4 break17_g63 = tex2DNode79;
				float4 appendResult18_g63 = (float4(break17_g63.x , break17_g63.y , break17_g63.z , break17_g63.w));
				float4 clampResult19_g63 = clamp( ( appendResult18_g63 * _DetailDistortionChannel ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 break2_g63 = clampResult19_g63;
				float clampResult20_g63 = clamp( ( break2_g63.x + break2_g63.y + break2_g63.z + break2_g63.w ) , 0.0 , 1.0 );
				float DistortionNoise90 = clampResult20_g63;
				float4 screenPos = i.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 panner22 = ( 1.0 * _Time.y * _MainTexturePanning + ( ( DistortionNoise90 * _DistortionStrenght ) + ( (float2( 0,0 ) + ((ase_screenPosNorm).xy - float2( 0,0 )) * (float2( 1,1 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( 0,0 ))) * float2( 4,4 ) ) ));
				float4 tex2DNode150 = tex2D( _MainTex, panner22 );
				float4 break17_g67 = tex2DNode150;
				float4 appendResult18_g67 = (float4(break17_g67.x , break17_g67.y , break17_g67.z , break17_g67.w));
				float4 clampResult19_g67 = clamp( ( appendResult18_g67 * _MainTextureChannel ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 break2_g67 = clampResult19_g67;
				float clampResult20_g67 = clamp( ( break2_g67.x + break2_g67.y + break2_g67.z + break2_g67.w ) , 0.0 , 1.0 );
				float3 temp_cast_2 = (clampResult20_g67).xxx;
				float3 desaturateInitialColor158 = temp_cast_2;
				float desaturateDot158 = dot( desaturateInitialColor158, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar158 = lerp( desaturateInitialColor158, desaturateDot158.xxx, _Desaturate );
				float4 texCoord162 = i.ase_texcoord1;
				texCoord162.xy = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float4 break17_g65 = tex2DNode79;
				float4 appendResult18_g65 = (float4(break17_g65.x , break17_g65.y , break17_g65.z , break17_g65.w));
				float4 clampResult19_g65 = clamp( ( appendResult18_g65 * _DetailMultiplyChannel ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 break2_g65 = clampResult19_g65;
				float clampResult20_g65 = clamp( ( break2_g65.x + break2_g65.y + break2_g65.z + break2_g65.w ) , 0.0 , 1.0 );
				float ifLocalVar106 = 0;
				if( ( _DetailMultiplyChannel.x + _DetailMultiplyChannel.y + _DetailMultiplyChannel.z + _DetailMultiplyChannel.w ) <= 0.0 )
				ifLocalVar106 = 1.0;
				else
				ifLocalVar106 = clampResult20_g65;
				float MultiplyNoise92 = ifLocalVar106;
				float4 break17_g66 = tex2DNode79;
				float4 appendResult18_g66 = (float4(break17_g66.x , break17_g66.y , break17_g66.z , break17_g66.w));
				float4 clampResult19_g66 = clamp( ( appendResult18_g66 * _DetailAdditiveChannel ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 break2_g66 = clampResult19_g66;
				float clampResult20_g66 = clamp( ( break2_g66.x + break2_g66.y + break2_g66.z + break2_g66.w ) , 0.0 , 1.0 );
				float AdditiveNoise91 = clampResult20_g66;
				float4 break166 = ( ( i.ase_color * float4( desaturateVar158 , 0.0 ) * ( texCoord162.z + 1.0 ) * MultiplyNoise92 ) + AdditiveNoise91 );
				float4 texCoord60 = i.ase_texcoord1;
				texCoord60.xy = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 texCoord43 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner44 = ( 1.0 * _Time.y * _AlphaOverridePanning + texCoord43);
				float4 tex2DNode45 = tex2D( _AlphaOverride, panner44 );
				float4 break2_g64 = ( tex2DNode45 * _AlphaOverrideChannel );
				float AlphaOverride49 = saturate( ( break2_g64.x + break2_g64.y + break2_g64.z + break2_g64.w ) );
				float temp_output_3_0_g68 = ( texCoord60.w - ( 1.0 - AlphaOverride49 ) );
				float4 appendResult167 = (float4(break166.r , break166.g , break166.b , ( i.ase_color.a * saturate( saturate( ( temp_output_3_0_g68 / fwidth( temp_output_3_0_g68 ) ) ) ) * AlphaOverride49 )));
				
				
				finalColor = appendResult167;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
1249;140;2560;1389;906.3856;827.6106;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;103;-784.2378,-2597.328;Inherit;False;1462.886;1030;Extra Noise Setup;14;106;105;86;91;87;92;90;79;85;80;83;81;84;108;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;84;-734.2378,-2214.328;Inherit;False;Property;_DetailNoisePanning;Detail Noise Panning;7;0;Create;True;0;0;0;False;0;False;0,0;-1,-1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;81;-631.819,-2456.415;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;83;-482.86,-2241.615;Inherit;True;Property;_DetailNoise;Detail Noise;6;0;Create;True;0;0;0;False;0;False;None;e38401949bf84a249b2f95cf9c6d1726;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;80;-391.8206,-2408.415;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;85;-180.2379,-2145.328;Inherit;False;Property;_DetailDistortionChannel;Detail Distortion Channel;8;0;Create;True;0;0;0;False;0;False;1,0,0,0;1,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;79;-200.3228,-2353.517;Inherit;True;Property;_TextureSample3;Texture Sample 3;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;34;-2955.764,-1202.928;Inherit;False;2252.64;1173.84;Main Texture Set Vars;17;149;23;147;27;110;145;146;138;135;22;136;10;111;109;150;151;156;;0,0.5461459,1,1;0;0
Node;AmplifyShaderEditor.FunctionNode;154;153.6483,-2241.953;Inherit;False;Channel Picker;-1;;63;dc5f4cb24a8bdf448b40a1ec5866280e;0;2;5;FLOAT4;1,0,0,0;False;7;FLOAT4;0,0,0,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;145;-2846.101,-404.1232;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;50;-642.348,-1541.705;Inherit;False;1249.023;565.425;Alpha Override;8;43;44;45;47;48;49;51;157;;0,0.5461459,1,1;0;0
Node;AmplifyShaderEditor.ComponentMaskNode;146;-2604.829,-359.4064;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;90;416.6483,-2215.953;Inherit;False;DistortionNoise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;43;-592.348,-1491.705;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;149;-2394.645,-634.368;Inherit;False;Property;_DistortionStrenght;Distortion Strenght;9;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;51;-616.1282,-1132.945;Inherit;False;Property;_AlphaOverridePanning;Alpha Override Panning;4;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;135;-2339.662,-547.2616;Inherit;False;90;DistortionNoise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;147;-2375.829,-403.4063;Inherit;True;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;0,0;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;151;-2072.593,-317.8097;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;4,4;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;47;-399.7247,-1273.259;Inherit;True;Property;_AlphaOverride;Alpha Override;3;0;Create;True;0;0;0;False;0;False;None;8192d3c793047b449a685ae19102dfe6;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;44;-352.3488,-1443.705;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-2124.063,-659.4615;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;48;-97.32484,-1188.281;Inherit;False;Property;_AlphaOverrideChannel;Alpha Override Channel;5;0;Create;True;0;0;0;False;0;False;1,0,0,0;1,0,0,0.5;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;23;-2017.901,-501.3873;Inherit;False;Property;_MainTexturePanning;Main Texture Panning;2;0;Create;True;0;0;0;False;0;False;0,0;0.2,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;138;-1995.662,-662.2618;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;86;-593.2379,-1899.328;Inherit;False;Property;_DetailMultiplyChannel;Detail Multiply Channel;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;45;-160.851,-1388.807;Inherit;True;Property;_TextureSample2;Texture Sample 2;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;105;-231.1713,-1862.338;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;157;239.2312,-1200.401;Inherit;False;Channel Picker Alpha;-1;;64;e49841402b321534583d1dc019041b68;0;2;5;FLOAT4;1,0,0,0;False;7;FLOAT4;0,0,0,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;153;-13.35168,-1929.953;Inherit;False;Channel Picker;-1;;65;dc5f4cb24a8bdf448b40a1ec5866280e;0;2;5;FLOAT4;1,0,0,0;False;7;FLOAT4;0,0,0,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;22;-1683.138,-518.7701;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;27;-1773.575,-855.0742;Inherit;True;Property;_MainTex;Main Texture;0;0;Create;False;0;0;0;False;0;False;None;6197da77867dbe143aee9b59b0566b9d;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;108;-228.1713,-1713.338;Inherit;False;Constant;_Float0;Float 0;15;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;87;-153.2379,-2547.328;Inherit;False;Property;_DetailAdditiveChannel;Detail Additive Channel;11;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.1,0.1,0.25,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;106;259.8287,-1914.338;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;382.6752,-1380.28;Inherit;False;AlphaOverride;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;150;-1508.96,-631.8519;Inherit;True;Property;_TextureSample0;Texture Sample 0;12;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;10;-1419.139,-362.7703;Inherit;False;Property;_MainTextureChannel;Main Texture Channel;1;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;159;-462.6389,-257.7749;Inherit;False;Property;_Desaturate;Desaturate?;12;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;162;-382.7326,-134.1047;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;92;431.6483,-1884.953;Inherit;False;MultiplyNoise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;155;132.2033,-2464.99;Inherit;False;Channel Picker;-1;;66;dc5f4cb24a8bdf448b40a1ec5866280e;0;2;5;FLOAT4;1,0,0,0;False;7;FLOAT4;0,0,0,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;156;-1069.835,-363.9704;Inherit;False;Channel Picker;-1;;67;dc5f4cb24a8bdf448b40a1ec5866280e;0;2;5;FLOAT4;1,0,0,0;False;7;FLOAT4;0,0,0,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;-304.4512,205.567;Inherit;False;49;AlphaOverride;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;91;400.6483,-2501.953;Inherit;False;AdditiveNoise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;98;-292.8669,32.71658;Inherit;False;92;MultiplyNoise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;163;-149.0954,-220.5375;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;158;-165.6389,-328.7749;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VertexColorNode;37;-168.5735,-497.7309;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;64;31.54882,205.567;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;60;15.54882,285.5669;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;101;136.5273,-200.7529;Inherit;False;91;AdditiveNoise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;170.5334,-373.1786;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;70;287.5488,253.5669;Inherit;False;Step Antialiasing;-1;;68;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;61;270.9582,23.08784;Inherit;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;68;495.5488,285.5669;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;102;408.6009,-362.8981;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;627.2988,159.4486;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;166;591.6144,-269.6106;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;111;-864.0005,-549.252;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;152;158.6752,-1380.28;Inherit;False;Channel Picker;-1;;69;dc5f4cb24a8bdf448b40a1ec5866280e;0;2;5;FLOAT4;1,0,0,0;False;7;FLOAT4;0,0,0,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;167;819.6144,-81.6106;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;110;-1053,-596.252;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-1181,-596.252;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;168;1055.01,-161.7703;Float;False;True;-1;2;ASEMaterialInspector;100;1;Nokdef/Screenspace Alpha;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;80;0;81;0
WireConnection;80;2;84;0
WireConnection;79;0;83;0
WireConnection;79;1;80;0
WireConnection;154;5;79;0
WireConnection;154;7;85;0
WireConnection;146;0;145;0
WireConnection;90;0;154;0
WireConnection;147;0;146;0
WireConnection;151;0;147;0
WireConnection;44;0;43;0
WireConnection;44;2;51;0
WireConnection;136;0;135;0
WireConnection;136;1;149;0
WireConnection;138;0;136;0
WireConnection;138;1;151;0
WireConnection;45;0;47;0
WireConnection;45;1;44;0
WireConnection;105;0;86;1
WireConnection;105;1;86;2
WireConnection;105;2;86;3
WireConnection;105;3;86;4
WireConnection;157;5;45;0
WireConnection;157;7;48;0
WireConnection;153;5;79;0
WireConnection;153;7;86;0
WireConnection;22;0;138;0
WireConnection;22;2;23;0
WireConnection;106;0;105;0
WireConnection;106;2;153;0
WireConnection;106;3;108;0
WireConnection;106;4;108;0
WireConnection;49;0;157;0
WireConnection;150;0;27;0
WireConnection;150;1;22;0
WireConnection;92;0;106;0
WireConnection;155;5;79;0
WireConnection;155;7;87;0
WireConnection;156;5;150;0
WireConnection;156;7;10;0
WireConnection;91;0;155;0
WireConnection;163;0;162;3
WireConnection;158;0;156;0
WireConnection;158;1;159;0
WireConnection;64;0;52;0
WireConnection;164;0;37;0
WireConnection;164;1;158;0
WireConnection;164;2;163;0
WireConnection;164;3;98;0
WireConnection;70;1;64;0
WireConnection;70;2;60;4
WireConnection;68;0;70;0
WireConnection;102;0;164;0
WireConnection;102;1;101;0
WireConnection;40;0;61;4
WireConnection;40;1;68;0
WireConnection;40;2;52;0
WireConnection;166;0;102;0
WireConnection;111;0;110;0
WireConnection;111;1;110;1
WireConnection;111;2;110;2
WireConnection;111;3;110;3
WireConnection;152;5;45;0
WireConnection;152;7;48;0
WireConnection;167;0;166;0
WireConnection;167;1;166;1
WireConnection;167;2;166;2
WireConnection;167;3;40;0
WireConnection;110;0;109;0
WireConnection;109;0;150;0
WireConnection;109;1;10;0
WireConnection;168;0;167;0
ASEEND*/
//CHKSM=8D89DA3008D5FE9A43D88796538487BFCEC6FD91