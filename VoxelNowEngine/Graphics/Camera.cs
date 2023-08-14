using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNowEngine.Graphics {
    public class Camera {

        public Vector3 position;
        public Quaternion rotation = Quaternion.Identity;

        public float FOV = MathF.PI * 45f / 180f ;
        public float Start = .01f;
        public float End = 1000f;

        public Matrix4 GetCameraMatrix() {

            Matrix4 Projection = Matrix4.CreatePerspectiveFieldOfView(FOV, 16f / 9f, Start, End);
            Matrix4 rotationP = Matrix4.CreateFromQuaternion(rotation);
            Matrix4 World = Matrix4.CreateTranslation(-position.X, -position.Y, position.Z);
            return World * rotationP * Projection;

        }

    }
}
