using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Rendering {
    using OpenTK.Graphics.OpenGL4;
    using OpenTK.Mathematics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace VallVoxel.Client.Graphics {
        internal class Shader {
            int Handle;
            Dictionary<string, int> uniformReference = new Dictionary<string, int>();
            internal Shader(string vertexPath, string fragmentPath) {
                if (!File.Exists(vertexPath))
                    throw new Exception("vertex path : " + vertexPath + " not fount");
                if (!File.Exists(fragmentPath))
                    throw new Exception("vertex path : " + fragmentPath + " not fount");
                string VertexShaderSource = File.ReadAllText(vertexPath);

                string FragmentShaderSource = File.ReadAllText(fragmentPath);
                int VertexShader;

                VertexShader = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(VertexShader, VertexShaderSource);
                int FragmentShader;

                FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(FragmentShader, FragmentShaderSource);

                GL.CompileShader(VertexShader);

                GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
                if (success == 0) {
                    string infoLog = GL.GetShaderInfoLog(VertexShader);
                    Console.WriteLine(infoLog);
                    Console.WriteLine(vertexPath);
                }

                GL.CompileShader(FragmentShader);

                GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
                if (success == 0) {
                    string infoLog = GL.GetShaderInfoLog(FragmentShader);
                    Console.WriteLine(infoLog);
                    Console.WriteLine(fragmentPath);
                }
                Handle = GL.CreateProgram();

                GL.AttachShader(Handle, VertexShader);
                GL.AttachShader(Handle, FragmentShader);

                GL.LinkProgram(Handle);

                GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
                if (success == 0) {
                    string infoLog = GL.GetProgramInfoLog(Handle);
                    Console.WriteLine(infoLog);
                }
                GL.DetachShader(Handle, VertexShader);
                GL.DetachShader(Handle, FragmentShader);
                GL.DeleteShader(FragmentShader);
                GL.DeleteShader(VertexShader);
            }

            public void Use() {

                GL.UseProgram(Handle);
            }

            internal void SetTransformationMatrix(Matrix4 transformation) {
                SetMatrix4("transform", transformation);
            }
            internal void SetMatrix4(string Name, Matrix4 matrix4) {

                Use();
                if (uniformReference.TryGetValue(Name, out int value)) {
                    if (value == -1)
                        return;
                    GL.UniformMatrix4(value, true, ref matrix4);
                    return;
                }

                value = GL.GetUniformLocation(Handle, Name);

                if (value == -1) {
                    Console.WriteLine("Error: not found " + Name + " in shader");
                }
                uniformReference.Add(Name, value);
                GL.UniformMatrix4(value, true, ref matrix4);

            }
            internal void SetUniform1(string Name, int uniform) {

                Use();
                if (uniformReference.TryGetValue(Name, out int value)) {
                    if (value == -1)
                        return;
                    GL.Uniform1(value, uniform);
                    return;
                }

                value = GL.GetUniformLocation(Handle, Name);

                if (value == -1) {
                    Console.WriteLine("Error: not found " + Name + " in shader");
                }
                uniformReference.Add(Name, value);
                GL.Uniform1(value, uniform);

            }
            internal void SetUniform3f(string Name, float x, float y, float z) {

                Use();
                if (uniformReference.TryGetValue(Name, out int value)) {
                    if (value == -1)
                        return;
                    GL.Uniform3(value, x, y, z);
                    return;
                }

                value = GL.GetUniformLocation(Handle, Name);

                if (value == -1) {
                    Console.WriteLine("Error: not found " + Name + " in shader");
                }
                uniformReference.Add(Name, value);
                GL.Uniform3(value, x, y, z);

            }

            //te quiero mucho guapoooo

        }
    }

}
