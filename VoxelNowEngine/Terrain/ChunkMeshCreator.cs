using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNowEngine.Graphics.Objects;

namespace VoxelNowEngine.Terrain
{
    public static class ChunkMeshCreator {

        static readonly byte[][] CubeVerticesPositions = {
            new byte[]{0,0,1,0,0,0,0,1,1,0,1,0},
            new byte[]{1,0,0,1,0,1,1,1,0,1,1,1},
            new byte[]{0,0,1,1,0,1,0,0,0,1,0,0},
            new byte[]{0,1,0,1,1,0,0,1,1,1,1,1},
            new byte[]{0,0,0,1,0,0,0,1,0,1,1,0},
            new byte[]{1,0,1,0,0,1,1,1,1,0,1,1},
        };
        static readonly byte[] CubeVerticesUV = new byte[8] { 0, 0, 1, 0, 0, 1, 1, 1 };
        static readonly ushort[] IndexPatern = { 0, 2, 1, 2, 3, 1 };
        static readonly ushort[] IndexPaternInverted = { 0, 3, 1, 3, 0, 2 };
        static readonly Vector3i[] CubeDirectionsOrder = {
            new Vector3i(-1,0,0),
            new Vector3i( 1,0,0),
            new Vector3i(0,-1,0),
            new Vector3i(0, 1,0),
            new Vector3i(0,0,-1),
            new Vector3i(0,0, 1)
        };
        static readonly Vector3i[][][] CubeAmbientOclusionAxis = new Vector3i[6][][] {
            new Vector3i[4][] {
            new Vector3i[2] { new Vector3i(0 ,-1 ,0 ),new Vector3i(0 ,0 ,1 ) },
            new Vector3i[2] { new Vector3i(0 ,-1 ,0 ),new Vector3i(0 ,0 ,-1 ) },
            new Vector3i[2] { new Vector3i(0 ,1 ,0 ),new Vector3i(0 ,0 ,1 ) },
            new Vector3i[2] { new Vector3i(0 ,1 ,0 ),new Vector3i(0 ,0 ,-1 ) }, },
            new Vector3i[4][] {
            new Vector3i[2] { new Vector3i(0 ,-1 ,0 ),new Vector3i(0 ,0 ,-1 ) },
            new Vector3i[2] { new Vector3i(0 ,-1 ,0 ),new Vector3i(0 ,0 ,1 ) },
            new Vector3i[2] { new Vector3i(0 ,1 ,0 ),new Vector3i(0 ,0 ,-1 ) },
            new Vector3i[2] { new Vector3i(0 ,1 ,0 ),new Vector3i(0 ,0 ,1 ) }, },
            new Vector3i[4][] {
            new Vector3i[2] { new Vector3i(-1 ,0 ,0 ),new Vector3i(0 ,0 ,1 ) },
            new Vector3i[2] { new Vector3i(1 ,0 ,0 ),new Vector3i(0 ,0 ,1 ) },
            new Vector3i[2] { new Vector3i(-1 ,0 ,0 ),new Vector3i(0 ,0 ,-1 ) },
            new Vector3i[2] { new Vector3i(1 ,0 ,0 ),new Vector3i(0 ,0 ,-1 ) }, },
            new Vector3i[4][] {
            new Vector3i[2] { new Vector3i(-1 ,0 ,0 ),new Vector3i(0 ,0 ,-1 ) },
            new Vector3i[2] { new Vector3i(1 ,0 ,0 ),new Vector3i(0 ,0 ,-1 ) },
            new Vector3i[2] { new Vector3i(-1 ,0 ,0 ),new Vector3i(0 ,0 ,1 ) },
            new Vector3i[2] { new Vector3i(1 ,0 ,0 ),new Vector3i(0 ,0 ,1 ) }, },
            new Vector3i[4][] {
            new Vector3i[2] { new Vector3i(-1 ,0 ,0 ),new Vector3i(0 ,-1 ,0 ) },
            new Vector3i[2] { new Vector3i(1 ,0 ,0 ),new Vector3i(0 ,-1 ,0 ) },
            new Vector3i[2] { new Vector3i(-1 ,0 ,0 ),new Vector3i(0 ,1 ,0 ) },
            new Vector3i[2] { new Vector3i(1 ,0 ,0 ),new Vector3i(0 ,1 ,0 ) }, },
            new Vector3i[4][] {
            new Vector3i[2] { new Vector3i(1 ,0 ,0 ),new Vector3i(0 ,-1 ,0 ) },
            new Vector3i[2] { new Vector3i(-1 ,0 ,0 ),new Vector3i(0 ,-1 ,0 ) },
            new Vector3i[2] { new Vector3i(1 ,0 ,0 ),new Vector3i(0 ,1 ,0 ) },
            new Vector3i[2] { new Vector3i(-1 ,0 ,0 ),new Vector3i(0 ,1 ,0 ) }, }, 
        };
        static readonly byte[] AmbientOclusionTable = new byte[8] { 0, 150, 150, 150, 150, 255, 150, 255 };

