#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in uint normalID;
layout (location = 2) in vec2 UVs;
layout (location = 3) in float aO;
  
out vec3 pos;
out vec3 normal;
out vec2 texturesPos;
out float ambientOclusion;


const vec3 normalTable[6] = vec3[](
    vec3(-1.0, 0.0, 0.0),
    vec3( 1.0, 0.0, 0.0),
    vec3( 0.0,-1.0, 0.0),
    vec3( 0.0, 1.0, 0.0),
    vec3( 0.0, 0.0,-1.0),
    vec3( 0.0, 0.0, 1.0)
); 

uniform mat4 transform;

void main()
{
    gl_Position = vec4(aPos, 1.0) * transform; 
    normal = normalTable[normalID];
    texturesPos = UVs;
    pos = aPos;
    ambientOclusion = aO;
}