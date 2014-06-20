#version 330 

//Input Data
in vec3 vertexPos;
in vec3 inColor;
in vec2 vertexUV;

//output data
out vec3 nColor;
out vec2 nTexCoord;

void main()
{
	nTexCoord = vertexUV;
	nColor = inColor;
	gl_Position = vec4(vertexPos, 1.0);
}