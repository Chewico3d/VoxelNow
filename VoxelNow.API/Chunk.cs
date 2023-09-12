using System;
using System.Linq.Expressions;

namespace VoxelNow.Core {
    public class Chunk {
        public readonly int IDx, IDy, IDz;
        public readonly int worldOffsetX, worldOffsetY, worldOffsetZ;

        public ushort[] voxels;
        public byte[] voxelsState;
        public ushort[] lightValue;//rrrrggggbbbbssss


        public Chunk(int IDx, int IDy, int IDz) {
            int voxelCount = GenerationConstants.voxelSizeX * GenerationConstants.voxelSizeY
                * GenerationConstants.voxelSizeZ;
            voxels = new ushort[voxelCount];
            voxelsState = new byte[voxelCount];
            lightValue = new ushort[voxelCount];

            this.IDx = IDx;
            this.IDy = IDy;
            this.IDz = IDz;

            this.worldOffsetX = IDx * GenerationConstants.voxelSizeX;
            this.worldOffsetY = IDy * GenerationConstants.voxelSizeY;
            this.worldOffsetZ = IDz * GenerationConstants.voxelSizeZ;

        }

        public int GetChunkID(int x, int y, int z) {
            return x + y * GenerationConstants.voxelSizeX
                + z * GenerationConstants.voxelSizeX * GenerationConstants.voxelSizeY;
        }


    }
}
