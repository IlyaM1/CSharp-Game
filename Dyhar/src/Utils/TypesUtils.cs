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
    }
}
