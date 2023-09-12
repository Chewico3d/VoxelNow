
namespace VoxelNow.Rendering {
    internal interface IObjectFabric {

        public ushort renderObjectID { get; }
        public IMeshData GenerateMeshData(IFabricData fabricData);
    }
}
