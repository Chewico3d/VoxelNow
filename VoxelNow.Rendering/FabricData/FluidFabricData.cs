
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using VoxelNow.API;
using VoxelNow.AssemblyLoader;

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

        public bool CanWaterPass(int x, int y, int z) {
            ushort voxelID = database.GetVoxel(x + xID * GenerationConstants.voxelSizeX,
                y + yID * GenerationConstants.voxelSizeY, z + zID * GenerationConstants.voxelSizeZ);

            return VoxelAssets.CanWaterPass(voxelID);

        }

        public float GetRelativeVertexWaterHeight(int x, int y, int z) {
            x += xID * GenerationConstants.voxelSizeX;
            y += yID * GenerationConstants.voxelSizeY;
            z += zID * GenerationConstants.voxelSizeZ;

            if (database.GetWaterValue(x, y + 1, z) != 0 | database.GetWaterValue(x - 1, y + 1, z) != 0
                | database.GetWaterValue(x , y + 1, z -1) != 0 | database.GetWaterValue(x - 1, y + 1, z - 1) != 0)
                return 1;

            float totalValue = database.GetWaterValue(x, y, z);
            totalValue += database.GetWaterValue(x - 1, y, z);
            totalValue += database.GetWaterValue(x, y, z - 1);
            totalValue += database.GetWaterValue(x - 1, y, z - 1);

            int numberOfWaterVoxels = 1;
            numberOfWaterVoxels += VoxelAssets.CanWaterPass(database.GetVoxel(x - 1, y, z))? 1 : 0;
            numberOfWaterVoxels += VoxelAssets.CanWaterPass(database.GetVoxel(x, y, z - 1)) ? 1 : 0;
            numberOfWaterVoxels += VoxelAssets.CanWaterPass(database.GetVoxel(x - 1, y, z - 1)) ? 1 : 0;

            return totalValue / numberOfWaterVoxels / 130;

        }

    }
}
