
namespace VoxelNow.Rendering.FabricData {
    public class FloatingFabricData : IFabricData {
        int IFabricData.renderObjectID {
            get { return 0x00; }
        }

        public List<float> v_Position = new List<float>();
        public List<int> indicies = new List<int>();

        public void AddIndex(int Index) {
            indicies.Add(Index);
        }

        public void AddVertexNormal(float xDir, float yDir, float zDir) {
            throw new NotImplementedException();
        }

        public void AddVertexTexture(float U, float V) {
            throw new NotImplementedException();
        }

        public void AddVertexPosition(float xPos, float yPos, float zPos) {
            v_Position.Add(xPos);
            v_Position.Add(yPos);
            v_Position.Add(zPos);
        }

    }
}
