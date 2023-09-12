using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Core;

namespace VoxelNow.Rendering.FabricData {
    public class SolidCunkFabricData : IFabricData {

        int IFabricData.renderObjectID { get { return 0x01; } }

        public Func<int, int, int, ushort> voxelData;

        public ushort GetVoxel(int x, int y, int z) {
            ushort voxelID = voxelData(x, y, z);
            return voxelID;

        }

    }
}
