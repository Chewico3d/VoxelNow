using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.API;
using VoxelNow.API.Noise;

namespace VoxelNow.Assets.WorldGeneration
{
    public class WorldGeneration : IWorldGenerator {
        FastNoise fn;

        PerlinNoiseMap2D noiseMap;
        public MapArray2D<int> GenerateHeightMap(int voxelsX, int voxelsZ) {

            MapArray2D<int> heightMap = new MapArray2D<int>(voxelsX, voxelsZ);
            noiseMap = new PerlinNoiseMap2D(voxelsX, voxelsZ, 200);
            for(int x = 0; x < voxelsX; x++) {
                for(int z = 0; z < voxelsZ; z++) {
                    float midX = x -voxelsX / 2;
                    float midY = z - voxelsZ / 2;

                    float DistanceToCenter = 1 - MathF.Pow(midX * midX * midX * midX + midY * midY * midY * midY, 0.25f) / (32 * 10);
                    DistanceToCenter = Math.Clamp(DistanceToCenter, 0, 1);

                    int initialHeight = (int)((fn.GetNoise((float)x / 2, (float)z / 2) * 100 + 100) * DistanceToCenter);

                    initialHeight += (int)((fn.GetNoise(x + 100, z - 322) * 30) * DistanceToCenter);
                    //heightMap.SetValue(x, z, initialHeight);
                    heightMap.SetValue(x, z, (int)(noiseMap.GetValue(x, z) * 300 + 100));
                }

            }

            return heightMap;

        }


        public void SetSeed(int seedNumber) {
            fn = new FastNoise(seedNumber);

        }
    }
}
