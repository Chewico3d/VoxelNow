using VoxelNow.API;

namespace VoxelNow.Assets.ProceduralVoxel {
    public class SimpleTree : IProceduralVoxel {
        public ushort ID { get { return 0x00; } }

        ushort[] treeStructure = {

            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 5, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,

            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 5, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,

            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 5, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,

            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 5, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,

            6, 6, 6, 6, 6,
            6, 6, 6, 6, 6,
            6, 6, 5, 6, 6,
            6, 6, 6, 6, 6,
            6, 6, 6, 6, 6,

            0, 6, 6, 6, 0,
            6, 6, 6, 6, 6,
            6, 6, 5, 6, 6,
            6, 6, 6, 6, 6,
            0, 6, 6, 6, 0,

            0, 0, 0, 0, 0,
            0, 6, 6, 6, 0,
            0, 6, 5, 6, 0,
            0, 6, 6, 6, 0,
            0, 0, 0,0 , 0,

            0, 0, 0, 0, 0,
            0, 0, 6, 0, 0,
            0, 6, 6, 6, 0,
            0, 0, 6, 0, 0,
            0, 0, 0,0 , 0
        };

        public void GenerateAt(IChunkDatabase chunkDatabase, int x, int y, int z) {

            int ID = 0;
            for(int itY = 0; itY < 8; itY++) {
                for(int itZ = 0; itZ < 5; itZ++) {
                    for(int itX = 0; itX < 5; itX++) {
                        if (treeStructure[ID] != 0)
                            chunkDatabase.SetVoxel(x + itX - 2, y + itY, z + itZ - 2, treeStructure[ID]);
                        ID++;
                    }
                }

            }


        }
    }
}
