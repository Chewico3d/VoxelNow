using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Core;

namespace VoxelNow.Rendering.FabricData {
    public class SolidCunkFabricData : IFabricData {

        int IFabricData.renderObjectID { get { return 0x01; } }

        ChunkDatabase database;
        public int xID, yID, zID;

        public SolidCunkFabricData(ChunkDatabase database, int xID, int yID, int zID) 
{
            this.database = database;
            this.xID = xID;
            this.yID = yID;
            this.zID = zID;
        }

        public ushort GetVoxel(int x, int y, int z) {
            return database.GetVoxel(x + xID * GenerationConstants.voxelSizeX,
                y + yID * GenerationConstants.voxelSizeY, z + zID * GenerationConstants.voxelSizeZ);

        }

    }
}
