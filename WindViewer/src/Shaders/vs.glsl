#version 330

in vec3 vPosition;
uniform mat4 modelview;

void
main()
{
	gl_Position = modelview * vec4(vPosition, 1.0);
}