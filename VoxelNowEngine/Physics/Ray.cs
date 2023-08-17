using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using VoxelNowEngine.Terrain;

namespace VoxelNowEngine.Phyisics {
    public static class Ray {

        static Vector3i CalculatingChunkID;
        static Chunk CalculatingChunk;

        public static float RayCast(RayInfo rayInfo) {
            rayInfo.rayDirection.Normalize();

            Vector3 axisDistance = Vector3.One / rayInfo.rayDirection;
            for (int x = 0; x < 3; x++)
                if (rayInfo.rayDirection[x] == 0)
                    axisDistance[x] = float.MaxValue;
            Vector3i axisDirection = new Vector3i((axisDistance.X > 0) ? 1 : -1, (axisDistance.Y > 0) ? 1 : -1, (axisDistance.Z > 0) ? 1 : -1);

            axisDistance *= axisDirection;

            Vector3i rayIDPos = new Vector3i((int)MathF.Floor(rayInfo.rayOrigin.X), (int)MathF.Floor(rayInfo.rayOrigin.Y), (int)MathF.Floor(rayInfo.rayOrigin.Z));

            float RayDistance = 0;

            Vector3 distancePerAxis = axisDistance;
            for (int x = 0; x < 3; x++) {
                distancePerAxis[x] *= MathF.Floor(rayInfo.rayOrigin[x]) - rayInfo.rayOrigin[x];
            }

            for (int z = 0; z < 10000; z++) {

                if (RayDistance >= rayInfo.rayMaxDistance)
                    break;

                RayDistance = float.MaxValue;
                for (int x = 0; x < 3; x++)
                    RayDistance = MathF.Min(RayDistance, distancePerAxis[x]);


                for (int x = 0; x < 3; x++) {
                    if (distancePerAxis[x] == RayDistance) {
                        distancePerAxis[x] += axisDistance[x];
                        rayIDPos[x] += axisDirection[x];
                    }

                }

                Vector3i rayChunkIDPos = new Vector3i((int)MathF.Floor((float)rayIDPos.X / 16), (int)MathF.Floor((float)rayIDPos.Y / 256), (int)MathF.Floor((float)rayIDPos.Z / 16));

                if (CalculatingChunkID != rayChunkIDPos || CalculatingChunk == null) {
                    CalculatingChunkID = rayChunkIDPos;
                    CalculatingChunk = ChunkWorld.GetChunk(rayChunkIDPos.X, rayChunkIDPos.Y, rayChunkIDPos.Z);

                    if (CalculatingChunk == null)
                        return RayDistance;
                }

                byte BlockID = CalculatingChunk.GetBlock(rayIDPos.X - rayChunkIDPos.X * 16, rayIDPos.Y - rayChunkIDPos.Y * 256, rayIDPos.Z - rayChunkIDPos.Z * 16);

                if (BlockID != 0)
                    return RayDistance;
            }


            return rayInfo.rayMaxDistance;

        }
        public static Vector3i RayCastCollision(RayInfo rayInfo) {
            rayInfo.rayDirection.Normalize();

            Vector3 axisDistance = Vector3.One / rayInfo.rayDirection;
            for (int x = 0; x < 3; x++)
                if (rayInfo.rayDirection[x] == 0)
                    axisDistance[x] = float.MaxValue;
            Vector3i axisDirection = new Vector3i((axisDistance.X > 0) ? 1 : -1, (axisDistance.Y > 0) ? 1 : -1, (axisDistance.Z > 0) ? 1 : -1);

            axisDistance *= axisDirection;

            Vector3i stepIDPos = new Vector3i((int)MathF.Floor(rayInfo.rayOrigin.X), (int)MathF.Floor(rayInfo.rayOrigin.Y), (int)MathF.Floor(rayInfo.rayOrigin.Z));

            Vector3 step = axisDistance;
            for (int x = 0; x < 3; x++) {
                if (axisDirection[x] < 0)
                    step[x] *= rayInfo.rayOrigin[x] - MathF.Floor(rayInfo.rayOrigin[x]);
                else
                    step[x] *= 1 - rayInfo.rayOrigin[x] + MathF.Floor(rayInfo.rayOrigin[x]);
            }

            float RayDistance = 0;
            for (int z = 0; z < 10000; z++) {

                if (RayDistance >= rayInfo.rayMaxDistance)
                    break;

                RayDistance = float.MaxValue;
                for (int x = 0; x < 3; x++)
                    RayDistance = MathF.Min(RayDistance, step[x]);


                for (int x = 0; x < 3; x++) {
                    if (step[x] == RayDistance) {
                        step[x] += axisDistance[x];
                        stepIDPos[x] += axisDirection[x];
                    }

                }

                Vector3i rayChunkIDPos = new Vector3i((int)MathF.Floor((float)stepIDPos.X / 16), (int)MathF.Floor((float)stepIDPos.Y / 256), (int)MathF.Floor((float)stepIDPos.Z / 16));

                if (CalculatingChunkID != rayChunkIDPos || CalculatingChunk == null) {
                    CalculatingChunkID = rayChunkIDPos;
                    CalculatingChunk = ChunkWorld.GetChunk(rayChunkIDPos.X, rayChunkIDPos.Y, rayChunkIDPos.Z);

                    if (CalculatingChunk == null)
                        return stepIDPos;
                }

                byte BlockID = CalculatingChunk.GetBlock(stepIDPos.X - rayChunkIDPos.X * 16, stepIDPos.Y - rayChunkIDPos.Y * 256, stepIDPos.Z - rayChunkIDPos.Z * 16);

                if (BlockID != 0)
                    return stepIDPos;
            }


            return new Vector3i(int.MaxValue, int.MaxValue, int.MaxValue);

        }

        public class RayInfo {
            public Vector3 rayOrigin;
            public Vector3 rayDirection;
            public float rayMaxDistance;
            
            public RayInfo(Vector3 origin, Vector3 Direction, float Distance = 10) {
                rayOrigin = origin;
                rayDirection = Direction;
                rayMaxDistance = Distance;
            }
        }
    }
}
