﻿using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.OpenApi.Models;
using T_Store.CustomValidations.FluentValidators;
using T_Store.Models;
using T_Strore.Business.Consumers;
using T_Strore.Business.Models;
using T_Strore.Business.Producers;
using T_Strore.Business.Services;
using T_Strore.Data.Repository;
using IncredibleBackendContracts.Constants;
using T_Strore.Business.Services.Interfaces;

namespace T_Store.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICalculationService, CalculationService>();
            services.AddScoped<IRateService, RateService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITransactionRepository, TransactionRepository>();
        }

        public static void AddProducers(this IServiceCollection services)
        {
            services.AddScoped<ITransactionProducer, TransactionProducer>();
        }

        public static void AddFluentValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation (config => config.DisableDataAnnotationsValidation = true);
            services.AddScoped<IValidator<TransactionRequest>, TransactionRequestValidator>();
            services.AddScoped<IValidator<TransactionTransferRequest>, TransactionTransferRequestValidator>();
        }

        public static void AddSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "T-Store",
                    Version = "v1"
                });
            });
        }

        public static void AddMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<RateConsumer>();
         
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.ReceiveEndpoint(/*RabbitEndpoint.CurrencyRates*/"test-queue", c =>
                    {
                        c.ConfigureConsumer<RateConsumer>(ctx);
                        
                    });

                    cfg.ReceiveEndpoint(RabbitEndpoint.TransactionCreate, c =>
                    {
                        c.Bind<TransactionModel>();
                    });
                });
            });
        }
    }
}
