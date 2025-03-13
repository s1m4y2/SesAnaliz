using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using SesAnalizAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Veritabaný Baðlantýsý
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS Desteði
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// API Controllerlarý
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger Yapýlandýrmasý
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Ses Analiz API", Version = "v1" });

    // IFormFile Desteði için Çözüm
    options.OperationFilter<FileUploadOperationFilter>();

    options.SchemaGeneratorOptions.SupportNonNullableReferenceTypes = true;

    options.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
});


var app = builder.Build();

// Middleware'ler
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.UseRouting();

app.Use(async (context, next) =>
{
    context.Response.Headers["Access-Control-Allow-Origin"] = "*";
    context.Response.Headers["Access-Control-Allow-Methods"] = "POST, GET, OPTIONS, DELETE";
    context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type";
    await next();
});

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering(); // Request içeriðini okumak için
    var requestBody = new StreamReader(context.Request.Body);
    var body = await requestBody.ReadToEndAsync();
    Console.WriteLine($"Gelen Ýstek: {context.Request.Method} {context.Request.Path}");
    Console.WriteLine($"Ýçerik: {body}");
    context.Request.Body.Position = 0; // Stream'i baþa sar
    await next();
});


// Genel Hata Yakalama (Unhandled Exceptions)
AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
{
    Console.WriteLine($"Unhandled Exception: {eventArgs.ExceptionObject}");
};

app.Run();

// Swagger için ekstra filtre
public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParams = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile));

        if (!fileParams.Any()) return;

        operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            [fileParams.First().Name] = new OpenApiSchema
                            {
                                Type = "string",
                                Format = "binary"
                            }
                        }
                    }
                }
            }
        };
    }
}
