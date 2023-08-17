using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using VoxelNowEngine.Graphics.Materials;
using VoxelNowEngine.Graphics.Objects;

namespace VoxelNowEngine.Graphics {
    public static class DebugRender {

        static LitMaterial litMat;
        static CubeRenderObject cubeRenderObject;
        static DebugRender() {
            litMat = new LitMaterial();
            cubeRenderObject = new CubeRenderObject();
        }

        public static void RenderCube(Camera camera,Vector3 Position, Vector3 Scale) {
            litMat.Use();
            litMat.SetTransformationMatrix(camera, Position, Scale);

            cubeRenderObject.Draw();

        }
        public static void RenderCube(Camera camera, Vector3 Position) {
            litMat.Use();
            litMat.SetTransformationMatrix(camera, Position);

            cubeRenderObject.Draw();

        }
    }
}
