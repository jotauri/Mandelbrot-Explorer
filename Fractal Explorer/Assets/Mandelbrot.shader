Shader "Jonny/Mandelbrot"
{
    // Mandelbrot shader by Jonny Hradek - 2019
    // License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
    // inspired by youtube tutorial by Martijn Steinrucken "The Art Of Code"
    // Part 1 youtube.com/watch?v=kY7liQVPQSc    
    // Part 2 youtube.com/watch?v=zmWkhlocBRY

    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Area("Area", vector) = (0, 0, 4, 4)
        _Repeats("Repeats", range(4,104)) = 20
        _Density("Color Density", range(.04, 1)) = .25
        _Flood("Flood", range(0, 1)) = .99
        _Red("Red", range(0,1)) = .75
        _Green("Green", range(0,1)) = .8
        _Blue("Blue", range(0,1)) = 0
        _FloodShape("Flood Shape", Float) = 0
        _Dissolve("Dissolve", range(1,2)) = 1
        _Movement("Movement", range(0,1)) = 0
        _Breathing("Breathing", range(0, 2)) = 0
        _Saturation("Saturation", range(0,1)) = 1
        _Shading("Shading", Float) = 0
        [Toggle] _Monochrom("Monochrom", Float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 _Area;
            float _Repeats, _Density, _Flood, _Red, _Green, _Blue, _Monochrom, _Shading,
            _FloodShape, _Breathing, _Movement, _Dissolve, _Saturation;
            sampler2D _MainTex;

            // rotate
            float2 rot(float2 p, float2 pivot, float a)
            {
                float s = sin(a);
                float c = cos(a);
                p -= pivot;
                p = float2(p.x *c-p.y *s, p.x *s+p.y *c);
                p+=pivot;
                return p;
            }

            fixed4 frag (v2f j) : SV_Target
            {
                float2 c = _Area.xy +(j.uv -.5) *_Area.zw;
                float2 z, zB;
                float i, r;
                r = (sin(_Time.y*_Breathing) *.5 +.7) * 100;
                
                for(i = 0; i < 100; i++)
                {
                    // Dissolve and Movement interfere,
                    // so if dissolve is off (1) we move normaly
                    // and if its on we move with distortions
                    if(_Movement == 0)
                        zB = rot(z, zB, _Dissolve);
                    else
                        {
                            if(_Dissolve == 1)
                                zB = rot(z, 0, _Time.y *_Movement);
                            else
                                zB = rot(z, zB, _Time.y *_Movement);
                        }
                        
                    z = float2(z.x *z.x - z.y *z.y, 2 *z.x *z.y) +c;

                    if(_FloodShape == 0) // waves
                        if(dot(zB, zB) > r) break;
                    if(_FloodShape == 1) // flowers                        
                        if(dot(z *z, zB *zB) > r*r) break;
                    if(_FloodShape == 2) // scales
                        if(abs(dot(z, zB)) > r) break;
                    if(_FloodShape == 3) // feathers                    
                        if(abs(z.x) > r) break;
                    if(_FloodShape == 4) // fungi
                        if(abs(dot(z *z, zB)) > r) break;
                    if(_FloodShape == 5) // coral
                        if((zB.x + z.y)*(zB.x + z.y) > r) break;
                }
                float m = pow(i/100, _Density);
                    
                float3 startCol = float3(_Red, _Green, _Blue);
                float3 col = 0;
                if (m <= _Flood)
                {
                    if(_Monochrom == 0)
                        col = sin(startCol *m *_Repeats) *.5 +.5;
                    else
                        col = startCol *m;
                }

                float extraShade = 1;
                if(_FloodShape > 2) extraShade = 0; // extra light shading only for waves/flowers/scales

                if(_Shading == 1)  // mixed
                    col *= 1 +sin(atan2(zB.x, z.y)) *.5;
                if(_Shading == 2)  // light
                    col *= 1 +sin(atan2(abs(zB.x +15 *extraShade), z.y)) *.5;
                if(_Shading == 3)  // dark
                    col *= 1 +sin(atan2(-abs(zB.x +15), z.y)) *.5;
                
                if(_Saturation != 1)
                {
                    float f = 1-_Saturation;
                    float L = .3*col.x+.6*col.y+.1*col.z;
                    col = float3(col.x+f*(L-col.x), col.y+f*(L-col.y), col.z+f*(L-col.z));
                }

                return float4(col, 1);
            }
            ENDCG
        }
    }
}
