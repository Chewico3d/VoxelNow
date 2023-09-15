
using VoxelNow.API;

namespace VoxelNow.Rendering.FabricData {
    public class FluidFabricData : IFabricData {
        int IFabricData.renderObjectID { get { return 0x03; } }

        IChunkDatabase database;
        public int xID, yID, zID;

        public FluidFabricData(IChunkDatabase database, int xID, int yID, int zID) {
            this.database = database;
            this.xID = xID;
            this.yID = yID;
            this.zID = zID;
        }

        public byte GetWaterValue(int x, int y, int z) {
            return database.GetWaterValue(x + xID * GenerationConstants.voxelSizeX,
                y + yID * GenerationConstants.voxelSizeY, z + zID * GenerationConstants.voxelSizeZ);
        }

    }
}
