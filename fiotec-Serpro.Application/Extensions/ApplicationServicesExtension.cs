using fiotec_Serpro.Application.Interfaces;
using fiotec_Serpro.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiotec_Serpro.Application.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)

        {
            services.AddScoped<ISerproAppService, SerproAppService>();

            return services;
        }
    }
}