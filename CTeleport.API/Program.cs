using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using CTeleport.Application;
using CTeleport.Infrastructure;
using CTeleport.Localization;
using CTeleport.API.Helper;
using System.Reflection;
using Serilog;
using CTeleport.Infrastructure.Models;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

string ApiCorsPolicy = "_apiCorsPolicy";

builder.Services.AddControllers();

builder.Services.AddCors(options => options.AddPolicy(ApiCorsPolicy, builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CTeleport API",
        Description = "An ASP.NET Core Web API for calculating distance between two airports",
        TermsOfService = new Uri("https://cteleport.com/privacy-policy"),
        Contact = new OpenApiContact
        {
            Name = "Contact",
            Url = new Uri("https://cteleport.com/contact")
        }
    });

    c.OperationFilter<AcceptLanguageHeaderParameter>();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});

builder.Services.RegisterApplication();

builder.Services.RegisterInfrastructure();

builder.Services.RegisterLocalization();

builder.Services.Configure<AirportApiSettings>(builder.Configuration.GetSection("AirportApi"));

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CTeleport");
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(ApiCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.RegisterApplicationMiddleware();

app.ConfigureLocalization(new CultureProvider());

app.Run();