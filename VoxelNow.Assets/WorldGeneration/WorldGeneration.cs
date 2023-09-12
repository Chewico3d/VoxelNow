using VoxelNow.API;
using VoxelNow.API.Tools;

namespace VoxelNow.Assets.WorldGeneration
{
    public class WorldGeneration : IWorldGenerator
    {

        CurveModifier curveModifier;
        PerlinNoiseMap2D noiseMap;
        float SmothValue(float value) {

            return -(MathF.Cos(MathF.PI * value) - 1) / 2;
        }

        public void GenerateTerrain(IChunkDatabase chunkDatabase, uint ID) {
            curveModifier = new CurveModifier();

            curveModifier.AddModifier(new Modifier(0, 60));
            curveModifier.AddModifier(new Modifier(0.1f, 120));
            curveModifier.AddModifier(new Modifier(0.2f, 135));
            curveModifier.AddModifier(new Modifier(1, 300));

            noiseMap = new PerlinNoiseMap2D(chunkDatabase.voxelSizeX, chunkDatabase.voxelSizeZ, 150);
            for (int x = 0; x < chunkDatabase.voxelSizeX; x++) {
                for (int z = 0; z < chunkDatabase.voxelSizeZ; z++) {
                    float midX = x - chunkDatabase.voxelSizeX / 2;
                    float midZ = z - chunkDatabase.voxelSizeZ / 2;

                    float distanceToEndX = (MathF.Abs(midX)) / chunkDatabase.voxelSizeX * 2;
                    distanceToEndX = (distanceToEndX < 0) ? 0 : (distanceToEndX > 1) ? 1 : distanceToEndX;

                    float distanceToEndZ = (MathF.Abs(midZ)) / chunkDatabase.voxelSizeZ * 2;
                    distanceToEndZ = (distanceToEndZ < 0) ? 0 : (distanceToEndZ > 1) ? 1 : distanceToEndZ;

                    float DistanceToCenter = MathF.Sqrt(distanceToEndX * distanceToEndX + distanceToEndZ * distanceToEndZ);
                    DistanceToCenter = (DistanceToCenter > 1) ? 1 : DistanceToCenter;
                    DistanceToCenter = 1 - DistanceToCenter;
                    
                    DistanceToCenter = SmothValue(DistanceToCenter);

                    float height = (noiseMap.GetValue(x, z) * 0.5f + 0.5f) * DistanceToCenter ;
                    //height *= DistanceToCenter;

                    height = curveModifier.GetSmothPoint(height);


                    int initialHeight = (int)(height);
                    initialHeight = (initialHeight < 0) ? 1 : (initialHeight >= chunkDatabase.voxelSizeY) ? chunkDatabase.voxelSizeY - 1 : initialHeight;
                    //initialHeight = (int)(DistanceToCenter * 100);

                    for (int y = 0; y < initialHeight; y++) {
                        if (y < 125)
                            chunkDatabase.SetVoxel(x, y, z, 2);
                        else {
                            if(y + 1 >= initialHeight) {
                                if(x == 300 && z == 300)
                                    chunkDatabase.SetVoxel(x, y, z, 0x200);
                                else
                                    chunkDatabase.SetVoxel(x, y, z, 4);
                            }
                            else
                                chunkDatabase.SetVoxel(x, y, z, 3);
                        }

                    }
                }

            }


        }

    }
}
