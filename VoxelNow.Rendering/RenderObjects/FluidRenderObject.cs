using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Rendering.MeshData;

namespace VoxelNow.Rendering.RenderObjects {
    internal class FluidRenderObject : IRenderObject {
        public ushort renderObjectID { get { return 0x03; } }

        int VAO;
        int vPositionBuffer;
        int indexBuffer;
        int TriangleCount;

        public bool built = false;

        public bool Draw() {
            if (!built)
                return false;

            if (TriangleCount == 0)
                return false;

            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, TriangleCount, DrawElementsType.UnsignedInt, 0);
            
            return true;
        }

        public void LoadData(IMeshData meshData) {
            if (!built) {
                VAO = GL.GenVertexArray();
                vPositionBuffer = GL.GenBuffer();
                indexBuffer = GL.GenBuffer();

                built = true;
            }

            FluidMeshData fluidMeshData = (FluidMeshData)meshData;

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vPositionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, fluidMeshData.vPosition.Length, fluidMeshData.vPosition, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.UnsignedByte, false, 3, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, fluidMeshData.indices.Length * sizeof(uint), fluidMeshData.indices, BufferUsageHint.DynamicDraw);

            TriangleCount = fluidMeshData.indices.Length;

        }
    }
}
