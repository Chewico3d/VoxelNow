using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Core;
using VoxelNow.Rendering.FabricData;
using VoxelNow.Rendering.MeshData;
using VoxelNow.API.API;

namespace VoxelNow.Rendering.Fabrics
{
    internal class SolidChunkFabric : IObjectFabric {
        public ushort renderObjectID { get { return 0x01; } }

        List<byte> v_Positions = new List<byte>();
        List<byte> v_UV = new List<byte>();
        List<byte> v_PlaneUV = new List<byte>();
        List<byte> v_ambientOclussion = new List<byte>();
        List<byte> v_Normals = new List<byte>();
        List<ushort> indices = new List<ushort>();

        SolidCunkFabricData workingData;

        bool IsSolidBlock(int x, int y, int z) {
            ushort voxelID = workingData.GetVoxel(x, y, z);
            if (voxelID == 0)
                return false;
            return AssetLoader.voxelsData[voxelID].voxelType == VoxelType.SolidVoxel;

        }

        TextureCoord getTextureCoord(ushort voxelID, int direction) {
            if(AssetLoader.voxelsData[voxelID].renderingFaceMode == VoxelRenderingFaceMode.Static)
                return AssetLoader.voxelsData[voxelID].textureCoordsFaces[direction];
            return new TextureCoord(255,255);
        }

        void ProcessVoxel(int x, int y, int z) {

            if (!IsSolidBlock(x,y,z))
                return;

            for(int direction = 0; direction < 6; direction++) {

                bool isNeighbourSolidBlock = IsSolidBlock(x + VoxelData.searchOrder[direction * 3 + 0],
                    y + VoxelData.searchOrder[direction * 3 + 1],
                    z + VoxelData.searchOrder[direction * 3 + 2]);

                if (isNeighbourSolidBlock)
                    continue;
                ushort voxelID = workingData.GetVoxel(x, y, z);

                int initialVertexID = v_Positions.Count / 3;

                byte[] vertexAO = new byte[4];

                TextureCoord textureCoord = getTextureCoord(voxelID, direction);
                
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

                    v_PlaneUV.Add((byte)UV_CordX);
                    v_PlaneUV.Add((byte)UV_CordY);

                    UV_CordX += textureCoord.posX;
                    UV_CordY += textureCoord.posY;

                    v_UV.Add((byte)UV_CordX);
                    v_UV.Add((byte)UV_CordY);

                    byte ambientOcclusion = GetAmbientOclussion(x, y, z, direction, vertex);
                    vertexAO[vertex] = ambientOcclusion;

                    v_Normals.Add((byte)direction);

                }

                int AOvalues;
                AOvalues = vertexAO[0];
                AOvalues += vertexAO[1] << 2;
                AOvalues += vertexAO[2] << 4;
                AOvalues += vertexAO[3] << 6;

                for (int vertex = 0; vertex < 4; vertex++)
                    v_ambientOclussion.Add((byte)AOvalues);

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
            
            bool value0IsSolid = IsSolidBlock(posX + checkDir0X + UpX, posY + checkDir0Y + UpY, posZ + checkDir0Z + UpZ);
            bool value1IsSolid = IsSolidBlock(posX + checkDir1X + UpX, posY + checkDir1Y + UpY, posZ + checkDir1Z + UpZ);
            bool valueMiddle = IsSolidBlock(posX + checkDir0X + checkDir1X + UpX, posY + checkDir0Y + checkDir1Y + UpY, posZ + checkDir0Z + checkDir1Z + UpZ);

            int ambientID = 0;
            if (value0IsSolid)
                ambientID += 1;
            if (valueMiddle)
                ambientID += 2;
            if (value1IsSolid)
                ambientID += 4;

            return VoxelData.ambientOcclusionResult[ambientID];

        }

        public IMeshData GenerateMeshData(IFabricData fabricData) {

            workingData = (SolidCunkFabricData)fabricData;
            v_Positions.Clear();
            v_UV.Clear();
            v_PlaneUV.Clear();
            v_Normals.Clear();
            v_ambientOclussion.Clear();
            indices.Clear();

            for(int x = 0; x < GenerationConstants.voxelSizeX; x++) {
                for(int y = 0; y < GenerationConstants.voxelSizeY; y++) {
                    for(int z = 0; z < GenerationConstants.voxelSizeZ; z++) {
                        ProcessVoxel(x, y, z);
                    }
                }
            }

            SolidChunkMeshData solidChunkMeshData = new SolidChunkMeshData();
            solidChunkMeshData.v_Positions = v_Positions.ToArray();
            solidChunkMeshData.indices = indices.ToArray();
            solidChunkMeshData.v_UV = v_UV.ToArray();
            solidChunkMeshData.v_PlaneUV = v_PlaneUV.ToArray();
            solidChunkMeshData.v_AmbientOclusion = v_ambientOclussion.ToArray();
            solidChunkMeshData.v_Normal = v_Normals.ToArray();

            return solidChunkMeshData;

        }
    }
}
