#version 330 

//Input Data
in vec3 vertexPos;
in vec3 inColor;

//output data
out vec3 nColor;

void main()
{
	nColor = inColor;
	gl_Position = vec4(vertexPos, 1.0);
}