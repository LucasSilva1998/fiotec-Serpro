using fiotec_Serpro.API.Extensions;
using fiotec_Serpro.Infra.Services.Interfaces;
using fiotec_Serpro.Infra.Services.Services;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger
builder.Services.AddSwaggerDocumentation();

// Configuração dos HttpClients
builder.Services.AddHttpClient("SerproApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Serpro:baseUrl"]);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient("SerproAuth", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Serpro:baseUrl"]);
});

// Registro do serviço
builder.Services.AddScoped<ISerproService, SerproService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fiotec - Serpro API - Consulta de CPF v1");
        c.RoutePrefix = string.Empty; // abre o Swagger direto na raiz
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
