using System;
using System.Linq;
using FluentValidation.HttpExtensions.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FluentValidation.HttpExtensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddFluentValidationHttpExtensions(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.Services
                .AddSingleton<HttpErrorPriorityProvider>()
                .AddWrapper<ProblemDetailsFactory>((serviceProvider, t) => new CustomProblemDetailsFactory(t, serviceProvider.GetRequiredService<HttpErrorPriorityProvider>()))
                .AddWrapper<IConfigureOptions<ApiBehaviorOptions>>((serviceProvider, t) => new CustomApiBehaviorOptionsSetup(t));
            return mvcBuilder;
        }

        private static IServiceCollection AddWrapper<T>(this IServiceCollection services,  Func<IServiceProvider, T, T> factory)
            where T: class
        {
            var coreResolver = services.Single(x => x.ServiceType == typeof(T));
            var coreImplementationType = coreResolver.ImplementationType;
            services.Add(new ServiceDescriptor(coreImplementationType, coreImplementationType, coreResolver.Lifetime));
            services.Add(new ServiceDescriptor(typeof(T), serviceProvider => factory(serviceProvider, (T)serviceProvider.GetRequiredService(coreImplementationType)), coreResolver.Lifetime));
            return services;
        }
    }
}
