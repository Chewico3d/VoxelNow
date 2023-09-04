using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Core;
using VoxelNow.Rendering.Textures;
using VoxelNow.Rendering.VallVoxel.Client.Graphics;

namespace VoxelNow.Rendering.Materials {
    internal class SolidChunkMaterial : Shader {
        internal SolidChunkMaterial() : base(
            AssetLoader.GetAssetPath("Shaders/SolidChunkVertexShader.glsl"),
            AssetLoader.GetAssetPath("Shaders/SolidChunkPixelShader.glsl")) {

            
        }

        internal void BindBaseTexture(TiledTexture tiledTexture) {
            SetUniform1("textureSizeX", tiledTexture.sizeX);
            SetUniform1("textureSizeY", tiledTexture.sizeY);

            tiledTexture.Use();

        }

        internal void SetSun(float xDir, float yDir, float zDir) {

            float magnitude = MathF.Sqrt(xDir * xDir + yDir * yDir + zDir * zDir);
            float mult = 1 / magnitude;
            SetUniform3f("sunDir", xDir * mult, yDir * mult, zDir * mult);

        }
    }
}
