#version 330 

//Input Data
in vec3 vertexPos;
in vec4 inColor;
in vec2 vertexUV;

//output data
out vec4 nColor;
out vec2 nTexCoord;


uniform mat4 modelview;

void main()
{
	nTexCoord = vertexUV;
	nColor = inColor;
	gl_Position = modelview * vec4(vertexPos, 1.0);
	//gl_Position = vec4(vertexPos, 1.0);
}