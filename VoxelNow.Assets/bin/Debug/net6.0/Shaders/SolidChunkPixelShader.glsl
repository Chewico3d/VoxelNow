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
    

    vec2 pixelatedPlaneUVcoord = floor (planeUV * 16) * 0.03125 * 2;
    float AOvalue = GetAmbientOcclusionValue(pixelatedPlaneUVcoord.x,pixelatedPlaneUVcoord.y) * .5;
    AOvalue = 1 - AOvalue * 0.7;

    vec3 col = texture(baseTexture, UVcoords / vec2(textureSizeX, textureSizeY)).rgb * AOvalue * (sunColision * .4 + .6);
    FragColor = vec4(col, 1);
} 