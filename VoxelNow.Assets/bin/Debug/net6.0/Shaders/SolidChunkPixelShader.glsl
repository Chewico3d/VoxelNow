#version 330 core
out vec4 FragColor;
  
in vec2 UVcoords;
in vec3 worldPosition;
in float AO;
in vec3 normal;

uniform sampler2D baseTexture;
uniform int textureSizeX;
uniform int textureSizeY;

void main()
{

    vec3 pixelWorldPosition = floor(worldPosition * 10) / 10 / 10;

    vec3 col = texture(baseTexture, UVcoords / vec2(textureSizeX, textureSizeY)).rgb * clamp(1 - AO * 0.4, 0, 1);
    //col = clamp(pixelWorldPosition, 0, 1) * clamp(1 - AO * 0.4, 0, 1);
    FragColor = vec4(normal * 0.5 + 0.5, 1);
} 