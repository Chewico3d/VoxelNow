namespace VoxelNow.API
{
    public interface IVoxelData
    {

        public uint voxelID { get; }

        public bool isProcedural { get; }
        public ushort proceduralObjectReference { get; }

        public VoxelType voxelType { get; }
        public VoxelRenderingFaceMode renderingFaceMode { get; }

        public byte lightResistence { get; } 

        public TextureCoord[] textureCoordsFaces { get; }
        public bool hasLightScattering { get; }


    }
}
