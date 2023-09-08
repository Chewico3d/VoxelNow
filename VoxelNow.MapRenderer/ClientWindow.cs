using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using VoxelNow.Core;
using VoxelNow.Rendering;
using VoxelNow.Rendering.FabricData;
using VoxelNow.Rendering.RenderObjects;

namespace VoxelNow.MapRenderer {
    internal class ClientWindow : GameWindow{
        internal ClientWindow() : base(new GameWindowSettings() { UpdateFrequency = 70}, new NativeWindowSettings() { Size = new OpenTK.Mathematics.Vector2i(1080, 1080)} ) { }
        internal FloatingRenderObject fRO = new FloatingRenderObject();

        RenderScene renderScene = new RenderScene();
        ChunkDatabase chunkDatabase;

        protected override void OnLoad() {

            AssetLoader.LoadAssemblyData();
            chunkDatabase = new ChunkDatabase(20, 10, 20);
            chunkDatabase.GenerateHeightMap();

            int sizeX = 32 * 20;
            int sizeY = 32 * 20;

            MapFabricData mapFabricData = new MapFabricData(sizeX, sizeY) ;

            
            for(int x = 0; x < sizeX; x++) {
                for(int y = 0; y < sizeY; y++) {
                    mapFabricData.SetColor(x, y, (byte)(chunkDatabase.terrainHeight.GetValue(x, y) ),
                        (byte)(chunkDatabase.terrainHeight.GetValue(x, y)),
                        (byte)(chunkDatabase.terrainHeight.GetValue(x, y)));

                }
            }

            renderScene.Initialize();

            renderScene.GenerateRenderObject(mapFabricData);
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            GL.Clear(ClearBufferMask.ColorBufferBit);


            renderScene.Render();
            renderScene.LoadRenderObject();
            SwapBuffers();
        }


    }
}
