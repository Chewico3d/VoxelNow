
namespace VoxelNow.Rendering {
    public interface IRenderObject {
        public ushort renderObjectID { get; }

        public void LoadData(IMeshData meshData);
        public bool Draw();

    }
}
