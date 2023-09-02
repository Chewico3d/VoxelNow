#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 UV;
layout (location = 2) in float ambientOcclusion;
layout (location = 3) in int aNormal;

vec3 normalsByID[6] = vec3[](
    vec3(-1,0,0),
    vec3(1,0,0),
    vec3(0,-1,0),
    vec3(0,1,0),
    vec3(0,0,-1),
    vec3(0,0,1) );
  
out vec2 UVcoords;
out float AO;
out vec3 worldPosition;
out vec3 normal;
uniform mat4 transform;

void main()
{
    worldPosition = aPos / 10;
    gl_Position = vec4(aPos / 7, 1.0) * transform; // see how we directly give a vec3 to vec4's constructor
    UVcoords = UV;
    AO = ambientOcclusion;

    normal = normalsByID[aNormal];
}