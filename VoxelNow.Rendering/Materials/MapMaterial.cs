
using VoxelNow.AssemblyLoader;
using VoxelNow.Rendering;

namespace VoxelNow.Rendering.Materials {
    internal class MapMaterial : Shader{

        public MapMaterial() : base(AssetLoader.GetAssetPath("Shaders/MapVertexShader.glsl"),
            AssetLoader.GetAssetPath("Shaders/MapPixelShader.glsl")) {

        }

    }
}
