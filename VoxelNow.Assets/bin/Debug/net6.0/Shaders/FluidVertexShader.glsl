#version 330 core
layout (location = 0) in vec3 aPos;
  
uniform mat4 transform;

void main()
{
    gl_Position = vec4(aPos / 7, 1.0) * transform;
}