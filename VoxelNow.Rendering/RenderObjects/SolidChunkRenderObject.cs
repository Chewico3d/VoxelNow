using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Rendering.MeshData;
using VoxelNow.Server;

namespace VoxelNow.Rendering.RenderObjects {
    public class SolidChunkRenderObject : IRenderObject {

        public ushort renderObjectID { get { return 0x01; } }

        int VAO;
        int v_Buffer;
        int v_UVs;
        int v_PlaneUV;
        int v_AmbientOcclusion;
        int v_Normal;
        int indexBuffer;

        int numberOfTriangles;

        bool built = false;

        public void Draw() {
            if (numberOfTriangles == 0)
                return;

            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, numberOfTriangles, DrawElementsType.UnsignedShort, 0);
        }

        public void LoadData(IMeshData meshData) {
            if (!built) {
                built = true;

                VAO = GL.GenVertexArray();
                v_Buffer = GL.GenBuffer();
                v_UVs = GL.GenBuffer();
                v_AmbientOcclusion = GL.GenBuffer();
                v_Normal = GL.GenBuffer();
                v_PlaneUV = GL.GenBuffer();
                indexBuffer = GL.GenBuffer();
            }
            SolidChunkMeshData solidChunkMeshData = (SolidChunkMeshData)meshData;

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_Buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.v_Positions.Length, solidChunkMeshData.v_Positions, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.UnsignedByte, false, 3, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_UVs);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.v_UV.Length, solidChunkMeshData.v_UV, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Byte, false, 2, 0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_AmbientOcclusion);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.v_AmbientOclusion.Length, solidChunkMeshData.v_AmbientOclusion, BufferUsageHint.DynamicDraw);

            GL.VertexAttribIPointer(2, 1, VertexAttribIntegerType.UnsignedByte, 1, IntPtr.Zero);
            GL.EnableVertexAttribArray(2);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_Normal);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.v_Normal.Length, solidChunkMeshData.v_Normal, BufferUsageHint.DynamicDraw);

            GL.VertexAttribIPointer(3, 1, VertexAttribIntegerType.UnsignedByte, 1, IntPtr.Zero  );
            GL.EnableVertexAttribArray(3);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_PlaneUV);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.v_PlaneUV.Length, solidChunkMeshData.v_PlaneUV, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(4, 2, VertexAttribPointerType.UnsignedByte, false, 2, 0);
            GL.EnableVertexAttribArray(4);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, solidChunkMeshData.indices.Length * sizeof(ushort), solidChunkMeshData.indices, BufferUsageHint.DynamicDraw);

            numberOfTriangles = solidChunkMeshData.indices.Length;
        }
    }
}
