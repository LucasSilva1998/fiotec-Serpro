using Microsoft.OpenApi.Models;

namespace fiotec_Serpro.API.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Fiotec - SERPRO API - Consulta de CPF",
                    Version = "v1",
                    Description = "API responsável por consultar informações de CPF utilizando o serviço do SERPRO.",
                    Contact = new OpenApiContact
                    {
                        Name = "Lucas Pereira",
                        Email = "lucassilva@fiotec.fiocruz.com",
                        Url = new Uri("https://github.com/LucasSilva1998/fiotec-Serpro")
                    }
                });

            });

            return services;
        }
    }
}
