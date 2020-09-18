using System;
using System.Collections.Generic;
using System.Reflection;

// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertPropertyToExpressionBody

namespace MagicDart.Ioc
{
    /// <summary>
    /// Expose methods to realise DIP with reflection
    /// </summary>
    public static class Dependencies
    {
        private static readonly Catalog ServiceCatalog = new Catalog();
        private static readonly Dictionary<Type, List<PropertyInfo>> Cache= new Dictionary<Type, List<PropertyInfo>>();
        private static readonly object CacheLocker = new object();

        public static void Add<TRegisterAs>(TRegisterAs instance)
            where TRegisterAs : class
        {
            ServiceCatalog.Add<TRegisterAs, TRegisterAs>(instance);
        }

        /// <summary>
        /// Add, update or remove specified dependency in current catalog
        /// </summary>
        /// <typeparam name="TRegisterAs">Typeof dependency used to get it back after. Generaly an abstract type</typeparam>
        /// <typeparam name="TConcrete">Typeof concrete denpency, should implement or inherit TRegisterAs.</typeparam>
        /// <param name="instance">Instance of dependency</param>
        public static void Add<TRegisterAs, TConcrete>(TConcrete instance)
            where TRegisterAs : class
            where TConcrete : TRegisterAs
        {
            ServiceCatalog.Add<TRegisterAs, TConcrete>(instance);
        }

        /// <summary>
        /// Remove specified dependency in current catalog
        /// </summary>
        /// <typeparam name="TRegisterAs">Typeof dependency to Remove.</typeparam>
        /// <returns>Dependencies removed, null else.</returns>
        public static TRegisterAs Remove<TRegisterAs>() where TRegisterAs : class
        {
            return ServiceCatalog.Remove<TRegisterAs>();
        }

        /// <summary>
        /// Get specified denpency type requested
        /// </summary>
        /// <typeparam name="TService">Type of dependency requested</typeparam>
        /// <returns>Depency found if exist, else null</returns>
        public static TService Get<TService>()
            where TService : class
        {
            return (TService)ServiceCatalog.Get(typeof(TService));
        }

        /// <summary>
        /// Inject dependencies by reflect specified object
        /// </summary>
        /// <param name="objectToCompose">Object in which dependencies should resolved</param>
        public static void Inject(object objectToCompose)
        {
            if (objectToCompose == null)
                return;

            var propertyToInject = GetFromCache(objectToCompose);
            foreach (var propToInject in propertyToInject)
            {
                var serviceToInject = ServiceCatalog.Get(propToInject.PropertyType);

                if (!propToInject.CanWrite)
                    throw new NotSupportedException(
                        $"You have flags property as ServiceDependency, but setter isn't accessible. Property involve: {propToInject.Name} ({propToInject.PropertyType}).");

                if (serviceToInject != null)
                    propToInject.SetValue(objectToCompose, serviceToInject, null);
            }
        }

        private static List<PropertyInfo> GetFromCache(object objectToCompose)
        {
            var objectTypeToCompose = objectToCompose.GetType();

            lock (CacheLocker)
            {
                if (Cache.ContainsKey(objectTypeToCompose))
                    return Cache[objectTypeToCompose];

                var propertyToInject = ServiceDependencyAttribute.GetDependencyList(objectTypeToCompose);
                Cache.Add(objectTypeToCompose, propertyToInject);
                return propertyToInject;
            }
        }
    }
}
