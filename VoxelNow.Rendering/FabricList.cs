using System.Reflection;
using VoxelNow.Rendering.FabricData;
using VoxelNow.Rendering.Fabrics;
using VoxelNow.Rendering.ObjectsFabrics;
using VoxelNow.Rendering.RenderCollection;
using VoxelNow.Rendering.RenderObjects;

namespace VoxelNow.Rendering
{
    internal static class FabricList
    {

        internal static IObjectFabric[] fabrics;
        internal static IRenderObject[] renderObjects;
        internal static IRenderCollection[] collections;

        internal static void Load()
        {

            renderObjects = new IRenderObject[5];
            fabrics = new IObjectFabric[5];
            collections = new IRenderCollection[5];

            CheckType(typeof(SolidFabric));
            CheckType(typeof(FluidFabric));
            CheckType(typeof(FloatingFabric));
            CheckType(typeof(MapFabric));

            CheckType(typeof(SolidRenderObject));
            CheckType(typeof(FluidRenderObject));
            CheckType(typeof(FloatingRenderObject));
            CheckType(typeof(MapRenderObject));

            CheckType(typeof(SolidRenderCollection));
            CheckType(typeof(FluidRenderCollection));
            CheckType(typeof(FloatingRenderCollection));
            CheckType(typeof(MapRenderCollection));

            //Assembly currentAssembly = Assembly.GetExecutingAssembly();
            //Module[] modules = currentAssembly.GetLoadedModules();
            //
            //for (int y = 0; y < modules.Length; y++)
            //{
            //    Type[] fields = modules[y].GetTypes();
            //
            //    for (int z = 0; z < fields.Length; z++)
            //        CheckType(fields[z]);
            //
            //}

        }

        internal static void CheckType(Type t)
        {

            if (t.IsAssignableTo(typeof(IRenderObject))){
                IRenderObject renderObject = (IRenderObject)Activator.CreateInstance(t);
                renderObjects[renderObject.renderObjectID] = renderObject;

                return;
            }else if (t.IsAssignableTo(typeof(IObjectFabric))){
                IObjectFabric fabricData = (IObjectFabric)Activator.CreateInstance(t);
                fabrics[fabricData.renderObjectID] = fabricData;

                return;
            }else if (t.IsAssignableTo(typeof(IRenderCollection))){
                IRenderCollection collection = (IRenderCollection)Activator.CreateInstance(t);
                collections[collection.renderObjectID] = collection;

                return;
            }

        }

    }
}
