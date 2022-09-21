using FluentValidation;
using FluentValidation.AspNetCore;
using IncredibleBackend.Messaging;
using IncredibleBackend.Messaging.Extentions;
using IncredibleBackend.Messaging.Interfaces;
using IncredibleBackendContracts.Constants;
using IncredibleBackendContracts.Events;
using IncredibleBackendContracts.Requests;
using MassTransit;
using Microsoft.OpenApi.Models;
using T_Store.CustomValidations.FluentValidators;
using T_Strore.Business.Consumers;
using T_Strore.Business.Producers;
using T_Strore.Business.Services;
using T_Strore.Business.Services.Interfaces;
using T_Strore.Data.Repository;

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
            services.AddScoped<IProcessorForProducer, ProcessorForProducer>();
            services.AddScoped<IMessageProducer, MessageProducer>();
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

        public static void ConfigureMessaging(this IServiceCollection services)
        {
            services.RegisterConsumersAndProducers((config) =>
            {
                config.AddConsumer<RateConsumer>();
            }, (cfg, ctx) =>
            {
                cfg.ReceiveEndpoint(RabbitEndpoint.CurrencyRates, c =>
                {
                    c.ConfigureConsumer<RateConsumer>(ctx);
                });
            }, (cfg) =>
            {
                cfg.RegisterProducer<TransactionCreatedEvent>(RabbitEndpoint.TransactionCreate);
                cfg.RegisterProducer<TransferTransactionCreatedEvent>(RabbitEndpoint.TransferTransactionCreate);
            });
        }
    }
}
