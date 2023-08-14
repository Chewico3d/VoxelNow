using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using VoxelNowEngine.Graphics;
using VoxelNowEngine.Terrain;

namespace VoxelNowEngine {
    public class Game : GameWindow {

        internal static World currentWorld;

        static WorldManager worldManager;
        internal static RenderMaster currentRenderMaster;

        Thread WorldManagerThreead;

        public Game(World initialWorld) : base(new GameWindowSettings()  , new NativeWindowSettings() { Size = new OpenTK.Mathematics.Vector2i(1920,1080)}) {
            GL.ClearColor(.7f, .8f, .9f, 1f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Cw);

            currentWorld = initialWorld;

        }

        protected override void OnLoad() {
            this.CursorState = CursorState.Grabbed;
            SetWorld(currentWorld);
        }

        internal void SetWorld(World world) {

            if (WorldManagerThreead != null)
                WorldManagerThreead.Abort();

            currentRenderMaster = new RenderMaster();

            currentWorld = world;
            currentWorld.InitWorld();

            worldManager = new WorldManager();
            WorldManagerThreead = new Thread(worldManager.StartThread);
            WorldManagerThreead.Start();
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            currentWorld.Render((float)args.Time);

            if (currentWorld.properties.GenerateChunks)
                currentRenderMaster.RenderChunks(currentWorld.mainChunkMaterial, currentWorld.mainRenderCamera);

            SwapBuffers();
            worldManager.LoadDataInMainThread();

        }

        protected override void OnUpdateFrame(FrameEventArgs args) {
            currentWorld.Update((float)args.Time);

        }

    }
}
