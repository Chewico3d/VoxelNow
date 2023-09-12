﻿using System.Reflection;
using VoxelNow.API;

namespace VoxelNow.AssemblyLoader {
    public static class AssetLoader {

        public static string assetPath = "AssetPack";
        public static IVoxelData[] voxelsData;
        public static IProceduralVoxel[] proceduralVoxels;
        public static IWorldGenerator worldGenerator;

        public static string GetAssetPath(string internalPath) => Path.Combine(assetPath, internalPath);
        public static string GetAssetPath() => assetPath;

        public static void LoadAssemblyData() {
            string[] assembliesPath = Directory.GetFiles(GetAssetPath(), "*.dll");
            voxelsData = new IVoxelData[1024];
            proceduralVoxels = new IProceduralVoxel[1024];

            for (int it = 0; it < assembliesPath.Length; it++) {
                assembliesPath[it] = Path.GetFullPath(assembliesPath[it]);
                Assembly loadedAssembly = Assembly.LoadFile(assembliesPath[it]);

                Console.WriteLine("Loading assembly : " + loadedAssembly.FullName);

                Type[] assemblyTypes = loadedAssembly.GetTypes();

                foreach (Type type in assemblyTypes) {

                    if (type.IsAssignableTo(typeof(IVoxelData))) {
                        IVoxelData workingVoxel = (IVoxelData)Activator.CreateInstance(type);
                        Console.WriteLine(" -> Loading voxel : " + type.Name + " ID : " + workingVoxel.voxelID);
                        voxelsData[workingVoxel.voxelID] = workingVoxel;

                        continue;
                    }

                    if (type.IsAssignableTo(typeof(IWorldGenerator))) {
                        if (worldGenerator != null)
                            Console.WriteLine(" X Overlaping world generator. Be carefull");
                        worldGenerator = (IWorldGenerator)Activator.CreateInstance(type);
                        Console.WriteLine(" -> Loaded world generator : " + worldGenerator.GetType().Name);
                        continue;

                    }

                }

            }

        }

    }
}
