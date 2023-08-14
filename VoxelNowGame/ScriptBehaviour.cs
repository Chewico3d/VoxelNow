using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNowGame {
    public interface ScriptBehaviour {
        public void Update(float deltaTime) { }
        public void Render(float deltaTime) { }
    }
}
