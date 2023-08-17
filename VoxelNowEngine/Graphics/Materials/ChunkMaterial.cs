using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNowEngine.Graphics.Textures;

namespace VoxelNowEngine.Graphics.Materials {
    public class ChunkMaterial : Shader{

        public ChunkMaterial() : base(@"Graphics\Shaders\ChunkVertexShader.glsl", @"Graphics\Shaders\ChunkPixelShader.glsl") {

        }

        public void SetMainTexture(TiledTexture texture) {
            SetUniform1("tiledTextureSizeX", texture.xSquares);
            SetUniform1("tiledTextureSizeY", texture.ySquares);



        }
    }
}
