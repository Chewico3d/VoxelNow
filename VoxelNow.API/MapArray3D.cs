using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Core {
    public class MapArray3D<T> {


        public readonly int sizeX, sizeY, sizeZ;
        public readonly T[] array;

        public MapArray3D(int sizeX, int sizeY, int sizeZ) {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;

            int arraySize = sizeX * sizeY * sizeZ;
            array = new T[arraySize];
        }

        public T GetValue(int x, int y, int z) {
            int posID = x + y * sizeX + z * sizeX * sizeY;
            return array[posID];
        }

        public void SetValue(int x, int y, int z, T value) {
            int posID = x + y * sizeX + z * sizeX * sizeY;
            array[posID] = value;
        }

    }
}
