using VoxelNow.API;

namespace VoxelNow.Assets.Voxels {
    public class Leave : IVoxelData {
        public uint voxelID { get { return 0x06; } }
        public VoxelType voxelType { get { return VoxelType.TransparentVoxel; } }

        public VoxelRenderingFaceMode renderingFaceMode { get { return VoxelRenderingFaceMode.Static; } }


        public TextureCoord[] textureCoordsFaces { get; } = new TextureCoord[6] {
            new TextureCoord(7,0),
            new TextureCoord(7,0),
            new TextureCoord(7,0),
            new TextureCoord(7,0),
            new TextureCoord(7,0),
            new TextureCoord(7,0)
        };

        public bool isProcedural { get { return false; } }
        public ushort proceduralObjectReference => throw new NotImplementedException();

        public bool hasLightScattering { get { return true; } }

        public byte lightResistence { get { return 40; } }
    }
}
