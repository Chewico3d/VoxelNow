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
        int v_AmbientOcclusion;
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
                indexBuffer = GL.GenBuffer();
            }
            SolidChunkMeshData solidChunkMeshData = (SolidChunkMeshData)meshData;

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_Buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.v_Positions.Length, solidChunkMeshData.v_Positions, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.UnsignedByte, false, 3, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_UVs);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.UVs.Length, solidChunkMeshData.UVs, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Byte, false, 2, 0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_AmbientOcclusion);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.v_AmbientOclusion.Length, solidChunkMeshData.v_AmbientOclusion, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(2, 1, VertexAttribPointerType.Byte, false, 1, 0);
            GL.EnableVertexAttribArray(2);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, solidChunkMeshData.indices.Length * sizeof(ushort), solidChunkMeshData.indices, BufferUsageHint.DynamicDraw);

            numberOfTriangles = solidChunkMeshData.indices.Length;
        }
    }
}
