Shader "Unlit/RadialSweepTimerShader"
{
        Properties
        {
            _MainTex ("Sprite Texture", 2D) = "white" {}
            _StartAngle ("Start Angle (Degrees)", Range(0, 360)) = 0 // 시작 각도
            _FillAngle ("Fill Angle (Degrees)", Range(0, 360)) = 360 // 부채꼴 각도
            _Color ("Color", Color) = (1, 1, 1, 1) // 색상
        }
    
        SubShader
        {
            Tags { "Queue"="Transparent" "RenderType"="Transparent" }
            LOD 200
    
            Pass
            {
                ZWrite Off    // 깊이 버퍼 비활성화
                Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩 설정
    
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
    
                sampler2D _MainTex; // 텍스처
                float _StartAngle;  // 시작 각도
                float _FillAngle;   // 부채꼴 각도
                float4 _Color;      // 색상
    
                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };
    
                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };
    
                v2f vert (appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }
    
                fixed4 frag (v2f i) : SV_Target
                {
                    // 중심 기준으로 좌표 변환
                    float2 uv = i.uv - 0.5; // 중심점 기준으로 좌표 변환
                    float angle = atan2(uv.y, uv.x) * (180.0 / UNITY_PI); // 각도 계산
                    if (angle < 0) angle += 360.0; // 음수 각도를 양수로 변환
                    float radius = length(uv); // 반지름 계산
    
                    // 시작 각도 및 끝 각도 계산
                    float endAngle = _StartAngle + _FillAngle; // 끝 각도
                    if (endAngle > 360.0) endAngle -= 360.0;   // 360도를 초과하면 wrap-around
    
                    // 각도 조건 확인 (시작부터 끝까지 채움)
                    bool inRange = false;
                    if (_FillAngle < 360.0) // 360도 미만일 경우
                    {
                        if (_StartAngle <= endAngle)
                        {
                            inRange = (angle >= _StartAngle && angle <= endAngle);
                        }
                        else
                        {
                            // 시작 각도가 끝 각도보다 클 경우 (0도 넘어가는 경우)
                            inRange = (angle >= _StartAngle || angle <= endAngle);
                        }
                    }
                    else
                    {
                        inRange = true; // 360도 전체일 경우 항상 참
                    }
    
                    if (radius > 0.5 || !inRange)
                    {
                        discard; // 조건 만족하지 않는 픽셀은 렌더링하지 않음
                    }
    
                    fixed4 col = tex2D(_MainTex, i.uv); // 텍스처 샘플링
                    col *= _Color; // 색상 곱하기
                    return col;
                }
                ENDCG
            }
        }
    
        FallBack "Transparent/VertexLit"
    }