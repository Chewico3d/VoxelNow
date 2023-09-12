using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.API;

namespace VoxelNow.Core {
    public class ChunkDatabase {

        public readonly int sizeX, sizeY, sizeZ;
        public readonly Chunk[] chunks;

        public ChunkDatabase(int sizeX, int sizeY, int sizeZ) {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;

            chunks = new Chunk[sizeX * sizeY * sizeZ];
        }

        public void GenerateTerrain() {

            MapArray3D<ushort> completeTerrain = AssetLoader.worldGenerator.GenerateTerrain(
                sizeX * GenerationConstants.voxelSizeX,
                sizeY * GenerationConstants.voxelSizeY,
                sizeZ * GenerationConstants.voxelSizeZ, 0);

            void ChunkLoadData(Chunk workingChunk) {

                int voxelID = 0;

                int offsetX = workingChunk.IDx * GenerationConstants.voxelSizeX;
                int offsetY = workingChunk.IDy * GenerationConstants.voxelSizeY;
                int offsetZ = workingChunk.IDz * GenerationConstants.voxelSizeZ;

                for(int z = 0; z < GenerationConstants.voxelSizeZ; z++) {
                    for(int y = 0; y < GenerationConstants.voxelSizeY; y++)
                        for(int x = 0; x < GenerationConstants.voxelSizeX; x++) {

                            workingChunk.voxels[voxelID] = completeTerrain
                                .GetValue(x + offsetX, y + offsetY, z + offsetZ);

                            voxelID++;
                        }
                }
            }

            int chunkID = 0;
            for (int z = 0; z < sizeZ; z++) {
                for (int y = 0; y < sizeY; y++)
                    for (int x = 0; x < sizeX; x++) {

                        chunks[chunkID] = new Chunk(x, y, z);
                        ChunkLoadData(chunks[chunkID]);
                        chunkID++;
                    }
            }
        }

        //Complete means that the chunk have borders : 32 + 3, 32 + 2, 32 + 2
        public ushort[] GetCompleteChunkVoxelsData(int chunkID) {

            int completeVoxelsAmount = (GenerationConstants.voxelSizeX + 2) 
                * (GenerationConstants.voxelSizeY + 2) * (GenerationConstants.voxelSizeZ + 2);
            ushort[] data = new ushort[completeVoxelsAmount];

            for(int z = 0; z < GenerationConstants.voxelSizeZ + 2; z++) {
                for (int y = 0; y < GenerationConstants.voxelSizeY + 2; y++) {
                    for (int x = 0; x < GenerationConstants.voxelSizeX + 2; x++) {

                        int voxelPositionX = x - 1 + chunks[chunkID].IDx * GenerationConstants.voxelSizeX;
                        int voxelPositionY = y - 1 + chunks[chunkID].IDy * GenerationConstants.voxelSizeY;
                        int voxelPositionZ = z - 1 + chunks[chunkID].IDz * GenerationConstants.voxelSizeZ;

                        if (voxelPositionX < 0 || voxelPositionY < 0 || voxelPositionZ < 0) continue;
                        if (voxelPositionX >= sizeX * GenerationConstants.voxelSizeX) continue;
                        if (voxelPositionY >= sizeY * GenerationConstants.voxelSizeY) continue;
                        if (voxelPositionZ >= sizeZ * GenerationConstants.voxelSizeZ) continue;

                        int ID = x + y * (GenerationConstants.voxelSizeX + 2)
                            + z * (GenerationConstants.voxelSizeX + 2) * (GenerationConstants.voxelSizeY + 2);

                        data[ID] = GetVoxel(voxelPositionX, voxelPositionY, voxelPositionZ);

                    }
                }
            }

            return data;

        }

        internal void SetVoxel(int x, int y, int z, ushort value) {
            (int, int) voxelPos = GetVoxelCordinates(x, y, z);
            chunks[voxelPos.Item1].voxels[voxelPos.Item2] = value;
        }
        public ushort GetVoxel(int x, int y, int z) {


            if (x < 0 || z < 0)
                return 0;

            if (y <= 0)
                return 1;

            if (x >= sizeX * GenerationConstants.voxelSizeX|| y >= sizeY * GenerationConstants.voxelSizeY
                || z >= sizeZ * GenerationConstants.voxelSizeZ)
                return 0;

            (int, int) voxelPos = GetVoxelCordinates(x, y, z);
            return chunks[voxelPos.Item1].voxels[voxelPos.Item2];
        }
        void SetVoxelLight(int x, int y, int z, ushort value) {
            (int, int) voxelPos = GetVoxelCordinates(x, y, z);
            chunks[voxelPos.Item1].lightValue[voxelPos.Item2] = value;
        }
        ushort GetVoxelLight(int x, int y, int z) {
            (int, int) voxelPos = GetVoxelCordinates(x, y, z);
            return chunks[voxelPos.Item1].lightValue[voxelPos.Item2];
        }


        (int, int) GetVoxelCordinates(int x, int y, int z) {


            int chunkX = (int)MathF.Floor((float)x / (float)GenerationConstants.voxelSizeX);
            int chunkY = (int)MathF.Floor((float)y / (float)GenerationConstants.voxelSizeY);
            int chunkZ = (int)MathF.Floor((float)z / (float)GenerationConstants.voxelSizeZ);

            int relativeX = x - chunkX * GenerationConstants.voxelSizeX;
            int relativeY = y - chunkY * GenerationConstants.voxelSizeY;
            int relativeZ = z - chunkZ * GenerationConstants.voxelSizeZ;

            int chunkID = chunkX + chunkY * sizeX + chunkZ * sizeX * sizeY;
            int voxelID = relativeX + relativeY * GenerationConstants.voxelSizeX
                + relativeZ * GenerationConstants.voxelSizeX * GenerationConstants.voxelSizeY;

            if (voxelID < 0)
                return (chunkID, voxelID);

            return (chunkID, voxelID);

        }

        int GetChunkID(int x, int y, int z) {
            return x + y * sizeX + z * sizeX * sizeY;
        }

        static (short, short, short, short) GetFromVoxelColor(ushort value) {
            short sun = (short)(value & 0b_1111);
            short blue = (short)((value >> 4) & 0b_1111);
            short green = (short)((value >> 8) & 0b_1111);
            short red = (short)((value >> 12) & 0b_1111);

            return (red, green, blue, sun);

        }


        static ushort GetVoxelColor(byte red, byte green, byte blue, byte sun) {
            red = (byte)(0b_1111 & red);
            green = (byte)(0b_1111 & green);
            blue = (byte)(0b_1111 & blue);
            sun = (byte)(0b_1111 & sun);

            ushort value = sun;
            value += (ushort)(blue >> 4);
            value += (ushort)(green >> 8);
            value += (ushort)(red >> 12);

            return value;

        }


    }
}
