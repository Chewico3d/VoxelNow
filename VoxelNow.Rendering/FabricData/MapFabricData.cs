using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Rendering.FabricData {
    public class MapFabricData : IFabricData {

        public MapFabricData(int sizeX, int sizeY) {

            this.SizeX = sizeX;
            this.SizeY = sizeY;

            Color = new byte[sizeX * sizeY * 3];

        }


        public int renderObjectID { get { return 0x02; } }

        public int SizeX, SizeY;
        public byte[] Color;
        
        public void SetColor(int x, int y, byte r, byte g, byte b) {
            int Id = x + y * SizeX;
            Id *= 3;

            Color[Id] = r;
            Color[Id + 1] = g;
            Color[Id + 2] = b;

        }

    }
}
