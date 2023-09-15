
using VoxelNow.Rendering.FabricData;
using VoxelNow.Rendering.MeshData;
using VoxelNow.API;
using VoxelNow.AssemblyLoader;

namespace VoxelNow.Rendering.Fabrics {
    internal class SolidFabric : IObjectFabric {
        public ushort renderObjectID { get { return 0x01; } }

        List<byte> v_Positions = new List<byte>();
        List<byte> v_UV = new List<byte>();
        List<byte> v_PlaneUV = new List<byte>();
        List<ushort> v_VertexShadow = new List<ushort>();
        List<byte> v_Normals = new List<byte>();
        List<byte> v_MaterialProps = new List<byte>();
        List<uint> indices = new List<uint>();

        SolidFabricData workingData;
        public IMeshData GenerateMeshData(IFabricData fabricData) {

            workingData = (SolidFabricData)fabricData;
            v_Positions.Clear();
            v_UV.Clear();
            v_PlaneUV.Clear();
            v_Normals.Clear();
            v_VertexShadow.Clear();
            v_MaterialProps.Clear();
            indices.Clear();

            for (int x = 0; x < GenerationConstants.voxelSizeX; x++) {
                for (int y = 0; y < GenerationConstants.voxelSizeY; y++) {
                    for (int z = 0; z < GenerationConstants.voxelSizeZ; z++) {
                        ProcessVoxel(x, y, z);
                    }
                }
            }

            SolidkMeshData solidChunkMeshData = new SolidkMeshData();
            solidChunkMeshData.v_Positions = v_Positions.ToArray();
            solidChunkMeshData.indices = indices.ToArray();
            solidChunkMeshData.v_UV = v_UV.ToArray();
            solidChunkMeshData.v_PlaneUV = v_PlaneUV.ToArray();
            solidChunkMeshData.v_VertexShadow = v_VertexShadow.ToArray();
            solidChunkMeshData.v_Normal = v_Normals.ToArray();
            solidChunkMeshData.v_MaterialInfo = v_MaterialProps.ToArray();

            return solidChunkMeshData;

        }

        void ProcessVoxel(int x, int y, int z) {

            if (!IsVoxel(x, y, z))
                return;

            ushort currentVoxelID = workingData.GetVoxel(x, y, z);

            bool currentVoxelIsTransparent = VoxelAssets.IsTransparent(currentVoxelID);
            bool TransparentDecay = false;


            for (int direction = 0; direction < 6; direction++) {

                int checkX = x + VoxelData.searchOrder[direction * 3 + 0];
                int checkY = y + VoxelData.searchOrder[direction * 3 + 1];
                int checkZ = z + VoxelData.searchOrder[direction * 3 + 2];

                if (!currentVoxelIsTransparent) {
                    if (IsSolidVoxel(checkX, checkY, checkZ))
                        continue;
                } else {
                    if (IsSolidVoxel(checkX, checkY, checkZ))
                        continue;
                    if (currentVoxelID == workingData.GetVoxel(checkX, checkY, checkZ))
                        continue;
                }

                int initialVertexID = v_Positions.Count / 3;
                byte[] vertexShadow = new byte[4];

                TextureCoord textureCoord = getTextureCoord(currentVoxelID, direction);

                for (int vertex = 0; vertex < 4; vertex++) {

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

                    byte shadow = GetVertexLight(x, y, z, direction, vertex);
                    vertexShadow[vertex] = (byte)(shadow);

                    v_Normals.Add((byte)direction);

                }

                ushort shadowFace = vertexShadow[0];
                shadowFace += (ushort)(vertexShadow[1] << 4);
                shadowFace += (ushort)(vertexShadow[2] << 8);
                shadowFace += (ushort)(vertexShadow[3] << 12);
                //v_VertexShadow.Add(shadow);

                for (int vertex = 0; vertex < 4; vertex++)
                    v_VertexShadow.Add(shadowFace);


                for (int triangle = 0; triangle < 6; triangle++)
                    indices.Add((uint)(VoxelData.triangleConfiguration[triangle] + initialVertexID));

                if (currentVoxelIsTransparent)
                    for (int triangle = 0; triangle < 6; triangle++)
                        indices.Add((uint)(VoxelData.invertedFace[triangle] + initialVertexID));


            }

        }

        byte GetVertexLight(int posX, int posY, int posZ, int direction, int vertex) {

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

            int lightValue0 = workingData.GetVoxelLight(posX + checkDir0X + UpX, posY + checkDir0Y + UpY, posZ + checkDir0Z + UpZ);
            int lightValue1 = workingData.GetVoxelLight(posX + checkDir1X + UpX, posY + checkDir1Y + UpY, posZ + checkDir1Z + UpZ);
            int lightMiddle = workingData.GetVoxelLight(posX + checkDir0X + checkDir1X + UpX, posY + checkDir0Y + checkDir1Y + UpY, posZ + checkDir0Z + checkDir1Z + UpZ);
            int lightUp = workingData.GetVoxelLight(posX + UpX, posY + UpY, posZ + UpZ);

            ushort voxel0ID = workingData.GetVoxel(posX + checkDir0X + UpX, posY + checkDir0Y + UpY, posZ + checkDir0Z + UpZ);
            ushort voxel1ID = workingData.GetVoxel(posX + checkDir1X + UpX, posY + checkDir1Y + UpY, posZ + checkDir1Z + UpZ);

            bool isVoxel0Solid = VoxelAssets.IsSolid(voxel0ID);
            bool isVoxel1Solid = VoxelAssets.IsSolid(voxel1ID);

            if (isVoxel0Solid & isVoxel1Solid)
                lightMiddle = 0;

            float average = lightValue0 + lightValue1 + lightMiddle + lightUp;
            average /= 68;
            average = MathF.Round(average);

            if (average >= 16)
                average = 15;

            return (byte)average;

        }

        bool IsSolidVoxel(int x, int y, int z) {
            ushort voxelID = workingData.GetVoxel(x, y, z);
            return VoxelAssets.IsSolid(voxelID);

        }

        bool IsVoxel(int x, int y, int z) {
            ushort voxelID = workingData.GetVoxel(x, y, z);
            return VoxelAssets.IsVoxel(voxelID);
        }

        bool IsTransparent(int x, int y, int z) {
            ushort voxelID = workingData.GetVoxel(x, y, z);
            return VoxelAssets.IsTransparent(voxelID);
        }

        TextureCoord getTextureCoord(ushort voxelID, int direction) {
            if (VoxelAssets.IsSolidRender(voxelID))
                return VoxelAssets.GetTextureCoord(voxelID)[direction];
            return new TextureCoord(255, 255);
        }


    }
}
