
namespace VoxelNow.Rendering.MeshData {
    internal class MapMeshData : IMeshData {
        public ushort renderObjectID { get { return 0x02; } }

        public float[] vertexData;
        public byte[] colorData;
        public uint[] indices;

    }
}
