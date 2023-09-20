
namespace VoxelNow.Rendering {
    internal static class FluidVoxelData {

        internal static int[] sidesCheck = {
            -1,0,
            1, 0,
            0,-1,
            0, 1
        };

        internal static int[][] sidesBaseVertex = {
            new int[] {
                0,1,
                0,0 },
            new int[] {
                1,0,
                1,1 },
            new int[] {
                0,0,
                1,0 },
            new int[] {
                1,1,
                0,1 }

        };

    }
}
