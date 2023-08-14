using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNowEngine.Terrain {
    public class SolidNode {
        internal int[][] PositionUVNodes;
        public SolidNode(int[][] positionUVNodes) {
            PositionUVNodes = positionUVNodes;

        }
        public SolidNode(int[] monoUV) {
            PositionUVNodes = new int[6][];
            for(int it = 0; it < 6; it++) {
                PositionUVNodes[it] = new int[2];
                PositionUVNodes[it][0] = monoUV[0];
                PositionUVNodes[it][1] = monoUV[1];
            }

        }

        public SolidNode() {
            PositionUVNodes = new int[6][] {
                new int[2], new int[2], new int[2], new int[2], new int[2], new int[2] };

        }

        public void SetFaceUVCords(int direction, int posX, int posY) {
            PositionUVNodes[direction][0] = posX;
            PositionUVNodes[direction][1] = posY;
        }

    }
}
