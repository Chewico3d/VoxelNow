#version 330 core
out vec4 FragColor;
  
in vec2 UVcoords;
in vec3 worldPosition;
in vec4 sunShadow;
in vec3 normal;
in vec2 planeUV;
in vec3 renderCoords;

in float scattering;

uniform sampler2D baseTexture;
uniform int textureSizeX;
uniform int textureSizeY;
uniform vec3 sunDir;

in vec4 objectProjection;
uniform mat4 invertedCameraMatrix;

vec3 sunColor = vec3(1,0.96,0.72);
vec3 ambientOclussionColor = vec3(0.9, 0.94,1);
float ambientOclussion = 0.7;


float CalculateScatter(){


    //vec3 rayDir = (vec4(objectProjection.xy / objectProjection.w, 1, 1) * invertedCameraMatrix).xyz;
    //rayDir = normalize(rayDir);


    return dot(-normal, sunDir);
}

float GetFog(){
    return objectProjection.z;
}

float GetVertexShadow(float x, float y){

    float upShadow = (1 - x) * sunShadow.y * sunShadow.y + x * sunShadow.w * sunShadow.w;
    float downShadow = (1 - x) * sunShadow.x * sunShadow.x + x * sunShadow.z * sunShadow.z;

    return downShadow * (1 - y) + upShadow * y;

}

void main()
{

    vec2 textureCoords = UVcoords / vec2(textureSizeX, textureSizeY);
    textureCoords -= (planeUV * 2 - vec2(1))/ vec2(textureSizeX, textureSizeY) * 0.01;

    vec4 textureCol = texture(baseTexture, textureCoords);
    if(textureCol.a < 0.5)
        discard;

    float sunColision = dot(sunDir, normal);
    sunColision = clamp(sunColision, 0, 1);

    vec3 col = textureCol.rgb * vec3(sunColision) * sunColor + textureCol.rgb * ambientOclussionColor * vec3(ambientOclussion);
    vec2 pixelatedUV = planeUV;
    col = col * GetVertexShadow(pixelatedUV.x, pixelatedUV.y) + clamp(GetFog() * 0.001,0,1);

    FragColor = vec4(col,1);

} 