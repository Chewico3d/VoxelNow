using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNowEngine.Graphics.Materials {
    public class LitMaterial : Shader {


        public LitMaterial() : base(@"Graphics\Shaders\LitVertexShader.glsl", @"Graphics\Shaders\LitPixelShader.glsl") {

        }

        

    }
}
