using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Core {
    internal static class ShadowVariables {

        internal static readonly int[] shadowFlatCheckOrder = {
            -1,-1,
             0,-1,
             1,-1,
             1, 0,
             1, 1,
             0, 1,
            -1, 1,
            -1, 0
        };

        internal static readonly int[] shadow3dCheckOrder = {
            
            -1, 1, 1,
            0, 1, 1,
            1, 1, 1,
            -1, 1, 0,
            0, 1, 0,
            1, 1, 0,
            -1, 1, -1,
            0, 1, -1,
            1, 1, -1,

            -1, 0, 1,
            0, 0, 1,
            1, 0, 1,
            -1, 0, 0,
            1, 0, 0,
            -1, 0, -1,
            0, 0, -1,
            1, 0, -1,

            -1, -1, 1,
            0, -1, 1,
            1, -1, 1,
            -1, -1, 0,
            0, -1, 0,
            1, -1, 0,
            -1, -1, -1,
            0, -1, -1,
            1, -1, -1,

        };
        internal static readonly int[] shadowAffectance = {
            24, 19, 24, 28, 14, 28, 24, 28, 24,
            28, 14, 28, 14, 14, 28, 14, 28,
            24, 28, 24, 28, 14, 28, 24, 28, 24,
        };

        internal const int minShadowAffectance = 14;

    }
}
