using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Core {
    public struct TextureCoord {
        public byte posX = 0;
        public byte posY = 0;

        public TextureCoord(){}
        public TextureCoord(byte posX, byte posY) {
            this.posX = posX;
            this.posY = posY;
        }
    }
}
