using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VoxelNow.Core;

namespace VoxelNow.API.API
{
    public interface IWorldGenerator
    {

        public MapArray3D<ushort> GenerateTerrain(int sizeX, int sizeY, int sizeZ, uint ID);

    }
}
