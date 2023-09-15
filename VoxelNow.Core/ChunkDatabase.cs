using System.Runtime.CompilerServices;
using VoxelNow.API;
using VoxelNow.AssemblyLoader;

namespace VoxelNow.Core {
    public class ChunkDatabase : IChunkDatabase{

        public readonly int sizeX, sizeY, sizeZ;
        public int voxelSizeX { get { return sizeX * GenerationConstants.voxelSizeX; } }
        public int voxelSizeY { get { return sizeY * GenerationConstants.voxelSizeY; } }
        public int voxelSizeZ { get { return sizeZ * GenerationConstants.voxelSizeZ; } }

        public readonly Chunk[] chunks;
        public Queue<(int, int, int)> proceduralVoxel = new Queue<(int, int, int)> ();

        public ChunkDatabase(int sizeX, int sizeY, int sizeZ) {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;

            chunks = new Chunk[sizeX * sizeY * sizeZ];
            for (int z = 0; z < sizeZ; z++) {
                for (int y = 0; y < sizeY; y++)
                    for (int x = 0; x < sizeX; x++) { 
                        chunks[GetChunkID(x,y,z)] = new Chunk(x, y, z);
                    }
            }
        }

        public void GenerateTerrain() {

            AssetLoader.worldGenerator.GenerateTerrain(this, 0);
            while(proceduralVoxel.Count != 0) {
                (int,int,int) proceduralVoxelPos = proceduralVoxel.Dequeue();
                ushort voxelID = GetVoxel(proceduralVoxelPos.Item1, proceduralVoxelPos.Item2, proceduralVoxelPos.Item3);
                if (!VoxelAssets.IsProcedural(voxelID))
                    continue;

                ushort proceduralID = VoxelAssets.GetProceduralID(voxelID);
                AssetLoader.proceduralVoxels[proceduralID].GenerateAt(this, proceduralVoxelPos.Item1, proceduralVoxelPos.Item2, proceduralVoxelPos.Item3);
            }

            VoxelShadow.GenerateAllChunkShadows(this);

        }

        //Complete means that the chunk have borders : 32 + 3, 32 + 2, 32 + 2
        public ushort[] GetCompleteChunkVoxelsData(int chunkID) {

            int completeVoxelsAmount = (GenerationConstants.voxelSizeX + 2)
                * (GenerationConstants.voxelSizeY + 2) * (GenerationConstants.voxelSizeZ + 2);
            ushort[] data = new ushort[completeVoxelsAmount];

            for (int z = 0; z < GenerationConstants.voxelSizeZ + 2; z++) {
                for (int y = 0; y < GenerationConstants.voxelSizeY + 2; y++) {
                    for (int x = 0; x < GenerationConstants.voxelSizeX + 2; x++) {

                        int voxelPositionX = x - 1 + chunks[chunkID].IDx * GenerationConstants.voxelSizeX;
                        int voxelPositionY = y - 1 + chunks[chunkID].IDy * GenerationConstants.voxelSizeY;
                        int voxelPositionZ = z - 1 + chunks[chunkID].IDz * GenerationConstants.voxelSizeZ;

                        if (voxelPositionX < 0 || voxelPositionY < 0 || voxelPositionZ < 0) continue;
                        if (voxelPositionX >= sizeX * GenerationConstants.voxelSizeX) continue;
                        if (voxelPositionY >= sizeY * GenerationConstants.voxelSizeY) continue;
                        if (voxelPositionZ >= sizeZ * GenerationConstants.voxelSizeZ) continue;

                        int ID = x + y * (GenerationConstants.voxelSizeX + 2)
                            + z * (GenerationConstants.voxelSizeX + 2) * (GenerationConstants.voxelSizeY + 2);

                        data[ID] = GetVoxel(voxelPositionX, voxelPositionY, voxelPositionZ);

                    }
                }
            }

            return data;

        }

        public void SetVoxel(int x, int y, int z, ushort value) {
            if (x < 0 | y < 0 | z < 0)
                return;
            if (x >= voxelSizeX | y >= voxelSizeY | z >= voxelSizeZ)
                return;

            (int, int) voxelPos = GetVoxelCordinates(x, y, z);
            if (VoxelAssets.IsProcedural(value))
                proceduralVoxel.Enqueue((x, y, z));

            chunks[voxelPos.Item1].voxels[voxelPos.Item2] = value;
        }
        public ushort GetVoxel(int x, int y, int z) {
            if (x < 0 | z < 0 | x >= voxelSizeX | y >= voxelSizeY | z >= voxelSizeZ)
                return 0;
            if (y <= 0)
                return 1;

            (int, int) voxelPos = GetVoxelCordinates(x, y, z);
            return chunks[voxelPos.Item1].voxels[voxelPos.Item2];
        }

        public byte GetSunLight(int x, int y, int z) {
            if (x < 0 | y < 0 | z < 0 | x >= voxelSizeX | y >= voxelSizeY | z >= voxelSizeZ)
                return 255;

            (int, int) voxelPos = GetVoxelCordinates(x, y, z);
            return chunks[voxelPos.Item1].sunLight[voxelPos.Item2];

        }
        public void SetSunLight(int x, int y, int z, byte value) {
            if (x < 0 | y <= 0 | z < 0 | x >= voxelSizeX | y >= voxelSizeY | z >= voxelSizeZ)
                return;

            (int, int) voxelPos = GetVoxelCordinates(x, y, z);
            chunks[voxelPos.Item1].sunLight[voxelPos.Item2] = value;

        }

        public byte GetWaterValue(int x, int y, int z) {
            if (x < 0 | y < 0 | z < 0 | x >= voxelSizeX | y >= voxelSizeY | z >= voxelSizeZ)
                return 127;

            (int, int) voxelPos = GetVoxelCordinates(x, y, z);
            byte fluid = chunks[voxelPos.Item1].fluid[voxelPos.Item2];

            if (fluid >= 128)
                return 0;

            return fluid;
        }
        public void SetWaterValue(int x, int y, int z, byte value) {

            if (x < 0 | y < 0 | z < 0 | x >= voxelSizeX | y >= voxelSizeY | z >= voxelSizeZ)
                return;
            value = value >= 128 ? (byte)127 : value;

            (int, int) voxelPos = GetVoxelCordinates(x, y, z);
            chunks[voxelPos.Item1].fluid[voxelPos.Item2] = value;

        }

        (int, int) GetVoxelCordinates(int x, int y, int z) {


            int chunkX = (int)MathF.Floor((float)x / (float)GenerationConstants.voxelSizeX);
            int chunkY = (int)MathF.Floor((float)y / (float)GenerationConstants.voxelSizeY);
            int chunkZ = (int)MathF.Floor((float)z / (float)GenerationConstants.voxelSizeZ);

            int relativeX = x - chunkX * GenerationConstants.voxelSizeX;
            int relativeY = y - chunkY * GenerationConstants.voxelSizeY;
            int relativeZ = z - chunkZ * GenerationConstants.voxelSizeZ;

            int chunkID = chunkX + chunkY * sizeX + chunkZ * sizeX * sizeY;
            int voxelID = relativeX + relativeY * GenerationConstants.voxelSizeX
                + relativeZ * GenerationConstants.voxelSizeX * GenerationConstants.voxelSizeY;

            if (voxelID < 0)
                return (chunkID, voxelID);

            return (chunkID, voxelID);

        }
        int GetChunkID(int x, int y, int z) {
            return x + y * sizeX + z * sizeX * sizeY;
        }


    }
}
