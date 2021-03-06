#version 300 es

precision mediump float;
uniform mediump isampler2D u_image;
uniform float ww;
uniform float wc;
uniform float slope;
uniform float intercept;
in vec2 v_texCoord;
out vec4 fragColor;

void main() {
  // color is packed into red channel
  float color = float(texture(u_image, v_texCoord).r);
  color = color * slope + intercept;
  color = (color - (wc - 0.5)) / (max(ww, 1.0)) + 0.5;
  color = clamp(color, 0.0, 1.0);

  fragColor = vec4(color, color, color, 1.0);
}