
namespace VoxelNow.Rendering.MeshData {
    public class FluidMeshData : IMeshData {
        public ushort renderObjectID { get { return 0x03; } }

        public byte[] vPosition;
        public uint[] indices;

    }
}
