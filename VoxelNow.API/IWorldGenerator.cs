
namespace VoxelNow.API
{
    public interface IWorldGenerator
    {

        public void GenerateTerrain(IChunkDatabase chunkDatabase, uint ID);

    }
}
