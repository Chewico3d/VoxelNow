using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Core;

namespace VoxelNow.Core.TerrainTools
{
    public class PerlinNoiseMap2D
    {

        Random random = new Random(0);

        int voxelSizeX, voxelSizeY;
        int samplesInX;
        int samplesInY;

        int sampleSeparation;

        readonly MapArray2D<(float, float)> gridVectors;

        public PerlinNoiseMap2D(int voxelSizeX, int voxelSizeY, int sampleSeparation)
        {
            this.voxelSizeX = voxelSizeX;
            this.voxelSizeY = voxelSizeY;

            this.sampleSeparation = sampleSeparation;

            samplesInX = (int)MathF.Ceiling((float)voxelSizeX / sampleSeparation);
            samplesInY = (int)MathF.Ceiling((float)voxelSizeY / sampleSeparation);

            gridVectors = new MapArray2D<(float, float)>(samplesInX + 1, samplesInY + 1);


            GenerateVectors();
        }


        void GenerateVectors()
        {

            for (int x = 0; x < samplesInX + 1; x++)
            {
                for (int y = 0; y < samplesInY + 1; y++)
                {

                    float vectorX = random.NextSingle() * 2 - 1;
                    float vectorY = random.NextSingle() * 2 - 1;

                    float magnitude = MathF.Sqrt(vectorX * vectorX + vectorY * vectorY);

                    gridVectors.SetValue(x, y, (vectorX , vectorY ));

                }
            }

        }
        /*
        2------3
        |      |
        |      |
        0------1
         */

        float SmothValue(float value)
        {

            return -(MathF.Cos(MathF.PI * value) - 1) / 2;
        }

        public float GetValue(float xPos, float yPos)
        {

            float relativeX = xPos / sampleSeparation;
            float relativeY = yPos / sampleSeparation;
            int IDPosX = (int)MathF.Floor(relativeX);
            int IDPosY = (int)MathF.Floor(relativeY);

            IDPosX = IDPosX < 0 ? 0 : IDPosX >= samplesInX - 1 ? samplesInX - 2 : IDPosX;
            IDPosY = IDPosY < 0 ? 0 : IDPosY >= samplesInY - 1 ? samplesInY - 2 : IDPosY;

            (float, float) vector0 = gridVectors.GetValue(IDPosX, IDPosY);
            (float, float) vector1 = gridVectors.GetValue(IDPosX + 1, IDPosY);
            (float, float) vector2 = gridVectors.GetValue(IDPosX, IDPosY + 1);
            (float, float) vector3 = gridVectors.GetValue(IDPosX + 1, IDPosY + 1);

            float localX = SmothValue(relativeX - IDPosX);
            float localY = SmothValue(relativeY - IDPosY);

            float vertexValue0 = vector0.Item1 * localX + vector0.Item2 * localY;
            float vertexValue1 = vector1.Item1 * (localX - 1) + vector1.Item2 * localY;
            float vertexValue2 = vector2.Item1 * localX + vector2.Item2 * (localY - 1);
            float vertexValue3 = vector3.Item1 * (localX - 1) + vector3.Item2 * (localY - 1);

            float valueDownX = vertexValue0 * (1 - localX) + vertexValue1 * localX;
            float valueUpX = vertexValue2 * (1 - localX) + vertexValue3 * localX;

            return valueUpX * localY + valueDownX * (1 - localY);

        }

    }
}
