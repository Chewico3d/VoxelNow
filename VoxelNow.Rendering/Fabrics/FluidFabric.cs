
using VoxelNow.API;
using VoxelNow.Rendering.FabricData;
using VoxelNow.Rendering.MeshData;

namespace VoxelNow.Rendering.Fabrics {
    public class FluidFabric : IObjectFabric {
        public ushort renderObjectID { get { return 0x03; } }
        FluidFabricData fluidData;

        List<byte> v_Position = new List<byte>();
        List<uint> indices = new List<uint>();

        public IMeshData GenerateMeshData(IFabricData fabricData) {
            fluidData = (FluidFabricData)fabricData;

            v_Position.Clear();
            indices.Clear();

            for (int x = 0; x < GenerationConstants.voxelSizeX; x++) {
                for (int y = 0; y < GenerationConstants.voxelSizeY; y++)
                    for (int z = 0; z < GenerationConstants.voxelSizeZ; z++)
                        ProcessVoxel(x, y, z);
            }

            FluidMeshData fluidMeshData = new FluidMeshData() {
                indices = indices.ToArray(),
                vPosition = v_Position.ToArray()
            };

            return fluidMeshData;

        }

        void ProcessVoxel(int x, int y, int z) {

            byte voxelWaterValue = fluidData.GetWaterValue(x, y, z);
            if (voxelWaterValue == 0)
                return;

            byte waterValueUp = fluidData.GetWaterValue(x, y + 1, z);
            if(waterValueUp == 0) {

                uint initialIndex = (uint)v_Position.Count / 3;

                for(int vertex = 0; vertex < 4; vertex++) {

                    int direction = 3;
                    int verticesPerFaceRow = direction * 4 + vertex;

                    int xPos = VoxelData.cubeVerticesPositions[VoxelData.verticesPerFace[verticesPerFaceRow] * 3 + 0] + x;
                    int yPos = VoxelData.cubeVerticesPositions[VoxelData.verticesPerFace[verticesPerFaceRow] * 3 + 1] + y;
                    int zPos = VoxelData.cubeVerticesPositions[VoxelData.verticesPerFace[verticesPerFaceRow] * 3 + 2] + z;

                    v_Position.Add((byte)xPos);
                    v_Position.Add((byte)yPos);
                    v_Position.Add((byte)zPos);

                }

                for (int triangle = 0; triangle < 6; triangle++)
                    indices.Add(VoxelData.triangleConfiguration[triangle] + initialIndex);

            }

        }
        
    }
}
