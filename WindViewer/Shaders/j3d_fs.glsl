#version 330 

//Interpolated values from the vertex shaders
in vec3 nColor;

//Output Data
out vec4 finalColor;

void main()
{
	finalColor = vec4(nColor,1.0);
	//finalColor = vec4(1.0, 1.0, 1.0, 1.0);
}