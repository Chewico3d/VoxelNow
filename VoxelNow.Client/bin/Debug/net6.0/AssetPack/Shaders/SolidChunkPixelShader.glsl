#version 330 core
out vec4 FragColor;
  
in vec2 UVcoords;
in vec3 worldPosition;
in float AO;
in vec3 normal;

uniform sampler2D baseTexture;
uniform int textureSizeX;
uniform int textureSizeY;
uniform vec3 sunDir;

void main()
{
    float sunColision = dot(sunDir, normal);
    sunColision = clamp(sunColision, 0, 1);
    vec3 pixelWorldPosition = floor(worldPosition * 10) / 10 / 10;
    float stilAO = clamp(1 - AO * .5, .5, 1);

    vec3 col = texture(baseTexture, UVcoords / vec2(textureSizeX, textureSizeY)).rgb * stilAO;
    //col = clamp(pixelWorldPosition, 0, 1) * clamp(1 - AO * 0.4, 0, 1);
    FragColor = vec4(col * (sunColision * 0.5 + 0.5), 1);
} 