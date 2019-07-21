using Microsoft.Extensions.DependencyInjection;
using Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddRbacService(this IServiceCollection service)
        {
            return service.AddScoped(factory => new RbacService());
        }
    }
}
