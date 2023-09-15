
using VoxelNow.AssemblyLoader;

namespace VoxelNow.Rendering.Materials {
    internal class FluidMaterial : Shader {


        internal FluidMaterial()
            : base(AssetLoader.GetAssetPath("Shaders/FluidVertexShader.glsl"),
                  AssetLoader.GetAssetPath("Shaders/FluidPixelShader.glsl")) {

        }
    }
}
