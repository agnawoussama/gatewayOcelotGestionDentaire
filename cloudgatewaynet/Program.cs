using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Charger la configuration Ocelot à partir du fichier ocelot.json
builder.Configuration.AddJsonFile("ocelot.json");

// Ajouter les services nécessaires pour Ocelot
builder.Services.AddOcelot();

// Ajouter une politique CORS pour permettre les requêtes provenant d'Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("http://localhost:4200")  // Permettre les requêtes venant de votre application Angular
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Utiliser la politique CORS
app.UseCors("CorsPolicy");

app.UseWebSockets();

// Utiliser Ocelot pour gérer les requêtes de la passerelle API
app.UseOcelot().Wait();


app.Run();