using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Rendering.MeshData;
using VoxelNow.Server;

namespace VoxelNow.Rendering.RenderObjects {
    internal class MapRenderObject : IRenderObject {
        public ushort renderObjectID { get { return 0x02; } }

        bool built = false;

        int VAO;
        int v_PositionBuffer;
        int v_ColorBuffer;
        int indicesBuffer;

        int numberOfTriangles;

        public bool Draw() {
            if (!built)
                return false;

            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, numberOfTriangles, DrawElementsType.UnsignedInt, 0);

            return true;

        }

        public void LoadData(IMeshData meshData) {
            if (!built) {
                built = true;
                v_PositionBuffer = GL.GenBuffer();
                v_ColorBuffer = GL.GenBuffer();
                indicesBuffer = GL.GenBuffer();
                VAO = GL.GenVertexArray();

            }

            MapMeshData mapMeshData = (MapMeshData)meshData;

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_PositionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, mapMeshData.vertexData.Length * sizeof(float), mapMeshData.vertexData, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_ColorBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, mapMeshData.colorData.Length, mapMeshData.colorData, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.UnsignedByte, false, 3 , 0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, mapMeshData.indices.Length * sizeof(uint), mapMeshData.indices, BufferUsageHint.DynamicDraw);

            numberOfTriangles = mapMeshData.indices.Length;

        }
    }
}
