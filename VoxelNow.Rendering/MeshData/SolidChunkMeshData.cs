
namespace VoxelNow.Rendering.MeshData {
    internal class SolidChunkMeshData : IMeshData {
        public ushort renderObjectID { get { return 0x01; } }

        internal byte[] v_Positions;
        internal byte[] v_UV;
        internal byte[] v_AmbientOclusion;
        internal byte[] v_Normal;
        internal byte[] v_PlaneUV;
        internal ushort[] indices;

    }
}
