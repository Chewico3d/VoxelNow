
namespace VoxelNow.API
{
    public interface IWorldGenerator
    {

        public MapArray3D<ushort> GenerateTerrain(int sizeX, int sizeY, int sizeZ, uint ID);

    }
}
