
namespace VoxelNow.API
{
    public interface IProceduralVoxel
    {
        public ushort ID { get; }

        public void GenerateAt(IChunkDatabase chunkDatabase, int x, int y, int z); 

    }
}
