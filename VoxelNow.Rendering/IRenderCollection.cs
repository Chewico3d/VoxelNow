using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Rendering.VallVoxel.Client.Graphics;
using VoxelNow.Server;

namespace VoxelNow.Rendering {
    internal interface IRenderCollection {
        public ushort renderObjectID { get; }
        internal void Draw(Camera camera, Shader shader);
        internal int AddObject(IRenderObject renderObject);
        internal IRenderObject GetObject(int objectID);
        internal void SetRenderObjectPosition(int objectID, float posX, float posY, float posZ);
    }
}
