Shader "TextEffect/Text2DOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        
        [Toggle(USE_GREY)] _Grey ("Use Grey", Float) = 0
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
        _OutlineScaleFactor("outlien scale factor",float) = 0.1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        // Blend Off
        ColorMask [_ColorMask]

        Pass
        {
            Name "OUTLINE"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            //Add for RectMask2D
            #include "UnityUI.cginc"
            //End for RectMask2D

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _MainTex_TexelSize;
            float _Grey;
            float _OutlineScaleFactor;
            
            //Add for RectMask2D
            float4 _ClipRect;
            //End for RectMask2D

            struct appdata
            {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float4 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 uv2 : TEXCOORD2;
                float4 uv3 : TEXCOORD3;
                fixed4 color : COLOR;
            };


            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 tangent : TANGENT;
                float4 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 uv2 : TEXCOORD2;
                float4 uv3 : TEXCOORD3;
                //Add for RectMask2D
                float4 worldPosition : TEXCOORD4;
                //End for RectMask2D
                fixed4 color : COLOR;
            };

            v2f vert(appdata IN)
            {
                v2f o;

                //Add for RectMask2D
                o.worldPosition = IN.vertex;
                //End for RectMask2D

                o.vertex = UnityObjectToClipPos(IN.vertex);
                o.tangent = IN.tangent;
                o.texcoord = IN.texcoord;
                o.color = IN.color;
                o.uv1 = IN.uv1;
                o.uv2 = IN.uv2;
                o.uv3 = IN.uv3;
                o.normal = IN.normal;
                return o;
            }

            fixed IsInRect(float2 pPos, float2 pClipRectMin, float2 pClipRectMax)
            {
                pPos = step(pClipRectMin, pPos) * step(pPos, pClipRectMax);
                return pPos.x * pPos.y;
            }

            fixed SampleAlpha(int pIndex, v2f IN, float width, fixed4 outlineColor)
            {
                const fixed sinArray[12] = { 0, 0.5, 0.866, 1, 0.866, 0.5, 0, -0.5, -0.866, -1, -0.866, -0.5 };
                const fixed cosArray[12] = { 1, 0.866, 0.5, 0, -0.5, -0.866, -1, -0.866, -0.5, 0, 0.5, 0.866 };
                float2 pos = IN.texcoord + _MainTex_TexelSize.xy * float2(cosArray[pIndex], sinArray[pIndex]) * width;	//normal.z 存放 _OutlineWidth
                return IsInRect(pos, IN.uv1, IN.uv2) * (tex2D(_MainTex, pos) + _TextureSampleAdd).a * outlineColor.a;		//tangent.w 存放 _OutlineColor.w
            }

            fixed4 gammaToLinear(fixed4 gammaColor) {
                return fixed4(pow(gammaColor.rgb, 2.2), gammaColor.a);
            }
            
            fixed4 linearToGamma(fixed4 linearColor) {
                return fixed4(pow(linearColor.rgb, 1.0 / 2.2), linearColor.a);                
            }
            
            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;//默认的文字颜色
                // 使用uv3.w来区分当前顶点是文字还是阴影
                float outlineWidthOut = IN.normal.x;
                float shadowOutlineWidthOut = IN.normal.y;               
                fixed4 outlineColorOut = gammaToLinear(IN.uv3);
                fixed4 shadowOutlineColorOut = gammaToLinear(IN.tangent);                
                float param = IN.uv2.w;
                
                if ((param > -1 && outlineWidthOut > 0) || (param == -1 && shadowOutlineWidthOut > 0))	//normal.z 存放 _OutlineWidth
                {
                    color.w *= IsInRect(IN.texcoord, IN.uv1, IN.uv2);	//uv1 uv2 存着原始字的uv长方形区域大小

                    fixed4 outlineColor = IN.uv3.x == -1 ?shadowOutlineColorOut : outlineColorOut;
                    half4 val = half4(outlineColor.rgb, 0);

                    float outlineWidth = IN.uv3.x == -1 ? shadowOutlineWidthOut : outlineWidthOut;

                    float pixelScale = _ScreenParams.x / 1080.0;
                    outlineWidth = outlineWidth * _OutlineScaleFactor / IN.uv1.w * pixelScale * 10.0;
                    
                    //val 是 _OutlineColor的rgb，a是后面计算的
                    val.w += SampleAlpha(0, IN, outlineWidth, outlineColor);
                    val.w += SampleAlpha(1, IN, outlineWidth, outlineColor);
                    val.w += SampleAlpha(2, IN, outlineWidth, outlineColor);
                    val.w += SampleAlpha(3, IN, outlineWidth, outlineColor);
                    val.w += SampleAlpha(4, IN, outlineWidth, outlineColor);
                    val.w += SampleAlpha(5, IN, outlineWidth, outlineColor);
                    val.w += SampleAlpha(6, IN, outlineWidth, outlineColor);
                    val.w += SampleAlpha(7, IN, outlineWidth, outlineColor);
                    val.w += SampleAlpha(8, IN, outlineWidth, outlineColor);
                    val.w += SampleAlpha(9, IN, outlineWidth, outlineColor);
                    val.w += SampleAlpha(10, IN, outlineWidth, outlineColor);
                    val.w += SampleAlpha(11, IN, outlineWidth, outlineColor);

                    color = (val * (1.0 - color.a)) + (color * color.a);
                    color.a = saturate(color.a);
                    color.a *= IN.color.a;	//字逐渐隐藏时，描边也要隐藏
                }

                 if(_Grey == 1)
                 {
                     color.rgb = dot(color.rgb, fixed3(0.3, 0.59, 0.11));              
                 }

                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #ifdef UNITY_UI_ALPHACLIP
                    clip(color.a - 0.001);
                #endif
                //End for RectMask2D

                return color;
            }

            ENDCG
        }
    }
} 