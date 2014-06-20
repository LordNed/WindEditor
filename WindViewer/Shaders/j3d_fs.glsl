#version 330 

//Interpolated values from the vertex shaders
in vec3 nColor;
in vec2 nTexCoord;


//Output Data
out vec4 finalColor;

uniform sampler2D tex;

void main()
{
	finalColor = texture(tex, nTexCoord) * vec4(nColor,1.0);
	//finalColor = vec4(1.0, 1.0, 1.0, 1.0);
}