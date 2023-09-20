using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.API;
using VoxelNow.AssemblyLoader;

namespace VoxelNow.Core {
    internal class WaterBehaviour {
        IChunkDatabase chunkDatabase;

        Queue<(int, int, int)> waterPropagation = new Queue<(int, int, int)> ();

        internal WaterBehaviour(IChunkDatabase chunkDatabase) {
            this.chunkDatabase = chunkDatabase;
        }

        public void AddWaterPropagation(int x, int y, int z) {
            waterPropagation.Enqueue((x, y, z));
        }

        public void Propagate() {
            if (waterPropagation.Count == 0)
                return;

            //we want to only itirenate in the water that we currently have, if we put more they will be done in the next propagation
            int waterUpdateCount = waterPropagation.Count;
            for (int itirenation = 0; itirenation < waterUpdateCount; itirenation++)
                UpdateWaterVoxel();
        }

        void UpdateWaterVoxel() {

            (int, int, int) voxelPosition = waterPropagation.Dequeue();
            byte waterValue = chunkDatabase.GetWaterValue(voxelPosition.Item1, voxelPosition.Item2, voxelPosition.Item3);

            if (waterValue == 0)
                return;

            ushort downVoxelID = chunkDatabase.GetVoxel(voxelPosition.Item1, voxelPosition.Item2 - 1, voxelPosition.Item3);
            if (VoxelAssets.CanWaterPass(downVoxelID)) {
                byte downWaterValue = chunkDatabase.GetWaterValue(voxelPosition.Item1, voxelPosition.Item2 - 1, voxelPosition.Item3);
                int nextDownWaterValue = waterValue += 20;
                nextDownWaterValue = nextDownWaterValue > 120 ? 120 : nextDownWaterValue;
                if (downWaterValue < nextDownWaterValue) {
                    chunkDatabase.SetWaterValue(voxelPosition.Item1, voxelPosition.Item2 - 1, voxelPosition.Item3, (byte)nextDownWaterValue);
                    waterPropagation.Enqueue((voxelPosition.Item1, voxelPosition.Item2 - 1, voxelPosition.Item3));
                }
            }else
            for(int direction = 0; direction < 8; direction++) {
                int checkX = voxelPosition.Item1 + voxelDirections[direction * 2 + 0];
                int checkY = voxelPosition.Item2;
                int checkZ = voxelPosition.Item3 + voxelDirections[direction * 2 + 1];

                ushort checkVoxelID = chunkDatabase.GetVoxel(checkX, checkY, checkZ);
                if (VoxelAssets.CanWaterPass(checkVoxelID)) {
                    byte checkVoxelWaterValue = chunkDatabase.GetWaterValue(checkX, checkY, checkZ);
                    int resultingWaterValue = waterValue - voxelDirectionResistence[direction];

                    if(resultingWaterValue > 0 && checkVoxelWaterValue < resultingWaterValue) {
                        chunkDatabase.SetWaterValue(checkX, checkY, checkZ, (byte)resultingWaterValue);
                        waterPropagation.Enqueue((checkX, checkY, checkZ));

                    }
                }

            }

        }


        static int[] voxelDirections = {
            0, 1,
            1, 1,
            1, 0,
            1,-1,
            0,-1,
           -1,-1,
           -1, 0,
           -1, 1
        };

        static int[] voxelDirectionResistence = {
            10,
            14,
            10,
            14,
            10,
            14,
            10,
            14
        };

    }
}
