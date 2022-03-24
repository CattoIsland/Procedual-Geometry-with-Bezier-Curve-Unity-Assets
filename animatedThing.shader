Shader "Unlit/animatedThing"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveAmp("Wave Amp", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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
            
            
            static float TAU = 6.28318530718f; //one turn in radians
            sampler2D _MainTex;
            float4 _MainTex_ST;

           
            float _WaveAmp;

        
           v2f vert (appdata v)
            {
                v2f o;


                //Sine Wave Animation on Generated Geometry written into Shader
                float wave = sin(v.uv.x * TAU * 8 + _Time.y) * v.uv.y * _WaveAmp;
                 
                v.vertex.z += wave; 

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

         

            fixed4 frag(v2f i) : SV_Target
            {
                //Animation Code
            

               // float wave = sin(i.uv.x * TAU *8 + _Time.y) * i.uv.y ;
            //return wave * 0.5 + 0.5;
                
                //END Animation Code 


                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
