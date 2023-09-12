using VoxelNow.API;

namespace VoxelNow.Assets.ProceduralVoxel {
    public class SimpleTree : IProceduralVoxel {
        public ushort ID { get { return 0x00; } }

        public void GenerateAt(int x, int y, int z, IChunkDatabase chunkDatabase) {
            throw new NotImplementedException();
        }
    }
}
