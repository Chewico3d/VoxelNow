using VoxelNow.API;

namespace VoxelNow.Assets.Voxels
{
    public class Grass : IVoxelData {
        public uint voxelID { get { return 0x04; } }
        public VoxelType voxelType { get { return VoxelType.SolidVoxel; } }

        public VoxelRenderingFaceMode renderingFaceMode { get { return VoxelRenderingFaceMode.Static; } }

        public TextureCoord[] textureCoordsFaces { get; } = new TextureCoord[6] {
            new TextureCoord(3,0),
            new TextureCoord(3,0),
            new TextureCoord(2,0),
            new TextureCoord(4,0),
            new TextureCoord(3,0),
            new TextureCoord(3,0)
        };
    }
}
