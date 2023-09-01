using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Core {
    public interface IVoxelData {

        public uint voxelID { get; }

        public VoxelType voxelType { get; }
        

    }
}
