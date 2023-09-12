using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Core;

namespace VoxelNow.API.API
{
    public interface IProceduralVoxel
    {
        public ushort ID { get; }

        public void GenerateAt(int x, int y, int z, ChunkDatabase chunkDatabase);

    }
}
