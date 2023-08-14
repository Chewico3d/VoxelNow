using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNowEngine.Graphics;
using VoxelNowEngine.Graphics.Objects;
using VoxelNowEngine.Terrain;

namespace VoxelNowEngine {
    internal class WorldManager {

        Dictionary<Vector3i, Chunk> LoadedChunks;
        Queue<(SolidChunkRenderObject.Data, Vector3i ChunkID)> ChunkQueue;

        internal WorldManager() {

            if (Game.currentWorld.properties.GenerateChunks) {
                LoadedChunks = new Dictionary<Vector3i, Chunk>();
                ChunkQueue = new Queue<(SolidChunkRenderObject.Data, Vector3i ChunkID)>();
            } else {
                LoadedChunks = null;
                ChunkQueue = null;
            }

        }

        internal void AddChunk(int xPos, int yPos, int zPos, Chunk chunk) {

            LoadedChunks.Add(new Vector3i(xPos, yPos, zPos), chunk);

            if (chunk == null)
                return;

            SolidChunkRenderObject.Data chunkRender = ChunkMeshCreator.CreateSolidChunkRenderObject(chunk);
            ChunkQueue.Enqueue((chunkRender,new Vector3i(xPos, yPos, zPos)));
        }

        internal bool ExistsChunk(int xPos, int yPos, int zPos) => LoadedChunks.ContainsKey(new Vector3i(xPos, yPos, zPos));

        public void StartThread() {

            while (true) {

                CreateNonExistingChunks();

                Thread.Sleep(1000 / 50);

            }
        }

        //Since OpenGL can not make things in another thread we will load them here
        internal void LoadDataInMainThread() {

            if (ChunkQueue.Count == 0)
                return;

            (SolidChunkRenderObject.Data, Vector3i ID) currentData = ChunkQueue.Dequeue();
            SolidChunkRenderObject currentLoadedChunk = new SolidChunkRenderObject(currentData.Item1);
            Game.currentRenderMaster.chunksRenders.Add((currentLoadedChunk, currentData.ID));

        }

        void CreateNonExistingChunks() {

            if (!Game.currentWorld.properties.GenerateChunks)
                return;

            Vector3 playerPosition = Game.currentWorld.mainRenderCamera.position / new Vector3(16, 256, 16);
            Vector3i playerIdPosition = new Vector3i((int)MathF.Floor(playerPosition.X + .5f), (int)MathF.Floor(playerPosition.Y + .5f), (int)MathF.Floor(playerPosition.Z + .5f));

            for (int x = 0; x < Game.currentWorld.properties.ChunkDistance; x++) {
                for (int z = 0; z < Game.currentWorld.properties.ChunkDistance; z++) {
                    int startDistance = Game.currentWorld.properties.ChunkDistance / 2;
                    Vector3i CalculatingChunk = playerIdPosition + new Vector3i(x - startDistance, 0, z - startDistance);

                    if (ExistsChunk(CalculatingChunk.X, 0, CalculatingChunk.Z))
                        continue;

                    Chunk calculatedNewChunk = Game.currentWorld.GenerateChunk(CalculatingChunk.X, 0, CalculatingChunk.Z);

                    AddChunk(CalculatingChunk.X, 0, CalculatingChunk.Z, calculatedNewChunk);
                    return;

                }
            }
        }

    }
}
