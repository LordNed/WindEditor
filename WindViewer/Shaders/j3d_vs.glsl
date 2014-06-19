#version 330 core

//Input Data
in vec3 vertexPos;
in vec2 vertexUV;

//output data
out vec2 UV;

//Values that stay constant for the whole mesh.
uniform mat4 MVP;

void main()
{
	//Output position of the vertex, in clip space : MVP * position
	gl_Position = MVP * vec4(vertexPos, 1);

	UV = vertexUV;
}