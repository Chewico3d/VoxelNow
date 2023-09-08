using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Rendering.VallVoxel.Client.Graphics;
using VoxelNow.Server;

namespace VoxelNow.Rendering.RenderCollection {
    internal class MapRenderCollection : IRenderCollection {
        public ushort renderObjectID { get { return 0x02; } }

        public List<IRenderObject> renderObjects = new List<IRenderObject>();

        public int AddObject(IRenderObject renderObject) {
            renderObjects.Add(renderObject);

            return renderObjects.Count - 1;
        }

        public void Draw(Camera camera, Shader shader) {
            shader.Use();
            camera.CalculateCameraMatrix();
            for (int x = 0; x < renderObjects.Count; x++) {

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
