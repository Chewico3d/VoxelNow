using OpenTK.Mathematics;
using VoxelNowEngine;
using VoxelNowEngine.Graphics;
using VoxelNowEngine.Graphics.Materials;
using VoxelNowEngine.Graphics.Textures;
using VoxelNowEngine.Terrain;
using VoxelNowGame;
using SimplexNoise;

namespace VoxelNowEngine {
    internal class ExplorationWorld : World {

        public Camera mainRenderCamera { get; set; } = new Camera();
        public ChunkMaterial mainChunkMaterial { get; set; }
        public TiledTexture mainChunkTexture { get; set; }
        public WorldProperties properties { get; set; } = new WorldProperties() { ChunkDistance = 16 };

        Chunk SimpleCube;
        Player playerScript = new Player();

        public Chunk GenerateChunk(int xChunk, int yChunk, int zChunk) {
            SimpleCube = new Chunk(xChunk, yChunk, zChunk);
            for (int x = -1; x < 17; x++) {
                for (int z = -1; z < 17; z++) {
                    float NoiseValue = Noise.CalcPixel2D(x + xChunk * 16, z + zChunk * 16, .01f);
                    for (int y = 0; y < 256; y++) {

                        if (y < NoiseValue * .3f)
                            SimpleCube.SetBlock(x, y, z, 1);

                        if (y + 1 > NoiseValue * .3f && y < NoiseValue * .3f)
                            SimpleCube.SetBlock(x, y, z, 2);


                    }
                }
            }
            return SimpleCube;
        }

        public void InitWorld() {

            playerScript.SetInitialPos();
            mainChunkTexture = new TiledTexture(@"C:\Chewico\Projects\VoxelNow\VoxelNowGame\maintext.png",2,2);

            SolidNode mainBlock = new SolidNode(new int[2] { 0, 0 });
            SolidNode mainBlock2 = new SolidNode(new int[2] { 0, 1 });

            mainBlock2.SetFaceUVCords(3, 1 , 1);
            NodesDatabase.SetSolidNode(1, mainBlock);
            NodesDatabase.SetSolidNode(2, mainBlock2);

            mainChunkMaterial = new ChunkMaterial();
            mainChunkMaterial.SetMainTexture(mainChunkTexture);



        }

        void World.Render(float deltaTime) {

        }

        void World.Update(float deltaTime) {

            ((ScriptBehaviour)playerScript).Update(deltaTime);
            mainRenderCamera.position = playerScript.playerPosition;
            mainRenderCamera.rotation = playerScript.getCameraOrientation();
            mainRenderCamera.FOV = playerScript.FOVadd;
        }

    }
}
