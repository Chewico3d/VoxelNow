#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 UV;
layout (location = 2) in float ambientOcclusion;
  
out vec2 UVcoords;
out float AO;
out vec3 worldPosition;
uniform mat4 transform;

void main()
{
    worldPosition = aPos / 10;
    gl_Position = vec4(aPos / 7, 1.0) * transform; // see how we directly give a vec3 to vec4's constructor
    UVcoords = UV;
    AO = ambientOcclusion;
}