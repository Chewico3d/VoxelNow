
using VoxelNow.AssemblyLoader;

namespace VoxelNow.Rendering.Materials {
    internal class FloatingMaterial : Shader{

        internal FloatingMaterial() 
            : base(AssetLoader.GetAssetPath("Shaders/FloatingVertexShader.glsl"),
                  AssetLoader.GetAssetPath("Shaders/FloatingPixelShader.glsl")) {



        }
    }
}
