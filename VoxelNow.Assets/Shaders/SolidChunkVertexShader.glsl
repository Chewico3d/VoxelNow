#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 UV;
layout (location = 2) in int ambientOcclusion;
layout (location = 3) in int aNormal;
layout (location = 4) in vec2 aPlaneUV;

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
out vec4 AO;
uniform mat4 transform;


void main()
{
    worldPosition = aPos / 10;
    gl_Position = vec4(aPos / 7, 1.0) * transform; // see how we directly give a vec3 to vec4's constructor
    UVcoords = UV;
    
    int AOvalue0 = ambientOcclusion & 3;
    int AOvalue1 = (ambientOcclusion >> 2) & 3;
    int AOvalue2 = (ambientOcclusion >> 4) & 3;
    int AOvalue3 = (ambientOcclusion >> 6) & 3;

    AO = vec4(AOvalue0, AOvalue1, AOvalue2, AOvalue3);
    //AO = vec4(1,0,1,0);
    

    planeUV = aPlaneUV;

    normal = normalsByID[aNormal];
}