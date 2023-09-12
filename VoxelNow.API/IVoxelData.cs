namespace VoxelNow.API
{
    public interface IVoxelData
    {

        public uint voxelID { get; }

        public bool isProcedural { get; }
        public ushort proceduralObjectReference { get; }

        public VoxelType voxelType { get; }
        public VoxelRenderingFaceMode renderingFaceMode { get; }

        //Case solid
        public TextureCoord[] textureCoordsFaces { get; }


    }
}
