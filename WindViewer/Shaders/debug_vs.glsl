#version 330

in vec3 vertexPos;
out vec4 outColor;


uniform mat4 modelview;
uniform vec3 inColor;

void main()
{
	outColor = vec4(inColor,1);
	gl_Position = modelview * vec4(vertexPos, 1.0);
}