//Tutorial : https://roystan.net/articles/toon-water
Shader "Custom/WaterShader_1"
{
    Properties
    {
        // _MainTex ("Texture", 2D) = "white" {}
        // _Color ("Color", Color) = (1,1,1,1)
        // _EdgeColor ("Edge Color", Color) = (1,1,1,1)

        // _Distance ("Distance", Float) = 1.0
        // _DepthFactor ("Depth Factor", Float) = -0.09

        _DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
        _DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1.0

        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        _SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777            //To Control where the noise occur

        _FoamDistance("Foam Distance", Float) = 0.4         //Control for what depth, the shoreline is visible
        
        _SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)             //Proprty to control scroll speed, in UVs per second
        _SurfaceDistortion("Surface Distortion", 2D) = "white"{}            //Two Channel Distortion Texture
        _SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27       //Control the multiply strength of the distortion.
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        ZWrite Off

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            // #pragma multi_compile_fog

            #include "UnityCG.cginc"
            
            
            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;
                float2 noiseUV : TEXCOORD1;
                float2 distortUV : TEXCOORD2;
                // float3 worldPos : TEXCOORD1;
                // UNITY_FOG_COORDS(1)
            };
            
            // sampler2D _MainTex;
            sampler2D _CameraDepthTexture;

            // float4 _MainTex_ST;
            // half _Distance;
            // half _DepthFactor;
            // fixed4 _Color, _EdgeColor;

            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;
            float _DepthMaxDistance;
            
            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;
            float _SurfaceNoiseCutoff;
            float _FoamDistance;

            float2 _SurfaceNoiseScroll;
            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;       //Tiling and Offset for sampler
            float _SurfaceDistortionAmount;

            v2f vert (appdata v)
            {
                v2f o;

                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                o.vertex = UnityObjectToClipPos(v.vertex);      //Convert Obj-Space to Camera-Clip space
                o.screenPos = ComputeScreenPos(o.vertex);           //Compute Depth
                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
                o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);

                // o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float depthSample01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r;
                float depthLinear = LinearEyeDepth(depthSample01);

                // float foamLine = 1 - saturate(_DepthFactor * (depthLinear - i.screenPos.w));
                // fixed4 col = _Color + foamLine * _EdgeColor;
                // return col;

                float depthDifference = depthLinear - i.screenPos.w;
                float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);
                float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference01);

                float foamDepthDifference01 = saturate(depthDifference / _FoamDistance);
                //To Convet from RGB([0] - [1]) range to Vector([-1] -[1]) range
                float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;          
                //Below, is so that the noise keeps scrolling as the time passes by
                float2 noiseUV = float2((i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x,
                    (i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);
                float4 surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;
                float surfaceNoiseCutOff = foamDepthDifference01 * _SurfaceNoiseCutoff;
                float surfaceNoise = surfaceNoiseSample > surfaceNoiseCutOff ? 1 : 0;

                return waterColor + surfaceNoise;
            }

            ENDCG
        }
    }
}
