using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Core {
    public static class AssetLoader {

        public static string assetPath = "AssetPack";

        public static string GetAssetPath(string internalPath) => Path.Combine(assetPath, internalPath);

    }
}
