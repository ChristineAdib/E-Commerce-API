using Core.Repositories;
using E_CommerceAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Repository.Data;
using Repository;
using System.Runtime.CompilerServices;
using E_CommerceAPI.Errors;
using Core;
using Core.Services;
using Services;
using Stripe;

namespace E_CommerceAPI.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            Services.AddScoped<IPaymentService, PaymentService>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped(typeof(ICartRepo), typeof(CartRepo));
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddAutoMapper(typeof(MappingProfile));

            //handling validation error
            Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToArray();
                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });

            return Services;
        }
    }
}
