using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Rendering.MeshData {
    internal class SolidChunkMeshData : IMeshData {
        public ushort renderObjectID { get { return 0x01; } }

        internal byte[] v_Positions;
        internal byte[] UVs;
        internal byte[] v_AmbientOclusion;
        internal ushort[] indices;

    }
}
