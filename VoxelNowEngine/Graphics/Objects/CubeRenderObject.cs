using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNowEngine.Graphics.Objects {
    public class CubeRenderObject : RenderObject {

        internal int VAO;
        internal int indexBuffer;
        internal int vPositionBuffer;
        int numberOfTriangles;

        static readonly byte[] CubeVerticesPositions = {
            0,0,1, 0,0,0, 0,1,1, 0,1,0,
            1,0,0,1,0,1,1,1,0,1,1,1,
            0,0,1,1,0,1,0,0,0,1,0,0,
            0,1,0,1,1,0,0,1,1,1,1,1,
            0,0,0,1,0,0,0,1,0,1,1,0,
            1,0,1,0,0,1,1,1,1,0,1,1
        };
        static readonly byte[] IndexPatern = { 0, 2, 1, 2, 3, 1 };

        public CubeRenderObject() {
            vPositionBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vPositionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, CubeVerticesPositions.Length, CubeVerticesPositions, BufferUsageHint.StaticDraw);

            byte[] indices = new byte[6 * 6];
            for (int x = 0; x < 6; x++)
                for (int y = 0; y < 6; y++)
                    indices[x * 6 + y] = (byte)(IndexPatern[y] + x * 4);

            indexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length, indices, BufferUsageHint.StaticDraw);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vPositionBuffer);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.UnsignedByte, false, 3, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);

            numberOfTriangles = indices.Length;

        }

        public void Draw() {
            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, numberOfTriangles, DrawElementsType.UnsignedByte, 0);
        }
    }
}
