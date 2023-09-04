﻿using System;
using System.Linq.Expressions;

namespace VoxelNow.Core {
    public class Chunk {
        public readonly int IDx, IDy, IDz;

        public ushort[] voxels;
        public byte[] voxelsState;
        public ushort[] lightValue;//rrrrggggbbbbssss

        public Chunk(int IDx, int IDy, int IDz) {
            int voxelCount = ChunkGenerationConstants.voxelSizeX * ChunkGenerationConstants.voxelSizeY
                * ChunkGenerationConstants.voxelSizeZ;
            voxels = new ushort[voxelCount];
            voxelsState = new byte[voxelCount];
            lightValue = new ushort[voxelCount];

            this.IDx = IDx;
            this.IDy = IDy;
            this.IDz = IDz;

        }

        public int GetChunkID(int x, int y, int z) {
            return x + y * ChunkGenerationConstants.voxelSizeX
                + z * ChunkGenerationConstants.voxelSizeX * ChunkGenerationConstants.voxelSizeY;
        }


    }
}
