using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.API;

namespace VoxelNow.Core {
    internal class WaterBehaviour {
        IChunkDatabase chunkDatabase;

        internal WaterBehaviour(IChunkDatabase chunkDatabase) {
            this.chunkDatabase = chunkDatabase;
        }



    }
}
