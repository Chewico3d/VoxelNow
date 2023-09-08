using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Rendering.MeshData {
    internal class MapMeshData : IMeshData {
        public ushort renderObjectID { get { return 0x02; } }

        public float[] vertexData;
        public byte[] colorData;
        public uint[] indices;

    }
}
