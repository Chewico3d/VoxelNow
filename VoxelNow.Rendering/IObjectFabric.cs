using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Rendering {
    internal interface IObjectFabric {

        public ushort renderObjectID { get; }
        public IMeshData GenerateMeshData(IFabricData fabricData);
    }
}
