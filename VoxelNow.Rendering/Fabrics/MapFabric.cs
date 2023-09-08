using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Rendering.FabricData;
using VoxelNow.Rendering.MeshData;

namespace VoxelNow.Rendering.Fabrics {
    internal class MapFabric : IObjectFabric {
        public ushort renderObjectID { get { return 0x02; } }

        public IMeshData GenerateMeshData(IFabricData fabricData) {
            MapFabricData mapData = (MapFabricData)fabricData;
            List<float> verticesPositions = new List<float>();
            List<byte> verticesColor = new List<byte>();
            List<uint> indices = new List<uint>();

            uint it = 0;
            for(int x = 0; x < mapData.SizeX; x++) {
                for(int y = 0; y < mapData.SizeY; y++) {

                    verticesPositions.Add((float)(x) / mapData.SizeX * 2 - 1) ;
                    verticesPositions.Add((float)(y) / mapData.SizeX * 2 - 1);
                    verticesPositions.Add((float)(x + 1) / mapData.SizeX * 2 - 1);
                    verticesPositions.Add((float)(y) / mapData.SizeX * 2 - 1);
                    verticesPositions.Add((float)(x) / mapData.SizeX * 2 - 1);
                    verticesPositions.Add((float)(y + 1) / mapData.SizeX * 2 - 1);
                    verticesPositions.Add((float)(x + 1) / mapData.SizeX * 2 - 1);
                    verticesPositions.Add((float)(y + 1) / mapData.SizeX * 2 - 1);

                    int ID = x * 3 + y * mapData.SizeX * 3;
                    for (int rep = 0; rep < 4; rep++) {
                        verticesColor.Add(mapData.Color[ID + 0]);
                        verticesColor.Add(mapData.Color[ID + 1]);
                        verticesColor.Add(mapData.Color[ID + 2]);
                    }

                    indices.Add(it * 4 + 0);
                    indices.Add(it * 4 + 2);
                    indices.Add(it * 4 + 1);

                    indices.Add(it * 4 + 2);
                    indices.Add(it * 4 + 3);
                    indices.Add(it * 4 + 1);

                    it++;
                }
            }


            return new MapMeshData() { colorData = verticesColor.ToArray(), vertexData =  verticesPositions.ToArray(), indices = indices.ToArray() };

        }
    }
}
