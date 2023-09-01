

using OpenTK.Windowing.Desktop;

namespace VoxelNow.Client {
    internal static class Program {

        internal static ClientWindow clientWindow;
        internal static NativeWindow nativeWindow;

        internal static void Main(string[] args) {
            clientWindow = new ClientWindow();
            clientWindow.Run();

        }
    }

}
