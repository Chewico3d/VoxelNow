using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.API {
    public class MapArray2D<T> {

        int sizeX, sizeY;
        public T[] array;

        public MapArray2D(int sizeX, int sizeY) {
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            int arraySize = sizeX * sizeY;
            array = new T[arraySize];
        }

        public T GetValue(int x, int y) {
            int posID = x + y * sizeX;
            return array[posID];
        }

        public void SetValue(int x, int y, T value) {
            int posID = x + y * sizeX;
            array[posID] = value;
        }

        

    }
}
