
using VoxelNow.API;

namespace VoxelNow.Assets.Voxels {
    public class TreeSeed : IVoxelData {
        public uint voxelID { get { return 0x200; } }

        public bool isProcedural { get { return true; } }
        public ushort proceduralObjectReference { get { return 0x00; } }

        public VoxelType voxelType { get { return VoxelType.NotRender; } }

        public VoxelRenderingFaceMode renderingFaceMode => throw new NotImplementedException();
        public TextureCoord[] textureCoordsFaces => throw new NotImplementedException();
    }
}
