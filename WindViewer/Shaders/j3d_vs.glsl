#version 330 core

//Input Data
in vec3 vertexPos;
//in vec3 color;
//in vec2 vertexTexCoord;

//output data
//out vec2 UV;
//out vec3 Color;

//Values that stay constant for the whole mesh.
uniform mat4 MVP;

void main()
{
	//Output position of the vertex, in clip space : MVP * position
	//gl_Position = MVP * vec4(vertexPos, 0, 1);

	//UV = vertexTexCoord;
	//Color = color;
	gl_Position = vec4(vertexPos, 1.0);
}