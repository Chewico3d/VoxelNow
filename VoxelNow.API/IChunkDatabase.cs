
namespace VoxelNow.API {
    public interface IChunkDatabase {

        public int voxelSizeX { get; }
        public int voxelSizeY { get; }
        public int voxelSizeZ { get; }

        public ushort GetVoxel(int x, int y, int z);
        public void SetVoxel(int x, int y, int z, ushort value);

        public byte GetSunLight(int x, int y, int z);
        public void SetSunLight(int x, int y, int z, byte value);

        public byte GetWaterValue(int x, int y, int z);
        public void SetWaterValue(int x, int y, int z, byte value);

    }
}
