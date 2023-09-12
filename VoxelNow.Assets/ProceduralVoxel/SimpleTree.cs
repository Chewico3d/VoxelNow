using VoxelNow.API;

namespace VoxelNow.Assets.ProceduralVoxel {
    public class SimpleTree : IProceduralVoxel {
        public ushort ID { get { return 0x00; } }

        public void GenerateAt(int x, int y, int z, IChunkDatabase chunkDatabase) {
            chunkDatabase.SetVoxel(x, y, z, 1);
            chunkDatabase.SetVoxel(x, y + 1, z, 1);
            chunkDatabase.SetVoxel(x, y + 2, z, 1);
        }
    }
}
