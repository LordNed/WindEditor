#version 330 core

//Interpolated values from the vertex shaders
in vec2 UV;

//Output Data
out vec3 color;

//Texture Sampler
uniform sampler2D diffuseTextureSampler;


void main()
{
	color = texture(diffuseTextureSampler, UV).rgb;
}