using System;
using OpenTK.Mathematics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNowEngine.Graphics.Materials;
using VoxelNowEngine.Terrain;

namespace VoxelNowEngine.Graphics {
    internal class RenderMaster {
        internal List<(RenderObject, Vector3i)> chunksRenders = new List<(RenderObject, Vector3i)> ();
        
        internal void RenderChunks(ChunkMaterial chunkMaterial, Camera camera) {
            for(int it = 0; it < chunksRenders.Count; it++) {
                chunkMaterial.Use();
                chunkMaterial.SetTransformationMatrix(camera, chunksRenders[it].Item2 * new Vector3(16, 256, 16));
                chunksRenders[it].Item1.Draw();
            }

        }

    }
}
