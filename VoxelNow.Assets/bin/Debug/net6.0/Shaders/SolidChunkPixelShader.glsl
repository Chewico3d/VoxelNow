#version 330 core
out vec4 FragColor;
  
in vec2 UVcoords;
in vec3 worldPosition;
in vec4 AO;
in vec3 normal;
in vec2 planeUV;

uniform sampler2D baseTexture;
uniform int textureSizeX;
uniform int textureSizeY;
uniform vec3 sunDir;


float GetAmbientOcclusionValue(float x, float y){
    float midUpX = mix(AO.g, AO.w, x);
    float midDownX = mix(AO.r, AO.b, x);

    return mix(midDownX, midUpX, y);
    
}

void main()
{
    float sunColision = dot(sunDir, normal);
    sunColision = clamp(sunColision, 0, 1);
    vec3 pixelWorldPosition = floor(worldPosition * 10) / 10 / 10;
    //float stilAO = clamp(1 - AO * .5, .5, 1);

    //vec3 col = texture(baseTexture, UVcoords / vec2(textureSizeX, textureSizeY)).rgb * stilAO;
    //col = clamp(pixelWorldPosition, 0, 1) * clamp(1 - AO * 0.4, 0, 1);

    vec2 pixelatedPlaneUVcoord = floor (planeUV * 10) * 0.1;
    float AOvalue = GetAmbientOcclusionValue(pixelatedPlaneUVcoord.x,pixelatedPlaneUVcoord.y);

    FragColor = vec4(AOvalue);
} 