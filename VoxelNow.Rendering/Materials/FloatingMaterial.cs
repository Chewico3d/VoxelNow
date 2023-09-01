using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Rendering.VallVoxel.Client.Graphics;

namespace VoxelNow.Rendering.Materials {
    internal class FloatingMaterial : Shader{

        internal FloatingMaterial() : base("Shaders/FloatingVertexShader.glsl", "Shaders/FloatingPixelShader.glsl") {

        }
    }
}
