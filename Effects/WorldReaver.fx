sampler uImage0 : register(s0);
float3 uColor;
float uOpacity;
float3 uSecondaryColor;
float uTime;
float2 uScreenResolution : register(c0);
float2 uScreenPosition;
float2 uTargetPosition;
float2 uImageOffset;
float uIntensity;
float uProgress : register(c1);
float2 uDirection;
float2 uZoom;

float distanceBelowCut(float2 coords)
{
    float offsetX = (coords.x - 0.5f) * uScreenResolution.x;
    float cutHeight = 0.5f * uScreenResolution.y - 2.0f * offsetX;
    return coords.y * uScreenResolution.y - cutHeight;
}

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    if (uProgress <= 60.0f)
    {
        return tex2D(uImage0, coords);
    }

    float whiteout = 0.0f;
    if (uProgress > 270.0f)
    {
        whiteout = (uProgress - 270.0f) / 30.0f;
        if (whiteout > 1.0f)
        {
            whiteout = 1.0f;
        }
    }

    float separation = 1.0f;
    if (uProgress <= 90.0f)
    {
        separation = (uProgress - 60.0f) / 30.0f;
    }
    separation = 25.0f * (1.0f - cos(3.14159265f * separation));

    float offset = distanceBelowCut(coords);
    if (abs(offset) < separation)
    {
        float4 black = { whiteout, whiteout, whiteout, 1 };
        return black;
    }
    float2 useCoords = { coords.x, coords.y };
    if (offset >= 0)
    {
        useCoords.y -= separation / uScreenResolution.y;
    }
    else
    {
        useCoords.y += separation / uScreenResolution.y;
    }
    if (useCoords.y < 0 || useCoords.y > 1)
    {
        float4 black = { whiteout, whiteout, whiteout, 1 };
        return black;
    }
    return tex2D(uImage0, useCoords);
}

technique Technique1
{
    pass WorldReaver
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}