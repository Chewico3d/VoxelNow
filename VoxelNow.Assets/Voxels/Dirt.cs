﻿using VoxelNow.API;

namespace VoxelNow.Assets.Voxels
{
    public class Dirt : IVoxelData {
        public uint voxelID { get { return 0x03; } }
        public VoxelType voxelType { get{ return VoxelType.SolidVoxel; } }

        public VoxelRenderingFaceMode renderingFaceMode { get { return VoxelRenderingFaceMode.Static; } }

        public TextureCoord[] textureCoordsFaces { get; } = new TextureCoord[6] {
            new TextureCoord(2,0),
            new TextureCoord(2,0),
            new TextureCoord(2,0),
            new TextureCoord(2,0),
            new TextureCoord(2,0),
            new TextureCoord(2,0)
        };

        public bool isProcedural { get { return false; } }
        public ushort proceduralObjectReference => throw new NotImplementedException();

        public bool hasLightScattering { get { return false; } }

        public byte lightResistence => throw new NotImplementedException();
    }
}
