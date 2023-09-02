using System;
using System.IO;
using System.Reflection;
using VoxelNow.API;

namespace VoxelNow.Core {
    public static class VoxelNowAssetsDatabase {

        public static string voxelAssemblyDatabasePath = "AssetPack";
        public static IVoxelData[] voxelsData;

        public static void LoadDatabaseAssembly() {
            string[] assembliesPath = Directory.GetFiles(voxelAssemblyDatabasePath, "*.dll");
            voxelsData = new IVoxelData[1024];
            
            for(int it = 0; it < assembliesPath.Length; it++) {
                assembliesPath[it] = Path.GetFullPath(assembliesPath[it]);
                Assembly loadedAssembly = Assembly.LoadFile(assembliesPath[it]);

                Console.WriteLine("Loading assembly : " + loadedAssembly.FullName);

                Type[] assemblyTypes = loadedAssembly.GetTypes();

                foreach (Type type in assemblyTypes) {

                    if (!type.IsAssignableTo(typeof(IVoxelData)))
                        continue;

                    IVoxelData workingVoxel = (IVoxelData)Activator.CreateInstance(type);
                    Console.WriteLine(" -> Loading voxel : " + type.Name + " ID : " + workingVoxel.voxelID);
                    voxelsData[workingVoxel.voxelID] = workingVoxel;

                }

            }

        }
    }
}