        static Chunk processingChunk;

        static List<byte> vPositions;
        static List<byte> vNormal;
        static List<byte> vTexture;
        static List<byte> vAmbientOculusion;

        static List<ushort> indices;
        internal static SolidChunkRenderObject.Data CreateSolidChunkRenderObject(Chunk chunk) {

            processingChunk = chunk;

            vPositions = new List<byte>();
            vNormal = new List<byte>();
            vTexture = new List<byte>();
            vAmbientOculusion = new List<byte>();
            indices = new List<ushort>();

            for (int x = 1; x < 17; x++) {
                for (int y = 1; y < 257; y++)
                    for (int z = 1; z < 17; z++)
                        ProcessNode(x, y, z);
            }


            return new SolidChunkRenderObject.Data(vPositions.ToArray(), vNormal.ToArray(), vTexture.ToArray(), vAmbientOculusion.ToArray(), indices.ToArray());
        }

        private static void ProcessNode(int x, int y, int z) {

            byte currentBlock = processingChunk.solidBlocks[z * 18 * 258 + y * 18 + x];
            if (currentBlock == 0)
                return;

            for (int direction = 0; direction < 6; direction++) {
                int processingDirectionX = CubeDirectionsOrder[direction].X + x;
                int processingDirectionY = CubeDirectionsOrder[direction].Y + y;
                int processingDirectionZ = CubeDirectionsOrder[direction].Z + z;

                byte processingDirectionValue = processingChunk.solidBlocks[processingDirectionZ * 18 * 258 + processingDirectionY * 18 + processingDirectionX];

                if (processingDirectionValue != 0)
                    continue;

                int initialVertexID = (ushort)(vPositions.Count / 3);


                byte[] ambientOclusionAxis = new byte[4];

                for (int vertexItirenation = 0; vertexItirenation < 4; vertexItirenation++) {
                    vPositions.Add((byte)(CubeVerticesPositions[direction][vertexItirenation * 3 + 0] - 1 + x));
                    vPositions.Add((byte)(CubeVerticesPositions[direction][vertexItirenation * 3 + 1] - 1 + y));
                    vPositions.Add((byte)(CubeVerticesPositions[direction][vertexItirenation * 3 + 2] - 1 + z));


                    int textureOffsetX = NodesDatabase.solidNodes[currentBlock - 1].PositionUVNodes[direction][0];
                    int textureOffsetY = NodesDatabase.solidNodes[currentBlock - 1].PositionUVNodes[direction][1];
                    vTexture.Add((byte)(CubeVerticesUV[vertexItirenation * 2 + 0] + textureOffsetX));
                    vTexture.Add((byte)(CubeVerticesUV[vertexItirenation * 2 + 1] + textureOffsetY));

                    vNormal.Add((byte)direction);

                    //Ambient oclusion
                    Vector3i ambientOclusion0 = CubeAmbientOclusionAxis[direction][vertexItirenation][0];
                    Vector3i ambientOclusion1 = CubeAmbientOclusionAxis[direction][vertexItirenation][1];
                    Vector3i ambientOclusion2 = ambientOclusion0 + ambientOclusion1;

                    ambientOclusion0 += CubeDirectionsOrder[direction] + new Vector3i(x, y, z);
                    ambientOclusion1 += CubeDirectionsOrder[direction] + new Vector3i(x, y, z);
                    ambientOclusion2 += CubeDirectionsOrder[direction] + new Vector3i(x, y, z);

                    byte valueOclusion0 = processingChunk.solidBlocks[ambientOclusion0.Z * 18 * 258 + ambientOclusion0.Y * 18 + ambientOclusion0.X];
                    byte valueOclusion1 = processingChunk.solidBlocks[ambientOclusion1.Z * 18 * 258 + ambientOclusion1.Y * 18 + ambientOclusion1.X];
                    byte valueOclusion2 = processingChunk.solidBlocks[ambientOclusion2.Z * 18 * 258 + ambientOclusion2.Y * 18 + ambientOclusion2.X];

                    int ambientOclusionTableID = ((valueOclusion0 != 0) ? 1 : 0) + ((valueOclusion2 != 0) ? 2 : 0) + ((valueOclusion1 != 0) ? 4 : 0);
                    vAmbientOculusion.Add(AmbientOclusionTable[ambientOclusionTableID]);
                    ambientOclusionAxis[vertexItirenation] = AmbientOclusionTable[ambientOclusionTableID];

                }

                if (ambientOclusionAxis[0] + ambientOclusionAxis[3] > ambientOclusionAxis[2] + ambientOclusionAxis[1]) {

                    for (int indexItirenation = 0; indexItirenation < 6; indexItirenation++)
                        indices.Add((ushort)(initialVertexID + IndexPatern[indexItirenation]));
                } else {

                    for (int indexItirenation = 0; indexItirenation < 6; indexItirenation++)
                        indices.Add((ushort)(initialVertexID + IndexPaternInverted[indexItirenation]));
                }


            }

        }

    }
}