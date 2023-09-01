using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Client {
    internal class playerScript {

        public float playerX, playerY, playerZ = -1;
        public float playerYaw, playerPitch;

        float lastMousePosX = 1280 / 2;
        float lastMousePosY = 720 / 2;

        public void Update() {

            float currentMousePosX = Program.nativeWindow.MousePosition.X;
            float currentMousePosY = Program.nativeWindow.MousePosition.Y;

            float differenceY = currentMousePosY - lastMousePosY;
            float differenceX = currentMousePosX - lastMousePosX;

            playerPitch += differenceY * 0.006f;
            playerYaw += differenceX * 0.006f;

            float frontAxisZ = MathF.Cos(playerYaw);
            float frontAxisX = MathF.Sin(playerYaw);

            if (Program.nativeWindow.IsKeyDown(Keys.W)) {
                playerZ += frontAxisZ * 0.016f;
                playerX += frontAxisX * 0.016f;
            }
            if (Program.nativeWindow.IsKeyDown(Keys.S)) {
                playerZ -= frontAxisZ * 0.016f;
                playerX -= frontAxisX * 0.016f;
            }

            if (Program.nativeWindow.IsKeyDown(Keys.D)) {
                playerX += frontAxisZ * 0.016f;
                playerZ += frontAxisX * -0.016f;
            }
            if (Program.nativeWindow.IsKeyDown(Keys.A)) {
                playerX -= frontAxisZ * 0.016f;
                playerZ -= frontAxisX * -0.016f;
            }
            if (Program.nativeWindow.IsKeyDown(Keys.Space)) {
                playerY += 0.03f;
            }
            if (Program.nativeWindow.IsKeyDown(Keys.LeftShift)) {
                playerY -= 0.03f;
            }

            Program.nativeWindow.MousePosition = new OpenTK.Mathematics.Vector2(1280 / 2, 720 / 2);
            lastMousePosX = 1280 / 2;
            lastMousePosY = 720 / 2;
        }
    }
}
