using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Core;
using VoxelNow.Rendering.VallVoxel.Client.Graphics;

namespace VoxelNow.Rendering.Materials {
    internal class MapMaterial : Shader{

        public MapMaterial() : base(AssetLoader.GetAssetPath("Shaders/MapVertexShader.glsl"),
            AssetLoader.GetAssetPath("Shaders/MapPixelShader.glsl")) {

        }

    }
}
