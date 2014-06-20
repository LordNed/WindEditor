#version 330 core

//Interpolated values from the vertex shaders
//in vec3 Color;
//in vec2 UV;



//Texture Sampler
//uniform sampler2D diffuseTextureSampler;

//Output Data
out vec4 outColor;

void main()
{
	outColor = vec4(1.0, 1.0, 1.0, 1.0);
}