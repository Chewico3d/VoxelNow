using VoxelNow.API;


namespace VoxelNow.AssemblyLoader {
    public static class VoxelAssets {

        static byte[] voxelType = new byte[1024];
        static byte[] lightResistance = new byte[1024];
        static byte[] renderingMode = new byte[1024];
        static bool[] isProcedural = new bool[1024];
        static ushort[] proceduralID = new ushort[1024];

        internal static void Initialize() {

            for(int it = 0; it < 1024; it++) {
                IVoxelData currentVoxel = AssetLoader.voxelsData[it];

                if(currentVoxel == null)
                    continue;

                voxelType[it] = (byte)currentVoxel.voxelType;

                if(currentVoxel.voxelType == VoxelType.TransparentVoxel)
                    lightResistance[it] = currentVoxel.lightResistence;

                if(currentVoxel.voxelType == VoxelType.SolidVoxel | currentVoxel.voxelType == VoxelType.TransparentVoxel)
                    renderingMode[it] = (byte)currentVoxel.renderingFaceMode;
                isProcedural[it] = currentVoxel.isProcedural;
                if (isProcedural[it])
                    proceduralID[it] = currentVoxel.proceduralObjectReference;

            }

        }

        public static bool IsSolid(ushort ID) {
            return voxelType[ID] == (byte)VoxelType.SolidVoxel;
        }

        public static bool IsVoxel(ushort ID) {
            return voxelType[ID] == (byte)VoxelType.SolidVoxel | voxelType[ID] == (byte)VoxelType.TransparentVoxel;
        }

        public static bool IsTransparent(ushort ID) {
            return voxelType[ID] == (byte)VoxelType.TransparentVoxel;
        }

        public static byte GetLightResistence(ushort ID) {
            return lightResistance[ID];
        }

        public static bool IsSolidRender(ushort ID) {
            return renderingMode[ID] == (byte)VoxelRenderingFaceMode.Static;
        }

        public static TextureCoord[] GetTextureCoord(ushort ID) {
            return AssetLoader.voxelsData[ID].textureCoordsFaces;
        }

        public static bool IsProcedural(ushort ID) {
            return isProcedural[ID];
        }

        public static ushort GetProceduralID(ushort ID) {
            return proceduralID[ID];
        }

        public static bool CanWaterPass(ushort ID) {
            if (IsSolid(ID))
                return false;
            return true;
        }
    }
}
