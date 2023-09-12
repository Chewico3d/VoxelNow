using VoxelNow.API;

namespace VoxelNow.Rendering.FabricData {
    public class SolidCunkFabricData : IFabricData {

        int IFabricData.renderObjectID { get { return 0x01; } }

        IChunkDatabase database;
        public int xID, yID, zID;

        public SolidCunkFabricData(IChunkDatabase database, int xID, int yID, int zID) 
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
