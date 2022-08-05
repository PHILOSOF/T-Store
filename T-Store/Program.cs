using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Data.SqlClient;
using T_Store.MapperConfig;
using T_Store.Middleware;
using T_Strore.Business.Services;
using T_Strore.Business.Services.Interfaces;
using T_Strore.Data;
using T_Strore.Data.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(@"Server=.\SQLEXPRESS;Database=T-Store.DB;Trusted_Connection=True;"));

builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var result = new BadRequestObjectResult(context.ModelState);
            result.StatusCode = StatusCodes.Status422UnprocessableEntity;
            return result;
        };
        
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "T-Store", Version = "v1" 
    });         
});

builder.Services.AddScoped<ITransactionRepository, TransactionRepositories>();
builder.Services.AddScoped<ITransactionServices, TransactionServices>();
builder.Services.AddScoped<ICalculationService, CalculationService>();

builder.Services.AddAutoMapper(typeof(MapperConfigStorage));


var app = builder.Build();

app.UseCustomExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();