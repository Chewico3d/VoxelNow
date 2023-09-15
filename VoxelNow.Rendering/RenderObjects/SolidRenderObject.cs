using OpenTK.Graphics.OpenGL4;
using VoxelNow.Rendering.MeshData;

namespace VoxelNow.Rendering.RenderObjects {
    public class SolidRenderObject : IRenderObject {

        public ushort renderObjectID { get { return 0x01; } }

        int VAO;
        int v_Buffer;
        int v_UVs;
        int v_PlaneUV;
        int v_VertexShadow;
        int v_Normal;
        int v_MaterialProps;
        int v_SunIntensity;
        int indexBuffer;

        int numberOfTriangles;

        bool built = false;

        public bool Draw() {
            if (numberOfTriangles == 0)
                return false;

            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, numberOfTriangles, DrawElementsType.UnsignedInt, 0);

            return true;
        }

        public void LoadData(IMeshData meshData) {
            if (!built) {
                built = true;

                VAO = GL.GenVertexArray();
                v_Buffer = GL.GenBuffer();
                v_UVs = GL.GenBuffer();
                v_VertexShadow = GL.GenBuffer();
                v_Normal = GL.GenBuffer();
                v_PlaneUV = GL.GenBuffer();
                v_MaterialProps = GL.GenBuffer();
                v_SunIntensity = GL.GenBuffer();
                indexBuffer = GL.GenBuffer();
            }
            SolidkMeshData solidChunkMeshData = (SolidkMeshData)meshData;

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_Buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.v_Positions.Length, solidChunkMeshData.v_Positions, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.UnsignedByte, false, 3, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_UVs);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.v_UV.Length, solidChunkMeshData.v_UV, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Byte, false, 2, 0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_VertexShadow);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.v_VertexShadow.Length * sizeof(ushort), solidChunkMeshData.v_VertexShadow, BufferUsageHint.DynamicDraw);

            GL.VertexAttribIPointer(2, 1, VertexAttribIntegerType.UnsignedShort, 2, IntPtr.Zero);
            GL.EnableVertexAttribArray(2);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_Normal);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.v_Normal.Length, solidChunkMeshData.v_Normal, BufferUsageHint.DynamicDraw);

            GL.VertexAttribIPointer(3, 1, VertexAttribIntegerType.UnsignedByte, 1, IntPtr.Zero  );
            GL.EnableVertexAttribArray(3);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_PlaneUV);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.v_PlaneUV.Length, solidChunkMeshData.v_PlaneUV, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(4, 2, VertexAttribPointerType.UnsignedByte, false, 2, 0);
            GL.EnableVertexAttribArray(4);

            GL.BindBuffer(BufferTarget.ArrayBuffer, v_MaterialProps);
            GL.BufferData(BufferTarget.ArrayBuffer, solidChunkMeshData.v_MaterialInfo.Length, solidChunkMeshData.v_MaterialInfo, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(5, 1, VertexAttribPointerType.UnsignedByte, false, 1, 0);
            GL.EnableVertexAttribArray(5);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, solidChunkMeshData.indices.Length * sizeof(uint), solidChunkMeshData.indices, BufferUsageHint.DynamicDraw);

            numberOfTriangles = solidChunkMeshData.indices.Length;
        }
    }
}
