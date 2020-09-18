using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace MagicDart.Ioc
{
    /// <summary>
    /// Use to mark property as dependency
    /// </summary>
    [MeansImplicitUse]
    public class ServiceDependencyAttribute : Attribute
    {
        /// <summary>
        /// Return all Service dependencies of specified type
        /// </summary>
        /// <param name="t">Type to inspect</param>
        /// <returns>All services dependencies or empty list</returns>
        public static List<PropertyInfo> GetDependencyList(Type t)
        {
            return t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(item => item.IsDefined(typeof(ServiceDependencyAttribute), true))
                .Union(t.BaseType != null ? GetDependencyList(t.BaseType).ToArray() : new PropertyInfo[0])
                .ToList();
        }
    }
}