using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace VoxelNowEngine.Graphics.Objects
{
    public class SolidChunkRenderObject : RenderObject {
        int VAO;
        int vPositionBuffer;
        int vNormalBuffer;
        int vTextureBuffer;
        int vAmbientOclusion;
        int IndexBuffer;

        int NumberOfIndices;

        internal SolidChunkRenderObject(Data data) {
            vPositionBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vPositionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, data.v_Positions.Length, data.v_Positions, BufferUsageHint.StaticDraw);

            vNormalBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vNormalBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, data.v_Normals.Length, data.v_Normals, BufferUsageHint.StaticDraw);

            vTextureBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vTextureBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, data.v_Textures.Length, data.v_Textures, BufferUsageHint.StaticDraw);

            vAmbientOclusion = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vAmbientOclusion);
            GL.BufferData(BufferTarget.ArrayBuffer, data.v_AmbientOclusion.Length, data.v_AmbientOclusion, BufferUsageHint.StaticDraw);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vPositionBuffer);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.UnsignedByte, false, 3, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vNormalBuffer);

            GL.VertexAttribIPointer(1, 1, VertexAttribIntegerType.UnsignedByte, 1, IntPtr.Zero);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vTextureBuffer);

            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.UnsignedByte, false, 2, 0);
            GL.EnableVertexAttribArray(2);


            GL.BindBuffer(BufferTarget.ArrayBuffer, vAmbientOclusion);

            GL.VertexAttribPointer(3, 1, VertexAttribPointerType.UnsignedByte, false, 1, 0);
            GL.EnableVertexAttribArray(3);

            IndexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, data.Indices.Length * sizeof(ushort), data.Indices, BufferUsageHint.StaticDraw);
            NumberOfIndices = data.Indices.Length;

        }

        internal void UpdateData(Data data) {

            GL.BindBuffer(BufferTarget.ArrayBuffer, vPositionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, data.v_Positions.Length, data.v_Positions, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vNormalBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, data.v_Normals.Length, data.v_Normals, BufferUsageHint.StaticDraw);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, vTextureBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, data.v_Textures.Length, data.v_Textures, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vAmbientOclusion);
            GL.BufferData(BufferTarget.ArrayBuffer, data.v_AmbientOclusion.Length, data.v_AmbientOclusion, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, data.Indices.Length * sizeof(ushort), data.Indices, BufferUsageHint.StaticDraw);
            NumberOfIndices = data.Indices.Length;

        }

        internal class Data {
            internal byte[] v_Positions;
            internal byte[] v_Normals;
            internal byte[] v_Textures;
            internal byte[] v_AmbientOclusion;
            internal ushort[] Indices;
            internal Data(byte[] v_Positions, byte[] v_Normals, byte[] v_Textures, byte[] v_AmbientOclusion, ushort[] Indices){
                this.v_Positions = v_Positions;
                this.v_Normals = v_Normals;
                this.v_Textures = v_Textures;
                this.v_AmbientOclusion = v_AmbientOclusion;
                this.Indices = Indices;

            }        
        }

        void RenderObject.Draw()
        {

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);
            GL.DrawElements(PrimitiveType.Triangles, NumberOfIndices, DrawElementsType.UnsignedShort, 0);
        }
    }
}
