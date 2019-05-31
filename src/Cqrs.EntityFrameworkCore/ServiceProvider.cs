using System;

namespace Cqrs.EntityFrameworkCore
{
    internal static class ServiceProvider
    {
        private static object _mutex = new object();
        private static IServiceProvider _instance;

        public static void Initialize(IServiceProvider instance)
        {
            _instance = instance;
        }

        public static IServiceProvider Current
        {
            get
            {
                lock (_mutex)
                {
                    return _instance ?? throw new InvalidOperationException("Service provider was not initialized. Call Initialize method first");
                }
            }
        }
    }

    internal static class IServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }
    }
}
