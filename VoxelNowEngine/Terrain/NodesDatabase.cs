using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNowEngine.Terrain {
    public static class NodesDatabase {

        public static SolidNode[] solidNodes { get; internal set; }

        static NodesDatabase() {
            solidNodes = new SolidNode[255];
        }

        public static void SetSolidNode(int nodeID, SolidNode solidNode) => solidNodes[nodeID - 1] = solidNode;

    }
}
