
namespace VoxelNow.Rendering.Textures {
    internal class TiledTexture : Texture {

        internal readonly int sizeX, sizeY;
        public TiledTexture(string path, int sizeX, int sizeY) : base(path) {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
        }
    }
}
