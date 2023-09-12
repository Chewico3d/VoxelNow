
using VoxelNow.API;

namespace VoxelNow.Assets.Voxels {
    public class Sand : IVoxelData {
        public uint voxelID { get { return 0x02; } }
        public VoxelType voxelType { get { return VoxelType.SolidVoxel; } }

        public VoxelRenderingFaceMode renderingFaceMode { get { return VoxelRenderingFaceMode.Static; } }

        public TextureCoord[] textureCoordsFaces { get; } = new TextureCoord[6] {
            new TextureCoord(1,0),
            new TextureCoord(1,0),
            new TextureCoord(1,0),
            new TextureCoord(1,0),
            new TextureCoord(1,0),
            new TextureCoord(1,0)
        };
    }
}