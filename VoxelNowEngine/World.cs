using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNowEngine.Graphics;
using VoxelNowEngine.Graphics.Materials;
using VoxelNowEngine.Graphics.Textures;
using VoxelNowEngine.Terrain;

namespace VoxelNowEngine {
    public interface World {
        public WorldProperties properties { get; }

        public Camera mainRenderCamera { get; set; }
        public ChunkMaterial mainChunkMaterial { get; set; }
        public TiledTexture mainChunkTexture { get; set; }

        public void InitWorld();
        public void Render(float deltaTime);
        public void Update(float deltaTime);

        public Chunk GenerateChunk(int xChunk, int yChunk, int zChunk);

    }
}
