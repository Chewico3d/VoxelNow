using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.API.Tools {
    public class DensityPointDistribution {
        public readonly int samplesX, samplesY;

        readonly MapArray2D<(float, float)> Points;
        Random random = new Random();
        
        public DensityPointDistribution(int samplesX, int samplesY) {
            this.samplesX = samplesX;
            this.samplesY = samplesY;

            Points = new MapArray2D<(float, float)>(samplesX, samplesY);




        }

    }
}
