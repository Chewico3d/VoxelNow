using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Rendering;

namespace VoxelNow.Server {
    public interface IRenderObject {
        public ushort renderObjectID { get; }

        public void LoadData(IMeshData meshData);
        public void Draw();

    }
}
