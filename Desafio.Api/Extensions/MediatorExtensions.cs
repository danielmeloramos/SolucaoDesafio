using Autofac;
using MediatR;
using MediatR.Pipeline;
using Desafio.Application;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace DesafioPrimeiro.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class MediatorExtensions
    {
        public static void AddMediator(this ContainerBuilder containerBuilder)
        {
            // Baseado em: https://github.com/jbogard/MediatR/blob/master/samples/MediatR.Examples.Autofac/Program.cs#L22

            containerBuilder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                containerBuilder
                    .RegisterAssemblyTypes(GetAssemblies().ToArray())
                    .AsClosedTypesOf(mediatrOpenType)
                    // when having a single class implementing several handler types
                    // this call will cause a handler to be called twice
                    // in general you should try to avoid having a class implementing for instance `IRequestHandler<,>` and `INotificationHandler<>`
                    // the other option would be to remove this call
                    // see also https://github.com/jbogard/MediatR/issues/462
                    .AsImplementedInterfaces();
            }

            containerBuilder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            containerBuilder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            containerBuilder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            containerBuilder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            containerBuilder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(IMediator).GetTypeInfo().Assembly;
            yield return typeof(AppModule).GetTypeInfo().Assembly;
        }

    }
}
