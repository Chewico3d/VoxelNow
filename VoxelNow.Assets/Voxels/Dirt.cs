
using VoxelNow.API;

namespace VoxelNow.Assets.Voxels {
    public class Dirt : IVoxelData {
        public uint voxelID { get { return 0x01; } }
        public VoxelType voxelType { get{ return VoxelType.SolidVoxel; } }

        public VoxelRenderingFaceMode renderingFaceMode { get { return VoxelRenderingFaceMode.Static; } }

        public TextureCoord[] textureCoordsFaces { get; } = new TextureCoord[6] {
            new TextureCoord(1,1),
            new TextureCoord(1,1),
            new TextureCoord(1,1),
            new TextureCoord(1,1),
            new TextureCoord(1,1),
            new TextureCoord(1,1)
        };
    }
}
