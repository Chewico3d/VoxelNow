using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNowEngine.Terrain {
    public class Chunk {
        internal byte[] solidBlocks = new byte[18 * 18 * 258];
        public readonly int xID, yID, zID;

        public Chunk(int x, int y, int z) {
            xID = x; yID = y; zID = z;
        }

        public byte GetBlock(int x, int y, int z) {
            x++; y++; z++;
            return solidBlocks[x + y * 18 + z * 258 * 18];
        }
        public void SetBlock(int x, int y, int z, byte value) {
            x++;y++;z++;

            solidBlocks[x + y * 18 + z * 258 * 18] = value;
        }

    }
}
