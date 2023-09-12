using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.ComponentModel;
using VoxelNow.Core;
using VoxelNow.Rendering;
using VoxelNow.Rendering.FabricData;
using VoxelNow.Rendering.RenderObjects;
using VoxelNow.AssemblyLoader;

namespace VoxelNow.Client {
    internal class ClientWindow : GameWindow{
        internal ClientWindow() : base(new GameWindowSettings() { UpdateFrequency = 70}, new NativeWindowSettings() { Size = new OpenTK.Mathematics.Vector2i(1920, 1080) } ) { }
        internal FloatingRenderObject fRO = new FloatingRenderObject();

        RenderScene renderScene = new RenderScene();
        ChunkDatabase chunkDatabase;
        playerScript playerScript = new playerScript();

        protected override void OnLoad() {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.ClearColor(0.2f, 0.4f, 0.6f, 1f);

            AssetLoader.LoadAssemblyData();
            chunkDatabase = new ChunkDatabase(30, 12, 30);

            GL.Enable(EnableCap.DepthTest);
            renderScene.Initialize();

            chunkDatabase.GenerateTerrain();

            for (int x = 0; x < chunkDatabase.chunks.Length; x++) {

                Chunk workingChunk = chunkDatabase.chunks[x];

                SolidCunkFabricData solidCunkFabricData = new SolidCunkFabricData(chunkDatabase, workingChunk.IDx, workingChunk.IDy, workingChunk.IDz);

                uint ID = renderScene.GenerateRenderObject(solidCunkFabricData);
                renderScene.SetObjectPosition(ID,
                    chunkDatabase.chunks[x].IDx * 32, chunkDatabase.chunks[x].IDy * 32, chunkDatabase.chunks[x].IDz * 32);

            }

            Program.nativeWindow = this;
            Program.nativeWindow.MousePosition = new OpenTK.Mathematics.Vector2(1280 / 2, 720 / 2);
        }
        protected override void OnRenderFrame(FrameEventArgs args) {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            renderScene.Render();
            SwapBuffers();

            renderScene.LoadRenderObject();
            renderScene.LoadRenderObject();
            renderScene.LoadRenderObject();
            renderScene.LoadRenderObject();
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {

            playerScript.Update();

            renderScene.mainRenderCamera.xPos = playerScript.playerX;
            renderScene.mainRenderCamera.yPos = playerScript.playerY;
            renderScene.mainRenderCamera.zPos = playerScript.playerZ;

            renderScene.mainRenderCamera.yaw = playerScript.playerYaw;
            renderScene.mainRenderCamera.pitch = playerScript.playerPitch;


        }
        protected override void OnClosing(CancelEventArgs e) {
            renderScene.EndThreads();

        }

    }
}
