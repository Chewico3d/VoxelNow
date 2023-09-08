using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.API {
    public interface IWorldGenerator {

        public MapArray2D<int> GenerateHeightMap(int voxelSizeX, int voxelSizeY);
        public void SetSeed(int seedNumber);

    }
}
