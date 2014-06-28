#version 330

in vec3 vertexPos;
out vec4 outColor;

uniform vec3 color;
uniform mat4 modelview;

void main()
{
	gl_Position = modelview * vec4(vertexPos, 1.0);
	outColor = vec4(color,1);
}