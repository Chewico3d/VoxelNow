using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNowEngine.Graphics;
using VoxelNowEngine.Graphics.Objects;

namespace VoxelNowEngine.Terrain {
    public class ChunkWorld {
        internal static ChunkWorld instance;

        internal Dictionary<Vector3i, Chunk> LoadedChunks;
        internal Dictionary<Vector3i, RenderObject> LoadedRenderChunks;
        
        internal Queue<(SolidChunkRenderObject.Data, Vector3i ChunkID)> AddChunkMeshData;
        internal Queue<(SolidChunkRenderObject.Data, RenderObject)> UpdateChunkMeshData;
        
        internal List<(Chunk, RenderObject)> UpdateChunk;

        public ChunkWorld() {
            instance = this;

            if (Game.currentWorld.properties.GenerateChunks) {
                LoadedChunks = new Dictionary<Vector3i, Chunk>();
                AddChunkMeshData = new Queue<(SolidChunkRenderObject.Data, Vector3i ChunkID)>();
                UpdateChunkMeshData = new Queue<(SolidChunkRenderObject.Data, RenderObject)>();
                LoadedRenderChunks = new Dictionary<Vector3i, RenderObject> ();
                UpdateChunk = new List<(Chunk, RenderObject)> ();
            } else {
                LoadedChunks = null;
                AddChunkMeshData = null;
                LoadedRenderChunks = null;
                UpdateChunk = null;
                UpdateChunkMeshData = null;
            }


        }

        //Since OpenGL can not make things in another thread we will load them here
        internal void LoadDataInMainThread() {


            Console.WriteLine(UpdateChunkMeshData.Count + " q");
            if (UpdateChunkMeshData.Count != 0)
                {

                (SolidChunkRenderObject.Data, RenderObject) workingMeshData = UpdateChunkMeshData.Dequeue();
                ((SolidChunkRenderObject)workingMeshData.Item2).UpdateData(workingMeshData.Item1);
                return;
            }

            if (AddChunkMeshData.Count == 0)
                return;
            (SolidChunkRenderObject.Data, Vector3i ID) currentData = AddChunkMeshData.Dequeue();
            SolidChunkRenderObject currentLoadedChunk = new SolidChunkRenderObject(currentData.Item1);
            Game.currentRenderMaster.chunksRenders.Add((currentLoadedChunk, currentData.ID));

            LoadedRenderChunks.Add(currentData.ID, currentLoadedChunk);

        }
        
        public static void ModifyBlock(int x, int y, int z, byte newBlock) {
            Vector3i chunkID = new Vector3i((int)MathF.Floor((float)x / 16f),
                (int)MathF.Floor((float)y / 256f), (int)MathF.Floor((float)z / 16f));

            if (!ExistsChunk(chunkID.X, chunkID.Y, chunkID.Z))
                return;

            Chunk workingChunk = GetChunk(chunkID.X, chunkID.Y, chunkID.Z);

            //Check if it colides
            Vector3i relativeBlockID = new Vector3i(x,y,z) - chunkID * new Vector3i(16, 256, 16);

            Game.chunkWorld.ModifyAndUpdateChunk(chunkID, relativeBlockID, newBlock);
            if (relativeBlockID.X == 0) {
                Game.chunkWorld.ModifyAndUpdateChunk(chunkID - new Vector3i(1, 0, 0), relativeBlockID + new Vector3i(16, 0, 0), newBlock);
            } else if(relativeBlockID.X == 15) {
                Game.chunkWorld.ModifyAndUpdateChunk(chunkID + new Vector3i(1, 0, 0), relativeBlockID - new Vector3i(16, 0, 0), newBlock);
            }

            if (relativeBlockID.Y == 0) {
                Game.chunkWorld.ModifyAndUpdateChunk(chunkID - new Vector3i(0, 1, 0), relativeBlockID + new Vector3i(0, 256, 0), newBlock);
            } else if (relativeBlockID.Y == 255) {
                Game.chunkWorld.ModifyAndUpdateChunk(chunkID + new Vector3i(0, 1, 0), relativeBlockID - new Vector3i(0, 256, 0), newBlock);
            }

            if (relativeBlockID.Z == 0) {
                Game.chunkWorld.ModifyAndUpdateChunk(chunkID - new Vector3i(0, 0, 1), relativeBlockID + new Vector3i(0, 0, 16), newBlock);
            } else if (relativeBlockID.Z == 15) {
                Game.chunkWorld.ModifyAndUpdateChunk(chunkID + new Vector3i(0, 0, 1), relativeBlockID - new Vector3i(0, 0, 16), newBlock);
            }


        }

        internal void ModifyAndUpdateChunk(Vector3i chunkID, Vector3i relativePosition, byte value) {

            if (!ExistsChunk(chunkID.X, chunkID.Y, chunkID.Z))
                return;

            Chunk workingChunk = GetChunk(chunkID.X, chunkID.Y, chunkID.Z);
            workingChunk.SetBlock(relativePosition.X, relativePosition.Y, relativePosition.Z, value);

            RenderObject workingChunkRenderObject;
            if (LoadedRenderChunks.TryGetValue(chunkID, out workingChunkRenderObject)) {
                Game.chunkWorld.UpdateChunk.Add((workingChunk, workingChunkRenderObject));

            }

        }

        public static bool ExistsChunk(int xPos, int yPos, int zPos) => Game.chunkWorld.LoadedChunks.ContainsKey(new Vector3i(xPos, yPos, zPos));
        public static Chunk GetChunk(int xPos, int yPos, int zPos) {
            Chunk chunk;
            Game.chunkWorld.LoadedChunks.TryGetValue(new Vector3i(xPos, yPos, zPos), out chunk);
            return chunk;
        }

        internal void CreateNonExistingChunks() {

            if (!Game.currentWorld.properties.GenerateChunks)
                return;

            if(UpdateChunk.Count == 0) {
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

                return;
            }

            (Chunk, RenderObject) updatingChunk = UpdateChunk[0];

            for(int it = UpdateChunk.Count - 1; it >= 0; it--) {
                if (UpdateChunk[it] == updatingChunk)
                    UpdateChunk.RemoveAt(it);
            }

            SolidChunkRenderObject.Data chunkRenderMesh = ChunkMeshCreator.CreateSolidChunkRenderObject(updatingChunk.Item1);
            Vector3i chunkID = new Vector3i(updatingChunk.Item1.xID, updatingChunk.Item1.yID, updatingChunk.Item1.zID);

            UpdateChunkMeshData.Enqueue((chunkRenderMesh, updatingChunk.Item2));

        }

        internal void AddChunk(int xPos, int yPos, int zPos, Chunk chunk) {

            LoadedChunks.Add(new Vector3i(xPos, yPos, zPos), chunk);

            if (chunk == null)
                return;

            SolidChunkRenderObject.Data chunkRender = ChunkMeshCreator.CreateSolidChunkRenderObject(chunk);
            AddChunkMeshData.Enqueue((chunkRender, new Vector3i(xPos, yPos, zPos)));
        }


    }
}
