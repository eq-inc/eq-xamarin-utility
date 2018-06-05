using System;
using System.Collections.Generic;

namespace Eq.Utility
{
    public static class DependencyServiceManager{
        private static IDependencyService sInstance = null;

        public static IDependencyService Instance
        {
            get
            {
                if(sInstance == null)
                {
                    LogController.Log(LogController.LogCategoryMethodError, "create instance");
#if __UNIT_TEST__
                    sInstance = new UnitTestDependencyService();
#else
                    sInstance = new LocalDependencyService();
#endif
                }

                return sInstance;
            }
        }
    }

    internal class LocalDependencyService : IDependencyService
    {
        public T Get<T>() where T : class
        {
            return Xamarin.Forms.DependencyService.Get<T>();
        }

        public void Register<T>(object impl) where T : class
        {
            Xamarin.Forms.DependencyService.Register<T>();
        }
    }

    internal class UnitTestDependencyService : IDependencyService
    {
        private readonly Dictionary<Type, object> mServiceDic = new Dictionary<Type, object>();

        public T Get<T>() where T : class
        {
            return (T)mServiceDic[typeof(T)];
        }

        void IDependencyService.Register<T>(object impl)
        {
            mServiceDic[typeof(T)] = impl;
        }
    }

    public interface IDependencyService
    {
        T Get<T>() where T : class;
        void Register<T>(object impl) where T : class;
    }
}
