
using OpenTK.Mathematics;

namespace VoxelNow.Rendering.RenderCollection {
    internal class FluidRenderCollection : IRenderCollection {
        public ushort renderObjectID { get { return 0x03; } }

        public List<IRenderObject> renderObjects = new List<IRenderObject>();
        public List<(float, float, float)> renderObjectsPositions = new List<(float, float, float)>();

        public int AddObject(IRenderObject renderObject) {
            renderObjects.Add(renderObject);
            renderObjectsPositions.Add((0, 0, 0));

            return renderObjects.Count - 1;
        }

        public void Draw(Camera camera, Shader shader) {
            shader.Use();
            camera.CalculateCameraMatrix();

            for (int x = 0; x < renderObjects.Count; x++) {
                (float, float, float) position = renderObjectsPositions[x];

                float distanceToCameraX = position.Item1 - camera.xPos;
                float distanceToCameraY = position.Item2 - camera.yPos;
                float distanceToCameraZ = position.Item3 - camera.zPos;

                float distanceToCamera = distanceToCameraX * distanceToCameraX + distanceToCameraY * distanceToCameraY + distanceToCameraZ * distanceToCameraZ;

                if (distanceToCamera > 600 * 600)
                    continue;

                Matrix4 matrix = camera.GetMatrixForObject(position.Item1, position.Item2, position.Item3);
                shader.SetTransformationMatrix(matrix);

                IRenderObject currentRendrerObject = renderObjects[x];
                currentRendrerObject.Draw();

            }

        }

        public void SetRenderObjectPosition(int objectID, float posX, float posY, float posZ) {
            renderObjectsPositions[objectID] = (posX, posY, posZ);
        }

        public IRenderObject GetObject(int objectID) {
            return renderObjects[objectID];
        }
    }
}