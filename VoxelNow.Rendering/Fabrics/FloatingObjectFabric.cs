
using VoxelNow.Rendering.FabricData;
using VoxelNow.Rendering.MeshData;

namespace VoxelNow.Rendering.ObjectsFabrics {
    internal class FloatingObjectFabric : IObjectFabric {
        public ushort renderObjectID { get { return 0x00; } }

        public IMeshData GenerateMeshData(IFabricData fabricData) {
            FloatingMeshData meshData = new FloatingMeshData();
            FloatingFabricData floatingFabricData = (FloatingFabricData)fabricData;

            meshData.v_Position = floatingFabricData.v_Position;
            meshData.indicies = floatingFabricData.indicies;

            return meshData;
        }
    }
}
