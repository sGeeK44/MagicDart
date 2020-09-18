using System;
using System.Collections.Generic;

namespace MagicDart.Ioc
{
    /// <summary>
    /// Used to hold all available dependencies
    /// </summary>
    public class Catalog
    {
        private readonly Dictionary<Type, object> _itemList = new Dictionary<Type,object>();

        /// <summary>
        /// Indicate if specified type of dependency is avaible in catalogue
        /// </summary>
        /// <param name="serviceType">dependency to search</param>
        /// <returns>True if dependency exist, false else</returns>
        public bool IsAlreadyExist(Type serviceType)
        {
            return _itemList.ContainsKey(serviceType);
        }

        /// <summary>
        /// Add, update or remove specified dependency in current catalog
        /// </summary>
        /// <typeparam name="TRegisterAs">Typeof dependency used to get it back after. Generaly an abstract type</typeparam>
        /// <param name="instance">Instance of dependency</param>
        public void Add<TRegisterAs>(TRegisterAs instance)
            where TRegisterAs : class
        {
            Add<TRegisterAs, TRegisterAs>(instance);
        }

        /// <summary>
        /// Add, update or remove specified dependency in current catalog
        /// </summary>
        /// <typeparam name="TRegisterAs">Typeof dependency used to get it back after. Generaly an abstract type</typeparam>
        /// <typeparam name="TConcrete">Typeof concrete denpency, should implement or inherit TRegisterAs.</typeparam>
        /// <param name="instance">Instance of dependency</param>
        public void Add<TRegisterAs, TConcrete>(TConcrete instance)
            where TRegisterAs : class
            where TConcrete : TRegisterAs
        {
            var serviceType = typeof(TRegisterAs);
            var isServiceExist = IsAlreadyExist(serviceType);
            if (isServiceExist)
            {
                if (instance == null)
                    _itemList.Remove(serviceType);
                else
                    _itemList[serviceType] = instance;
            }
            else if (instance != null)
                _itemList.Add(serviceType, instance);
        }

        /// <summary>
        /// Remove specified dependency in current catalog
        /// </summary>
        /// <typeparam name="TRegisterAs">Typeof dependency to Remove.</typeparam>
        /// <returns>Dependencies removed, null else.</returns>
        public TRegisterAs Remove<TRegisterAs>() where TRegisterAs : class
        {
            var itemToRemove = Get<TRegisterAs>();
            if (itemToRemove == null)
                return null;

            return _itemList.Remove(typeof(TRegisterAs)) ? itemToRemove : null;
        }

        /// <summary>
        /// Get specified denpency type requested
        /// </summary>
        /// <typeparam name="TService">Type of dependency requested</typeparam>
        /// <returns>Depency found if exist, else null</returns>
        public TService Get<TService>()
            where TService : class
        {
            return (TService)Get(typeof(TService));
        }

        /// <summary>
        /// Get specified denpency type requested
        /// </summary>
        /// <param name="type">Type of dependency requested</param>
        /// <returns>Depency found if exist, else null</returns>
        public object Get(Type type)
        {
            var result = IsAlreadyExist(type) ? _itemList[type] : null;
            if(result != null)
                Dependencies.Inject(result);

            return result;
        }

        /// <summary>
        /// Remove all registered services.
        /// </summary>
        public void Clear()
        {
            _itemList.Clear();
        }
    }
}