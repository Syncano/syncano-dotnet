using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace Syncano4.Unity3d
{
    public static class Extensions
    {
        public static T GetCustomAttribute<T>(this PropertyInfo propertyInfo) where T:Attribute
        {
            return  (T) propertyInfo.GetCustomAttributes(typeof (T), false).FirstOrDefault();
        }
    }
}