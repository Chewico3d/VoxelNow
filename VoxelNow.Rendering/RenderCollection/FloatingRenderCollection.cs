using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Rendering.VallVoxel.Client.Graphics;
using VoxelNow.Server;

namespace VoxelNow.Rendering.RenderCollection {
    internal class FloatingRenderCollection : IRenderCollection {
        public ushort renderObjectID { get { return 0x00; } }

        public List<IRenderObject> renderObjects = new List<IRenderObject>();
        public List<(float, float, float)> renderObjectsPositions = new List<(float, float, float)> ();

        public int AddObject(IRenderObject renderObject) {
            renderObjects.Add(renderObject);
            renderObjectsPositions.Add((0, 0, 0));

            return renderObjects.Count - 1;
        }

        public void Draw(Camera camera, Shader shader) {
            shader.Use();
            camera.CalculateCameraMatrix();
            for(int x = 0; x < renderObjects.Count; x++) {
                (float, float, float) position = renderObjectsPositions[x];
                Matrix4 matrix = camera.GetMatrixForObject(position.Item1, position.Item2, position.Item3);
                shader.SetTransformationMatrix(matrix);

                IRenderObject currentRendrerObject = renderObjects[x];
                currentRendrerObject.Draw();

            }

        }

        public void SetRenderObjectPosition(int objectID, float posX, float posY, float posZ) {
            throw new NotImplementedException();
        }

        public IRenderObject GetObject(int objectID) {
            return renderObjects[objectID];
        }
    }
}
