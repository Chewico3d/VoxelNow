using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Core {
    public class ChunkDatabase {

        public readonly int sizeX, sizeY, sizeZ;
        public readonly Chunk[] chunks;

        public ChunkDatabase(int sizeX, int sizeY, int sizeZ) {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;

            chunks = new Chunk[sizeX * sizeY * sizeZ];
            int itirenation = 0;
            for(int x = 0; x < sizeX; x++) {
                for(int y = 0; y < sizeY; y++)
                    for(int z = 0; z < sizeZ; z++) {

                        chunks[itirenation++] = new Chunk(x, y, z);
                        itirenation++;

                    }
            }

        }

        void SetVoxelID(int x, int y, int z, ushort value) {
            (int, int) voxelPos = GetVoxelCordinates(x, y, z);
            chunks[voxelPos.Item1].voxels[voxelPos.Item2] = value;
        }
        ushort GetVoxelID(int x, int y, int z) {
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
            int chunkX = (int)MathF.Floor((float)x / ChunkGenerationConstants.voxelSizeX);
            int chunkY = (int)MathF.Floor((float)y / ChunkGenerationConstants.voxelSizeY);
            int chunkZ = (int)MathF.Floor((float)z / ChunkGenerationConstants.voxelSizeZ);

            int relativeX = x - chunkX * ChunkGenerationConstants.voxelSizeX;
            int relativeY = y - chunkX * ChunkGenerationConstants.voxelSizeY;
            int relativeZ = z - chunkX * ChunkGenerationConstants.voxelSizeZ;

            int chunkID = chunkX + chunkY * sizeX + chunkZ * sizeX * sizeY;
            int voxelID = relativeX + relativeY * ChunkGenerationConstants.voxelSizeX
                + relativeZ * ChunkGenerationConstants.voxelSizeX * ChunkGenerationConstants.voxelSizeZ;

            return (chunkID, voxelID);

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
