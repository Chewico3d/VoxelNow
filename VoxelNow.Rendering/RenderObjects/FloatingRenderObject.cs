using OpenTK.Graphics.OpenGL4;
using VoxelNow.Rendering.MeshData;
namespace VoxelNow.Rendering.RenderObjects {
    public class FloatingRenderObject : IRenderObject {

        public ushort renderObjectID { get { return 0x00; } }
        bool build = false;

        int vPositionBuffer;
        int indices;
        int VAO;

        int triangleCount;

        public bool Draw() {
            if (!build)
                return false;
            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, triangleCount, DrawElementsType.UnsignedInt, 0);
            return true;
        }

        public void LoadData(IMeshData meshData) {

            if (!build) {
                vPositionBuffer = GL.GenBuffer();
                indices = GL.GenBuffer();
                VAO = GL.GenVertexArray();
                build = true;
            }
            

            GL.BindVertexArray(VAO);
            FloatingMeshData floatingRenderObject = (FloatingMeshData)meshData;

            GL.BindBuffer(BufferTarget.ArrayBuffer, vPositionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, floatingRenderObject.v_Position.Count * sizeof(float), floatingRenderObject.v_Position.ToArray(), BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indices);
            GL.BufferData(BufferTarget.ElementArrayBuffer, floatingRenderObject.indicies.Count * sizeof(int), floatingRenderObject.indicies.ToArray(), BufferUsageHint.StaticDraw);


            GL.BindBuffer(BufferTarget.ArrayBuffer, vPositionBuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indices);
            triangleCount = floatingRenderObject.indicies.Count;

        }
    }
}
