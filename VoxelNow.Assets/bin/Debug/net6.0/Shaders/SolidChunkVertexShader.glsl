#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 UV;
layout (location = 2) in int aSunShadow;
layout (location = 3) in int aNormal;
layout (location = 4) in vec2 aPlaneUV;
layout (location = 5) in int materialProps;

vec3 normalsByID[6] = vec3[](
    vec3(-1,0,0),
    vec3(1,0,0),
    vec3(0,-1,0),
    vec3(0,1,0),
    vec3(0,0,-1),
    vec3(0,0,1) );
  
out vec2 UVcoords;
out vec3 worldPosition;
out vec3 normal;
out vec2 planeUV;
out vec4 sunShadow;

//not used
out float scattering;
out vec4 objectProjection;

uniform mat4 transform;

void main()
{

    UVcoords = UV;
    worldPosition = aPos / 10;
    gl_Position = vec4(aPos / 7, 1.0) * transform; 
    objectProjection = gl_Position;
    
    scattering = 1 & materialProps;

    float v0Shadow = float((aSunShadow) & 15) / 16;
    float v1Shadow = float((aSunShadow >> 4) & 15)/ 16;
    float v2Shadow = float((aSunShadow >> 8) & 15)/ 16;
    float v3Shadow = float((aSunShadow >> 12) & 15)/ 16;

    sunShadow = vec4(v0Shadow, v1Shadow, v2Shadow, v3Shadow);

    planeUV = aPlaneUV;
    normal = normalsByID[aNormal];
}