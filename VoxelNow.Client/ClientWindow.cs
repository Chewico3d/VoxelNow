using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using VoxelNow.Core;
using VoxelNow.Rendering;
using VoxelNow.Rendering.FabricData;
using VoxelNow.Rendering.RenderObjects;

namespace VoxelNow.Client {
    internal class ClientWindow : GameWindow{
        internal ClientWindow() : base(new GameWindowSettings() { UpdateFrequency = 70}, new NativeWindowSettings() { Size = new OpenTK.Mathematics.Vector2i(1280, 720) }) { }
        internal FloatingRenderObject fRO = new FloatingRenderObject();

        Scene scene = new Scene();
        playerScript playerScript = new playerScript();



        protected override void OnLoad() {

            VoxelNowAssetsDatabase.LoadDatabaseAssembly();
            
            SolidCunkFabricData solidChunkData = new SolidCunkFabricData();
            solidChunkData.voxelsIDs = new ushort[34 * 34 * 34];

            for(int x = 0; x < 32; x++) {
                for(int z = 0; z < 32; z++) {
                    for(int y = 0; y < 4; y++) {

                        if(y == 3)
                            solidChunkData.SetVoxel(x, y, z, 2);
                        else
                            solidChunkData.SetVoxel(x, y, z, 1);
                    }
                }
            }

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.Enable(EnableCap.DepthTest);

            scene.Initialize();
            scene.GenerateRenderObject(solidChunkData);
            Program.nativeWindow = this;
            Program.nativeWindow.MousePosition = new OpenTK.Mathematics.Vector2(1280 / 2, 720 / 2);
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            scene.RenderScene();
            SwapBuffers();
            scene.LoadRenderObject();
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {

            playerScript.Update();

            scene.mainRenderCamera.xPos = playerScript.playerX;
            scene.mainRenderCamera.yPos = playerScript.playerY;
            scene.mainRenderCamera.zPos = playerScript.playerZ;

            scene.mainRenderCamera.yaw = playerScript.playerYaw;
            scene.mainRenderCamera.pitch = playerScript.playerPitch;


        }

    }
}
