using Dyhar.src.Entities;

namespace Dyhar.src.Utils
{
    public static class TypesUtils
    {
        public static bool CanBeDownCasted<TBase, TChild>(TBase obj) 
            where TBase : class where TChild : class
        {
            var tryCast = obj as TChild;

            if (tryCast != null)
                return true;
            else
                return false;
        }

        public static object GetStaticFieldFromInstance<TObject>(TObject obj, string staticFieldName)
        {
            return obj.GetType().GetProperty(staticFieldName).GetValue(obj);
        }

        public static void EmptyFunction()
        {

        }
    }
}
