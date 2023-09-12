
using VoxelNow.Rendering;



namespace VoxelNow.Rendering {
    internal interface IRenderCollection {
        public ushort renderObjectID { get; }
        internal void Draw(Camera camera, Shader shader);
        internal int AddObject(IRenderObject renderObject);
        internal IRenderObject GetObject(int objectID);
        internal void SetRenderObjectPosition(int objectID, float posX, float posY, float posZ);
    }
}
