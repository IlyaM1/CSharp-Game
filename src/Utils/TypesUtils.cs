using System;
using System.Linq;

namespace Dyhar.src.Utils
{
    public static class TypesUtils
    {
        public static object GetStaticFieldFromInstance<TObject>(TObject obj, string staticFieldName)
        {
            return obj.GetType().GetProperty(staticFieldName).GetValue(obj);
        }

        public static Type GetTypeFromString(string typeName)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .First(x => x.Name == typeName);
        }

        public static object CreateObject(Type type, string[] parameters)
        {
            var constructorInfo = type.GetConstructors().FirstOrDefault();
            var constructorParameters = constructorInfo.GetParameters();
            var arguments = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameterType = constructorParameters[i].ParameterType;
                arguments[i] = Convert.ChangeType(parameters[i], parameterType);
            }

            var obj = constructorInfo.Invoke(arguments);
            return obj;
        }        

        public static void EmptyFunction()
        {

        }
    }
}
