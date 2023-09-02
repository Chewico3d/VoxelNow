using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Rendering.Textures;
using VoxelNow.Rendering.VallVoxel.Client.Graphics;

namespace VoxelNow.Rendering.Materials {
    internal class SolidChunkMaterial : Shader {
        internal SolidChunkMaterial() : base("AssetPack/Shaders/SolidChunkVertexShader.glsl", "AssetPack/Shaders/SolidChunkPixelShader.glsl") {

            

        }

        internal void BindBaseTexture(TiledTexture tiledTexture) {
            SetUniform1("textureSizeX", tiledTexture.sizeX);
            SetUniform1("textureSizeY", tiledTexture.sizeY);

            tiledTexture.Use();

        }
    }
}
