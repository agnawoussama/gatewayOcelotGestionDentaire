using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Charger la configuration Ocelot � partir du fichier ocelot.json
builder.Configuration.AddJsonFile("ocelot.json");

// Ajouter les services n�cessaires pour Ocelot
builder.Services.AddOcelot();

// Ajouter une politique CORS pour permettre les requ�tes provenant d'Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("http://localhost:4200")  // Permettre les requ�tes venant de votre application Angular
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Utiliser la politique CORS
app.UseCors("CorsPolicy");

app.UseWebSockets();

// Utiliser Ocelot pour g�rer les requ�tes de la passerelle API
app.UseOcelot().Wait();


app.Run();