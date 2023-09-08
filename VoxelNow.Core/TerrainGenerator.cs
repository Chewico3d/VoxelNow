using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.API;

namespace VoxelNow.Core {
    internal static class TerrainGenerator {

        static ChunkDatabase workingDatabase;

        public static void GenerateTerrain(ChunkDatabase chunkDatabase) {
            workingDatabase = chunkDatabase;
            AssetLoader.worldGenerator.SetSeed(0);

            Console.WriteLine("Starting generating");
            MapArray2D<int> heightMap = AssetLoader.worldGenerator.GenerateHeightMap(chunkDatabase.sizeX * GenerationConstants.voxelSizeX,
                chunkDatabase.sizeZ * GenerationConstants.voxelSizeZ);

            Console.WriteLine("Starting putting");
            for (int x = 0; x < chunkDatabase.sizeX * GenerationConstants.voxelSizeX; x++) {
                for(int z = 0; z < chunkDatabase.sizeZ * GenerationConstants.voxelSizeZ; z++) {

                    int height = heightMap.GetValue(x, z);
                    height = (height <= 0)? 1 : (height >= chunkDatabase.sizeY * GenerationConstants.voxelSizeY) ?
                        chunkDatabase.sizeY * GenerationConstants.voxelSizeY - 1 : height;

                    for(int y = 0; y < height; y++) {
                        chunkDatabase.SetVoxel(x, y, z, 1);
                    }

                }
            }
            Console.WriteLine("end putting");

            workingDatabase.terrainHeight = heightMap;
        }

        public static void GenerateHeight(ChunkDatabase chunkDatabase) {
            workingDatabase = chunkDatabase;
            AssetLoader.worldGenerator.SetSeed(0);

            MapArray2D<int> heightMap = AssetLoader.worldGenerator.GenerateHeightMap(chunkDatabase.sizeX * GenerationConstants.voxelSizeX,
                chunkDatabase.sizeZ * GenerationConstants.voxelSizeZ);

            workingDatabase.terrainHeight = heightMap;

            
        }


    }
}
