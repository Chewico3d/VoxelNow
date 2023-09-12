using System.Reflection;

namespace VoxelNow.Rendering
{
    internal static class AssemblyListLoader
    {

        internal static IObjectFabric[] fabrics;
        internal static IRenderObject[] renderObjects;
        internal static IRenderCollection[] collections;

        internal static void ReadFromAssembly()
        {

            renderObjects = new IRenderObject[5];
            fabrics = new IObjectFabric[5];
            collections = new IRenderCollection[5];

            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Module[] modules = currentAssembly.GetLoadedModules();

            for (int y = 0; y < modules.Length; y++)
            {
                Type[] fields = modules[y].GetTypes();

                for (int z = 0; z < fields.Length; z++)
                {

                    CheckType(fields[z]);
                }

            }

            //Only for debug
#if false
            for (int x = 0; x < fabrics.Length; x++)
            {
                if (fabrics[x] == null)
                    continue;

                if (renderObjects[x] == null)
                    throw new Exception("no IRenderObject in " + fabrics[x].GetType().Name);

                if (collections[x] == null)
                    throw new Exception("no IRenderCollection in " + fabrics[x].GetType().Name);

            }

#endif

        }

        internal static void CheckType(Type t)
        {

            Type[] interfaces = t.GetInterfaces();

            if (interfaces.Length == 0)
                return;

            if (interfaces[0] == typeof(IRenderObject))
            {

                IRenderObject renderObject = (IRenderObject)Activator.CreateInstance(t);
                renderObjects[renderObject.renderObjectID] = renderObject;

                return;
            }
            if (interfaces[0] == typeof(IObjectFabric))
            {

                IObjectFabric fabricData = (IObjectFabric)Activator.CreateInstance(t);
                fabrics[fabricData.renderObjectID] = fabricData;

                return;
            }
            if (interfaces[0] == typeof(IRenderCollection))
            {

                IRenderCollection collection = (IRenderCollection)Activator.CreateInstance(t);
                collections[collection.renderObjectID] = collection;

                return;
            }

        }

    }
}
