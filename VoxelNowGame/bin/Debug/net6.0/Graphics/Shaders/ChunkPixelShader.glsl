#version 330 core
out vec4 FragColor;
  
in vec3 normal;
in vec3 pos;
in vec2 texturesPos;
in vec4 transformedPosition;
in float ambientOclusion;

uniform sampler2D maintext;
uniform int tiledTextureSizeX;
uniform int tiledTextureSizeY;

void main()
{
    vec3 sun = normalize(vec3(2,6,3));
    float FOG = transformedPosition.z / 500;
    float i = dot(normal, sun) * 0.5 + 0.5;
    vec2 textureCordinates = texturesPos / vec2(tiledTextureSizeX, tiledTextureSizeY);
    FragColor = vec4(mix(texture(maintext, textureCordinates).rgb * (1 - ambientOclusion / 400) * i, vec3(1,1,1), FOG),1);
}