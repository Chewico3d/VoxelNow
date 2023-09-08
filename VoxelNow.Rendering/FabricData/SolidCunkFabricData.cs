﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNow.Core;

namespace VoxelNow.Rendering.FabricData {
    public class SolidCunkFabricData : IFabricData {

        int IFabricData.renderObjectID { get { return 0x01; } }

        public ushort[] voxelsIDs;

        public ushort GetVoxel(int x, int y, int z) {
            x++; y++; z++;
            int ID = x + y * (GenerationConstants.voxelSizeX + 2)
                + z * (GenerationConstants.voxelSizeX + 2) * (GenerationConstants.voxelSizeY + 2);
            return voxelsIDs[ID];

        }
        public void SetVoxel(int x, int y, int z , ushort value) {
            x++; y++; z++;
            int ID = x + y * (GenerationConstants.voxelSizeX + 2)
                + z * (GenerationConstants.voxelSizeX + 2) * (GenerationConstants.voxelSizeY + 2);

            voxelsIDs[ID] = value;
        }


    }
}
