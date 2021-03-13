using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebChat.Damain.Interfaces;
using WeChat.SocketsManager.Extentions;

namespace WeChat.SocketsManager
{
    public static class SocketsExtentions
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<IConnectionManager, ConnectionManager>();

            foreach(var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(SocketHandlers))
                    services.AddSingleton(type);
            }

            return services;
        }

        public static IApplicationBuilder MapSokets(this IApplicationBuilder application,PathString path, SocketHandlers socket)
        {
            return application.Map(path, (x) => x.UseMiddleware<SocketMiddleware>(socket));
        }
    }
}
