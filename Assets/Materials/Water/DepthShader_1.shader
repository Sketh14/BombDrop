Shader "Custom/DepthShader_1"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Depth ("Depth Fade", Float) = 1.0
        _Distance ("Distance", Float) = 1.0
        _DepthFactor ("Depth Factor", Float) = -0.09
        _Fix ("Depth Distance", Float) = -0.09
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        ZWrite Off

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            // #pragma surface surf Standard fullforwardshadows
            
            // Use shader model 3.0 target, to get nicer looking lighting
            // #pragma target 3.0

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _CameraDepthTexture;
            
            half _Depth;
            half _DepthFactor;
            half _Distance;
            half _Fix;
            fixed4 _Color;
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
            };
            
            // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
            // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
            // #pragma instancing_options assumeuniformscaling
            // UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
            // UNITY_INSTANCING_BUFFER_END(Props)
            
            v2f vert(appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = ComputeScreenPos(o.pos);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
            
            /*half4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv.xy / i.uv.w;
                
                half lin = LinearEyeDepth(tex2D(_CameraDepthTexture, uv).r);
                half dist =  i.uv.w - _Fix;
                half depth = lin - dist;
                
                
                return lerp(half4(1,1,1,0), _Color, saturate(depth * _Depth));
            }*/
            
            /*fixed4 frag (v2f i) : SV_Target
            {
                float2 screenUVs = i.uv.xy / i.uv.w;
                // float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.uv)));
                float sceneZ = LinearEyeDepth(tex2D(_CameraDepthTexture, screenUVs).w);
                float waterZ = sceneZ - i.uv.w;

                float distanceDivided = waterZ / _Distance;

                fixed4 col = _Color;
                col.a = saturate(distanceDivided);
                return col;
            }*/

            fixed4 frag (v2f i) : SV_Target
            {
                float depthSample = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.uv));
                float depth = LinearEyeDepth(depthSample).r;
                // float foamLine = 1 - saturate(_DepthFactor * (depth - i.uv.w));

                float foamLine = 1 - saturate(_DepthFactor * (_Distance - i.worldPos.y));
                // fixed4 col = _Color + foamLine * _EdgeColor;                 //To give below the "_Distance" separate color
                fixed4 col = _Color;
                col.a = foamLine;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
