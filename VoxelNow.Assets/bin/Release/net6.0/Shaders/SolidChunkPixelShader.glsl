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

    float AOvalue = GetAmbientOcclusionValue(planeUV.x, planeUV.y) * .25;
    AOvalue = clamp(1 - AOvalue,0,1);

    vec2 textureCoords = UVcoords / vec2(textureSizeX, textureSizeY);
    textureCoords -= (planeUV * 2 - vec2(1))/ vec2(textureSizeX, textureSizeY) * 0.01;
    vec3 col = texture(baseTexture, textureCoords).rgb * AOvalue * (sunColision * .4 + .6);
    FragColor = vec4(col, 1);
} 