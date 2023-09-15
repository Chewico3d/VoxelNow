namespace VoxelNow.Rendering {
    internal static class VoxelData {
        internal static readonly byte[] cubeVerticesPositions = {
            0, 0, 0,
            1, 0, 0,
            0, 1, 0,
            1, 1, 0,
            0, 0, 1,
            1, 0, 1,
            0, 1, 1,
            1, 1, 1,

        };

        internal static readonly int[] searchOrder = {
            -1, 0, 0,
             1, 0, 0,
             0,-1, 0,
             0, 1, 0,
             0, 0,-1,
             0, 0, 1
        };

        internal static readonly int[] verticesPerFace = {
            4, 6, 0, 2,
            1, 3, 5, 7,
            4, 0, 5, 1,
            2, 6, 3, 7,
            0, 2, 1, 3,
            5, 7, 4, 6

        };

        internal static readonly uint[] triangleConfiguration = { 0, 2, 1, 2, 3, 1 };

        internal static readonly uint[] alternativeTriangleConfiguration = { 0, 3, 1, 0, 2, 3 };
        internal static readonly uint[] invertedFace = { 0, 1, 2, 2, 1, 3 };

        internal static readonly byte[] UV_Cords = {
            0,0, 0, 1, 1, 0, 1, 1
        };

        //Hard to explain, 6 directions * 4 vertex * 2 directions to ckeck * 3 directions to check;
        //This value is preComputed int the bottom function
        internal static int[] ambientOclusionCheckDirection = { 0, -1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, -1, 0, 0, 0, -1, 0, 1, 0, 0, 0, -1, 0, -1, 0, 0, 0, -1, 0, 1, 0, 0, 0, -1, 0, -1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, -1, 0, 0, 0, 0, 1, -1, 0, 0, 0, 0, -1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, -1, -1, 0, 0, 0, 0, -1, -1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, -1, 1, 0, 0, 0, 0, 1, -1, 0, 0, 0, -1, 0, -1, 0, 0, 0, 1, 0, 1, 0, 0, 0, -1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, -1, 0, 1, 0, 0, 0, 1, 0, -1, 0, 0, 0, -1, 0, -1, 0, 0, 0, 1, 0 };
        internal static byte[] ambientOcclusionResult = {
            0, 1, 1, 2, 1, 3, 2, 3

        };

        /*
        internal static void CalculateAmbientOcclusion() {
            ambientOclusionCheckDirection = new int[6 * 4 * 2 * 3];

            for(int dir = 0; dir < 6; dir++) {

                int dirOffset = dir * 4 * 2 * 3;
                for (int vertex = 0; vertex < 4; vertex++) {

                    int vertexOffset = vertex * 2 * 3;
                    int vPosX = cubeVerticesPositions[verticesPerFace[dir * 4 + vertex] * 3 + 0];
                    int vPosY = cubeVerticesPositions[verticesPerFace[dir * 4 + vertex] * 3 + 1];
                    int vPosZ = cubeVerticesPositions[verticesPerFace[dir * 4 + vertex] * 3 + 2];

                    int nextCheck = vertex + 1;
                    if (nextCheck > 3)
                        nextCheck = 0;

                    int vNextPosX = cubeVerticesPositions[verticesPerFace[dir * 4 + nextCheck] * 3 + 0];
                    int vNextPosY = cubeVerticesPositions[verticesPerFace[dir * 4 + nextCheck] * 3 + 1];
                    int vNextPosZ = cubeVerticesPositions[verticesPerFace[dir * 4 + nextCheck] * 3 + 2];

                    nextCheck = nextCheck + 1;
                    if (nextCheck > 3)
                        nextCheck = 0;

                    int vBackPosX = cubeVerticesPositions[verticesPerFace[dir * 4 + nextCheck] * 3 + 0];
                    int vBackPosY = cubeVerticesPositions[verticesPerFace[dir * 4 + nextCheck] * 3 + 1];
                    int vBackPosZ = cubeVerticesPositions[verticesPerFace[dir * 4 + nextCheck] * 3 + 2];

                    int difX = 0;
                    if (vPosX != vNextPosX)
                        difX = vPosX - vNextPosX;
                    if(vPosX != vBackPosX)
                        difX = vPosX - vBackPosX;


                    int difY = 0;
                    if (vPosY != vNextPosY)
                        difY = vPosY - vNextPosY;
                    if (vPosY != vBackPosY)
                        difY = vPosY - vBackPosY;


                    int difZ = 0;
                    if (vPosZ != vNextPosZ)
                        difZ = vPosZ - vNextPosZ;
                    if (vPosZ != vBackPosZ)
                        difZ = vPosZ - vBackPosZ;

                    if(difX != 0) {

                        if(difY != 0) {

                            ambientOclusionCheckDirection[dirOffset + vertexOffset + 0 * 3 + 0] = difX;
                            ambientOclusionCheckDirection[dirOffset + vertexOffset + 0 * 3 + 1] = 0;
                            ambientOclusionCheckDirection[dirOffset + vertexOffset + 0 * 3 + 2] = 0;

                            ambientOclusionCheckDirection[dirOffset + vertexOffset + 1 * 3 + 0] = 0;
                            ambientOclusionCheckDirection[dirOffset + vertexOffset + 1 * 3 + 1] = difY;
                            ambientOclusionCheckDirection[dirOffset + vertexOffset + 1 * 3 + 2] = 0;

                        } else {

                            ambientOclusionCheckDirection[dirOffset + vertexOffset + 0 * 3 + 0] = difX;
                            ambientOclusionCheckDirection[dirOffset + vertexOffset + 0 * 3 + 1] = 0;
                            ambientOclusionCheckDirection[dirOffset + vertexOffset + 0 * 3 + 2] = 0;

                            ambientOclusionCheckDirection[dirOffset + vertexOffset + 1 * 3 + 0] = 0;
                            ambientOclusionCheckDirection[dirOffset + vertexOffset + 1 * 3 + 1] = 0;
                            ambientOclusionCheckDirection[dirOffset + vertexOffset + 1 * 3 + 2] = difZ;
                        }


                    } else {
                        ambientOclusionCheckDirection[dirOffset + vertexOffset + 0 * 3 + 0] = 0;
                        ambientOclusionCheckDirection[dirOffset + vertexOffset + 0 * 3 + 1] = difY;
                        ambientOclusionCheckDirection[dirOffset + vertexOffset + 0 * 3 + 2] = 0;


                        ambientOclusionCheckDirection[dirOffset + vertexOffset + 1 * 3 + 0] = 0;
                        ambientOclusionCheckDirection[dirOffset + vertexOffset + 1 * 3 + 1] = 0;
                        ambientOclusionCheckDirection[dirOffset + vertexOffset + 1 * 3 + 2] = difZ;


                    }




                }


            }

            for(int x = 0; x < ambientOclusionCheckDirection.Length; x++) {

                Console.Write(ambientOclusionCheckDirection[x] + ", ");
            }

            Console.WriteLine("s");

        }*/

    }
}
