using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Rendering.FabricData;
using VoxelNow.Rendering.MeshData;

namespace VoxelNow.Rendering.Fabrics {
    internal class SolidChunkFabric : IObjectFabric {
        public ushort renderObjectID { get { return 0x01; } }

        List<byte> v_Positions = new List<byte>();
        List<byte> v_UV = new List<byte>();
        List<byte> ambientOclussion = new List<byte>();
        List<ushort> indices = new List<ushort>();

        SolidCunkFabricData workingData;

        void ProcessVoxel(int x, int y, int z) {

            ushort voxelID = workingData.GetVoxel(x, y, z);

            if (voxelID == 0)
                return;

            for(int direction = 0; direction < 6; direction++) {

                ushort neighbor = workingData.GetVoxel(x + VoxelData.searchOrder[direction * 3 + 0],
                    y + VoxelData.searchOrder[direction * 3 + 1],
                    z + VoxelData.searchOrder[direction * 3 + 2]);

                if (neighbor != 0)
                    continue;

                int initialVertexID = v_Positions.Count / 3;

                byte[] vertexAO = new byte[4];
                
                for(int vertex = 0; vertex < 4; vertex++) {

                    int verticesPerFaceRow = direction * 4 + vertex;
                    int positionX = VoxelData.cubeVerticesPositions[VoxelData.verticesPerFace[verticesPerFaceRow] * 3 + 0];
                    int positionY = VoxelData.cubeVerticesPositions[VoxelData.verticesPerFace[verticesPerFaceRow] * 3 + 1];
                    int positionZ = VoxelData.cubeVerticesPositions[VoxelData.verticesPerFace[verticesPerFaceRow] * 3 + 2];

                    positionX += x; 
                    positionY += y; 
                    positionZ += z;

                    //We multiply for 7 to have sub 7 parts in a voxel and still use byte to make from voxel 0 to 32
                    positionX *= 7;
                    positionY *= 7;
                    positionZ *= 7;

                    v_Positions.Add((byte)positionX);
                    v_Positions.Add((byte)positionY);
                    v_Positions.Add((byte)positionZ);

                    int UV_CordX = VoxelData.UV_Cords[vertex * 2 + 0];
                    int UV_CordY = VoxelData.UV_Cords[vertex * 2 + 1];

                    v_UV.Add((byte)UV_CordX);
                    v_UV.Add((byte)UV_CordY);

                    byte ambientOcclusion = GetAmbientOclussion(x, y, z, direction, vertex);
                    vertexAO[vertex] = ambientOcclusion;    
                    ambientOclussion.Add(ambientOcclusion);

                }

                if (vertexAO[0] + vertexAO[3] > vertexAO[1] + vertexAO[2]) {
                    for (int triangle = 0; triangle < 6; triangle++)
                        indices.Add((ushort)(VoxelData.triangleConfiguration[triangle] + initialVertexID));
                } else {
                    for (int triangle = 0; triangle < 6; triangle++)
                        indices.Add((ushort)(VoxelData.alternativeTriangleConfiguration[triangle] + initialVertexID));
                }


            }

        }

        byte GetAmbientOclussion(int posX, int posY, int posZ, int direction, int vertex) {

            int directionOffset = direction * 4 * 2 * 3;
            int vertexOffset = vertex * 2 * 3;

            int UpX = VoxelData.searchOrder[direction * 3 + 0];
            int UpY = VoxelData.searchOrder[direction * 3 + 1];
            int UpZ = VoxelData.searchOrder[direction * 3 + 2];

            int checkDir0X = VoxelData.ambientOclusionCheckDirection[directionOffset + vertexOffset + 0];
            int checkDir0Y = VoxelData.ambientOclusionCheckDirection[directionOffset + vertexOffset + 1];
            int checkDir0Z = VoxelData.ambientOclusionCheckDirection[directionOffset + vertexOffset + 2];

            int secondValueOffset = 3;
            int checkDir1X = VoxelData.ambientOclusionCheckDirection[directionOffset + vertexOffset + secondValueOffset + 0];
            int checkDir1Y = VoxelData.ambientOclusionCheckDirection[directionOffset + vertexOffset + secondValueOffset + 1];
            int checkDir1Z = VoxelData.ambientOclusionCheckDirection[directionOffset + vertexOffset + secondValueOffset + 2];
            
            ushort value0 = workingData.GetVoxel(posX + checkDir0X + UpX, posY + checkDir0Y + UpY, posZ + checkDir0Z + UpZ);
            ushort value1 = workingData.GetVoxel(posX + checkDir1X + UpX, posY + checkDir1Y + UpY, posZ + checkDir1Z + UpZ);
            ushort valueMiddle = workingData.GetVoxel(posX + checkDir0X + checkDir1X + UpX, posY + checkDir0Y + checkDir1Y + UpY, posZ + checkDir0Z + checkDir1Z + UpZ);

            int ambientID = 0;
            if (value0 != 0)
                ambientID += 1;
            if (valueMiddle != 0)
                ambientID += 2;
            if (value1 != 0)
                ambientID += 4;

            return VoxelData.ambientOcclusionResult[ambientID];

        }

        public IMeshData GenerateMeshData(IFabricData fabricData) {

            workingData = (SolidCunkFabricData)fabricData;
            v_Positions.Clear();
            v_UV.Clear();
            indices.Clear();

            for(int x = 0; x < ChunkGenerationConstants.voxelSizeX; x++) {
                for(int y = 0; y < ChunkGenerationConstants.voxelSizeY; y++) {
                    for(int z = 0; z < ChunkGenerationConstants.voxelSizeZ; z++) {
                        ProcessVoxel(x, y, z);
                    }
                }
            }

            SolidChunkMeshData solidChunkMeshData = new SolidChunkMeshData();
            solidChunkMeshData.v_Positions = v_Positions.ToArray();
            solidChunkMeshData.indices = indices.ToArray();
            solidChunkMeshData.UVs = v_UV.ToArray();
            solidChunkMeshData.v_AmbientOclusion = ambientOclussion.ToArray();

            return solidChunkMeshData;

        }
    }
}
