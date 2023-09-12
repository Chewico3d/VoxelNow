
namespace VoxelNow.Rendering.MeshData {
    public class FloatingMeshData : IMeshData {

        public List<float> v_Position = new List<float>();
        public List<int> indicies = new List<int>();

        public ushort renderObjectID { get { return 0x00; } }

    }
}
