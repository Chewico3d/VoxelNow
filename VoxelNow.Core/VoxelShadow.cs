using VoxelNow.API;
using VoxelNow.AssemblyLoader;

namespace VoxelNow.Core {
    internal static class VoxelShadow {

        static ChunkDatabase workingDatabase;
        static MapArray2D<int> shadowHeight;

        static Queue<(int, int, int)> voxelLightQueue = new Queue<(int, int, int)> ();

        public static void GenerateAllChunkShadows(ChunkDatabase chunkDatabase) {
            workingDatabase = chunkDatabase;

            shadowHeight = new MapArray2D<int>(chunkDatabase.voxelSizeX, chunkDatabase.voxelSizeZ);
            CalculateAndSetShadowHeight();

            voxelLightQueue.Clear();
            GenerateVoxelLightQueue();

            while(voxelLightQueue.Count > 0)
                PropagateVoxelLight();


        }
        static void CalculateAndSetShadowHeight() {

            for(int x = 0; x < workingDatabase.voxelSizeX; x++) {
                for(int z= 0; z < workingDatabase.voxelSizeZ; z++) {

                    for(int y = workingDatabase.voxelSizeY - 1; y >= 0; y--) {

                        ushort collidedBlock = workingDatabase.GetVoxel(x, y, z);

                        if (VoxelAssets.IsVoxel(collidedBlock)) {
                            shadowHeight.SetValue(x, z, y + 1);
                            break;
                        } else
                            workingDatabase.SetSunLight(x, y, z, 255);
                    }

                }
            }

        }
        static void GenerateVoxelLightQueue() {

            void ProcessVoxel(int x, int y, int z) {
                for(int it = 0; it < 8; it++) {

                    int nearX = x + ShadowVariables.shadowFlatCheckOrder[it * 2 + 0];
                    int nearZ = z + ShadowVariables.shadowFlatCheckOrder[it * 2 + 1];

                    byte nearLightValue = workingDatabase.GetSunLight(nearX, y, nearZ);

                    if (nearLightValue == 255)
                        continue;

                    ushort nearVoxelID = workingDatabase.GetVoxel(nearX, y, nearZ);
                    if (VoxelAssets.IsSolid(nearVoxelID))
                        continue;

                    workingDatabase.SetSunLight(nearX, y, nearZ, 255 - 50);
                    voxelLightQueue.Enqueue((nearX, y, nearZ));

                }
            }

            for (int x = 1; x < workingDatabase.voxelSizeX - 1; x++) {
                for (int z = 1; z < workingDatabase.voxelSizeZ - 1; z++)
                    for(int y = shadowHeight.GetValue(x,z); y < GetMaxShadowHeight(x,z); y++) {
                        ProcessVoxel(x, y, z);
                    }
            }
        }
        static void PropagateVoxelLight() {

            (int, int, int) voxelPosition = voxelLightQueue.Dequeue();
            byte currentLightValue = workingDatabase.GetSunLight(voxelPosition.Item1, voxelPosition.Item2, voxelPosition.Item3);

            if (currentLightValue < ShadowVariables.minShadowAffectance)
                return;

            ushort currentVoxelID = workingDatabase.GetVoxel(voxelPosition.Item1, voxelPosition.Item2, voxelPosition.Item3);

            for(int direction = 0; direction < 8 + 9 + 9; direction++) {

                int nearX = voxelPosition.Item1 + ShadowVariables.shadow3dCheckOrder[direction * 3 + 0];
                int nearY = voxelPosition.Item2 + ShadowVariables.shadow3dCheckOrder[direction * 3 + 1];
                int nearZ = voxelPosition.Item3 + ShadowVariables.shadow3dCheckOrder[direction * 3 + 2];

                byte nearLightValue = workingDatabase.GetSunLight(nearX, nearY, nearZ);

                //Shadow affectance is because of the pythagorean theorem
                int propagatedLightValue = currentLightValue - ShadowVariables.shadowAffectance[direction];
                propagatedLightValue -= VoxelAssets.GetLightResistence(currentVoxelID);

                if (nearLightValue >= propagatedLightValue)
                    continue;

                ushort nearVoxelID = workingDatabase.GetVoxel(nearX, nearY, nearZ);

                if (VoxelAssets.IsSolid(nearVoxelID))
                    continue;

                if (propagatedLightValue <= 0)
                    continue;

                workingDatabase.SetSunLight(nearX, nearY, nearZ, (byte)propagatedLightValue);
                if (propagatedLightValue < ShadowVariables.minShadowAffectance)
                    continue;

                voxelLightQueue.Enqueue((nearX, nearY, nearZ));
            }

        }

        static int GetMaxShadowHeight(int x, int z) {

            int GetMax(int a, int b) => (a > b) ? a : b;
            int max = int.MinValue;

            for (int rep = 0; rep < 8; rep++) {
                max = GetMax(max, shadowHeight.GetValue(x + ShadowVariables.shadowFlatCheckOrder[rep * 2 + 0]
                    , z + ShadowVariables.shadowFlatCheckOrder[rep * 2 + 1]));
            }

            return max;

        }

    }
}
