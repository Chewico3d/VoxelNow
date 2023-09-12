using VoxelNow.Rendering.Materials;
using VoxelNow.Rendering.Textures;
using VoxelNow.AssemblyLoader;

namespace VoxelNow.Rendering
{
    public class RenderScene {

        IRenderCollection[] renderCollections;
        IObjectFabric[] fabrics;
        IRenderObject[] renderObjects;

        public Camera mainRenderCamera = new Camera();
        FloatingMaterial floatingMaterial;
        SolidChunkMaterial solidChunkMaterial;
        MapMaterial mapMaterial;

        TiledTexture solidBlocksTexture;

        Queue<(uint, IFabricData)> objectsToBuild = new Queue<(uint, IFabricData)>();
        Queue<(uint, IMeshData)> objectsToLoad = new Queue<(uint, IMeshData)>();
        Thread buildThread;

        bool runningThread = true;

        
        public void Initialize() {
            AssemblyListLoader.ReadFromAssembly();

            renderCollections = AssemblyListLoader.collections;
            renderObjects = AssemblyListLoader.renderObjects;
            fabrics = AssemblyListLoader.fabrics;

            floatingMaterial = new FloatingMaterial();
            solidChunkMaterial = new SolidChunkMaterial();
            mapMaterial = new MapMaterial();

            solidBlocksTexture = new TiledTexture(AssetLoader.GetAssetPath("Textures/SolidBlocksTexture.png"), 20, 20);
            solidChunkMaterial.BindBaseTexture(solidBlocksTexture);

            buildThread = new Thread(FabricBuildLoop);
            buildThread.Start();

        }
        public void Render() {

            renderCollections[2].Draw(mainRenderCamera, mapMaterial);

            solidChunkMaterial.SetSun(1, 5, 3);
            renderCollections[1].Draw(mainRenderCamera, solidChunkMaterial);
            renderCollections[0].Draw(mainRenderCamera, floatingMaterial);

        }
        private int GetRenderObjectID(uint ID) {
            return (int)(ID >> 16);
        }
        public void SetObjectPosition(uint objID, float posX, float posY, float posZ) {

            uint collectionID = objID >> 16;
            uint localObjectID = 0b_00000000_00000000_11111111_11111111 & objID;

            renderCollections[collectionID].SetRenderObjectPosition((int)localObjectID, posX, posY, posZ);

        }
        private IRenderObject GetRenderObject(uint objID) {
            uint collectionID = objID >> 16;
            uint localObjectID = 0b_00000000_00000000_11111111_11111111 & objID;

            return renderCollections[collectionID].GetObject((int)localObjectID);

        }

        #region Render object construction chain
        public uint GenerateRenderObject(IFabricData fabricData) {

            IRenderObject renderObject = (IRenderObject)Activator.CreateInstance(renderObjects[fabricData.renderObjectID].GetType());
            ushort localObjectID = (ushort)renderCollections[fabricData.renderObjectID].AddObject(renderObject);
            ushort collectionID = (ushort)renderObject.renderObjectID;

            uint objectID = ((uint)collectionID << 16) + localObjectID;
            objectsToBuild.Enqueue((objectID, fabricData));

            return objectID;
        }
        public void UpdateRenderObject(IFabricData fabricData, uint objectID) {
            objectsToBuild.Enqueue((objectID, fabricData));
        }
        public void FabricBuildLoop() {
            while (runningThread) {
                FabricBuild();

            }

        }
        void FabricBuild() {
            if (objectsToBuild.Count == 0)
                return;

            (uint, IFabricData) fabricData = objectsToBuild.Dequeue();
            IObjectFabric objectFabric = fabrics[GetRenderObjectID(fabricData.Item1)];

            IMeshData generatedMeshData = objectFabric.GenerateMeshData(fabricData.Item2);
            objectsToLoad.Enqueue((fabricData.Item1, generatedMeshData));

        }
        
        public void LoadRenderObject() {
            if (objectsToLoad.Count == 0)
                return;

            (uint, IMeshData) meshData = objectsToLoad.Dequeue();

            IRenderObject workingRenderObject = GetRenderObject(meshData.Item1);
            workingRenderObject.LoadData(meshData.Item2);

        }

        #endregion End Render object construction chain

        public void EndThreads() {
            runningThread = false;
            buildThread.Join();
        }
    }
}
