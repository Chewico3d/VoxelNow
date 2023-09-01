#version 330 core
out vec4 FragColor;
  
in vec4 vertexColor; // the input variable from the vertex shader (same name and same type)
in vec2 UVcoords;

void main()
{
    
    FragColor = vec4(UVcoords, 1, 1);
} 