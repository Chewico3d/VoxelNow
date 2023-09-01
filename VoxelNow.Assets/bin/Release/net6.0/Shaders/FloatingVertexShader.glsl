#version 330 core
layout (location = 0) in vec3 aPos; // the position variable has attribute position 0
  
out vec4 vertexColor; // specify a color output to the fragment shader
uniform mat4 transform;

void main()
{
    gl_Position = vec4(aPos, 1.0) * transform; // see how we directly give a vec3 to vec4's constructor
    vertexColor = vec4(aPos, 1.0); // set the output variable to a dark-red color
}