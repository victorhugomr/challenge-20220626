using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using challenge_20220626.Models;
using challenge_20220626.Services;

class Program{
    
    static void Main(string[] args){

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.Configure<ProductDatabaseSettings>
            (builder.Configuration.GetSection("OpenFoodFactsDatabase"));

        builder.Services.AddSingleton<ProductServices>();
        builder.Services.AddSingleton<ScrapingService>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()){
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
    
}