using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelNowEngine.Graphics;
using VoxelNowEngine.Graphics.Objects;
using VoxelNowEngine.Terrain;

namespace VoxelNowEngine {
    internal class WorldManager {


        internal bool RuningWorld = true;
        internal bool ActiveThread = false;


        public void StartThread() {
            ActiveThread = true;
            while (RuningWorld) {

                Game.chunkWorld.CreateNonExistingChunks();
                Thread.Sleep(1000 / 50);

            }
            ActiveThread = false;
        }



    }
}
