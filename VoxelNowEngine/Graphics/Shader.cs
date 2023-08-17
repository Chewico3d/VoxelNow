using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace VoxelNowEngine.Graphics {
    public class Shader {

        int Handle;
        Dictionary<string, int> uniformReference = new Dictionary<string, int>();
        internal Shader(string vertexPath, string fragmentPath) {
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
            }

            GL.CompileShader(FragmentShader);

            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0) {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
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

        internal void SetMatrix4(string Name, Matrix4 matrix4) {

            Use();
            if (uniformReference.TryGetValue(Name, out int value)) {

                GL.UniformMatrix4(value, true, ref matrix4);
                return;
            }

            value = GL.GetUniformLocation(Handle, Name);

            if (value == -1)
                Console.WriteLine("Error: not found " + Name + " in shader");
            uniformReference.Add(Name, value);
            GL.UniformMatrix4(value, true, ref matrix4);

        }
        internal void SetUniform1(string Name, int uniform) {

            Use();
            if (uniformReference.TryGetValue(Name, out int value)) {

                GL.Uniform1(value, uniform);
                return;
            }

            value = GL.GetUniformLocation(Handle, Name);

            if (value == -1)
                Console.WriteLine("Error: not found " + Name + " in shader");
            uniformReference.Add(Name, value);
            GL.Uniform1(value, uniform);

        }

        public virtual void SetTransformationMatrix(Camera camera, Vector3 Position) {
            Matrix4 InvertZ = Matrix4.CreateScale(1, 1, -1);
            Matrix4 localMatrix = Matrix4.CreateTranslation(Position * new Vector3(1,1,-1));
            Matrix4 generalMatrix = camera.GetCameraMatrix();

            Matrix4 resultMatrix = InvertZ * localMatrix * generalMatrix;

            SetMatrix4("transform", resultMatrix);

        }
        public virtual void SetTransformationMatrix(Camera camera, Vector3 Position, Vector3 Scale) {
            Matrix4 InvertZ = Matrix4.CreateScale(1, 1, -1);
            Matrix4 localMatrix = Matrix4.CreateTranslation(Position * new Vector3(1, 1, -1));
            localMatrix = Matrix4.CreateScale(Scale) * localMatrix;
            Matrix4 generalMatrix = camera.GetCameraMatrix();

            Matrix4 resultMatrix = InvertZ * localMatrix * generalMatrix;

            SetMatrix4("transform", resultMatrix);

        }

    }
    //te quiero mucho guapoooo
}
