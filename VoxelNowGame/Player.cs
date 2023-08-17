using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNowGame {
    internal class Player : ScriptBehaviour{

        float sensibility = 1f;
        Vector2 lastPosition;
        Vector2 viewDirection;
        public float FOVadd = MathF.PI * 45f / 180f;

        public Vector3 playerPosition = new Vector3(0, 10, 0);

        public void SetInitialPos() {
            Vector2 mousePosition = Program.mainGame.MousePosition;
            lastPosition = mousePosition;
        }

        void ScriptBehaviour.Update(float deltaTime) {

            Vector2 mousePosition = Program.mainGame.MousePosition;

            float positionX = mousePosition.X;
            float positionY = mousePosition.Y;

            float difPosX = positionX - lastPosition.X;
            float difPosY = positionY - lastPosition.Y;

            viewDirection += new Vector2(difPosX, difPosY) * sensibility * deltaTime;

            lastPosition = mousePosition;

            Quaternion moverotation = Quaternion.FromEulerAngles(0, viewDirection.X, 0);
            Vector3 front = moverotation * Vector3.UnitZ * 8;
            Vector3 left = moverotation * Vector3.UnitX * 8;

            if (Program.mainGame.IsKeyDown(Keys.W))
                playerPosition += front * deltaTime;
            if(Program.mainGame.IsKeyDown(Keys.S))
                playerPosition += -front * deltaTime;

            if (Program.mainGame.IsKeyDown(Keys.D))
                playerPosition += left * deltaTime;
            if (Program.mainGame.IsKeyDown(Keys.A))
                playerPosition += -left * deltaTime;

            if (Program.mainGame.IsKeyDown(Keys.Space))
                playerPosition += Vector3.UnitY * deltaTime * 20;
            if (Program.mainGame.IsKeyDown(Keys.LeftControl))
                playerPosition -= Vector3.UnitY * deltaTime * 10;
            if (Program.mainGame.IsKeyDown(Keys.E))
                FOVadd += deltaTime * .2f;
            if (Program.mainGame.IsKeyDown(Keys.Q))
                FOVadd += deltaTime * -.2f;
        }

        internal Vector3 GetPlayerDirection() {

            Vector3 Direction = Vector3.Zero;
            float euler2rad = MathF.PI / 180;

            Direction.X = MathF.Sin(viewDirection.X) * MathF.Cos(-viewDirection.Y);
            Direction.Z = MathF.Cos(viewDirection.X) * MathF.Cos(-viewDirection.Y);

            Direction.Y = MathF.Sin(-viewDirection.Y);

            return Direction;

        
        } 
        internal Quaternion getCameraOrientation() {
            return Quaternion.FromEulerAngles(viewDirection.Y, viewDirection.X, 0);
            
        }

    }
}
