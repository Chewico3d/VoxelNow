
namespace VoxelNow.Rendering.MeshData {
    internal class SolidkMeshData : IMeshData {
        public ushort renderObjectID { get { return 0x01; } }

        internal byte[] v_Positions;
        internal byte[] v_UV;
        internal ushort[] v_VertexShadow;
        internal byte[] v_Normal;
        internal byte[] v_PlaneUV;
        internal byte[] v_MaterialInfo;
        internal uint[] indices;

    }
}
