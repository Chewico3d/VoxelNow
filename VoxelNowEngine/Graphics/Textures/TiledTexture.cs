using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNowEngine.Graphics.Textures {
    public class TiledTexture : Texture{

        public readonly int xSquares, ySquares;

        public TiledTexture(string TexturePath, int xSquares, int ySquares) : base (TexturePath) {
            this.ySquares = ySquares;
            this.xSquares = xSquares;
        }

    }
}
