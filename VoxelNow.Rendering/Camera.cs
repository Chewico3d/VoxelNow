using OpenTK.Mathematics;

namespace VoxelNow.Server {
    public class Camera {

        public float xPos, yPos, zPos;
        public float FOV = 45f / 180f * 3.1415f;
        public float near = 0.01f, far = 1000f;

        public float pitch, yaw, roll;

        public float aspectRatio = 16f / 9;

        public float flatDirX, flatDirZ;
        Matrix4 cameraMatrix;

        public void SetPosition(float x, float y, float z) {
            xPos = x; yPos = y; zPos = z;
        }

        public void CalculateCameraMatrix() {

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(FOV, aspectRatio, near, far);
            Matrix4 cameraPosition = Matrix4.CreateTranslation(-xPos, -yPos, zPos);
            Matrix4 cameraRotationZ = Matrix4.CreateRotationZ(roll);
            Matrix4 cameraRotationY = Matrix4.CreateRotationY(yaw);
            Matrix4 cameraRotationX = Matrix4.CreateRotationX(pitch);

            cameraMatrix = cameraPosition * cameraRotationZ * cameraRotationY * cameraRotationX * projection;

            flatDirX = MathF.Sin(yaw);
            flatDirZ = MathF.Cos(yaw);


        }

        public Matrix4 GetMatrixForObject(float objX, float objY, float objZ) {
            Matrix4 objectTranslation = Matrix4.CreateTranslation(objX, objY, -objZ);
            Matrix4 invertZ = Matrix4.CreateScale(1, 1, -1);

            return invertZ * objectTranslation * cameraMatrix;

        }

    }
}
