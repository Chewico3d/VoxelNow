
namespace VoxelNow.API {
    public interface IChunkDatabase {

        public int voxelSizeX { get; }
        public int voxelSizeY { get; }
        public int voxelSizeZ { get; }

        public ushort GetVoxel(int x, int y, int z);
        public void SetVoxel(int x, int y, int z, ushort value);

    }
}
