using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Networking {
    internal interface IPacket {
        internal uint PacketId { get; }

        internal void Read(BinaryReader reader);
        internal void Write(BinaryWriter writer);

    }
}
