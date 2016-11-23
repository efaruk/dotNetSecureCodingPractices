using System;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using SCP.Configuration;
using SCP.Data;
using SCP.Diagnostics.Logging;
using SCP.Security;
using SCP.Security.Cryptography;

namespace SCP.Web
{
    public static class DependencyBuilder
    {
        private static IContainer _container;
        private static Log4NetLogger _logger;

        public static void Build()
        {
            if (_container != null) throw new InvalidOperationException("Dependency Container already initialized.");
            // Create Logger
            _logger = new Log4NetLogger();
            _logger.Info(">> Building Container");
            var builder = new ContainerBuilder();

            //Register Global Dependencies
            builder.RegisterInstance(_logger).As<ILogger>().SingleInstance();
            builder.RegisterType<UtcDateTimeProvider>().As<IDateTimeProvider>().SingleInstance();
            builder.RegisterType<AppConfigSettingsProvider>().As<ISettingsProvider>().SingleInstance();
            builder.RegisterType<DefaultPasswordHashProvider>().As<IPasswordHashProvidcer>().SingleInstance();
            builder.RegisterType<DefaultEncryptionProvider>().As<IEncryptionProvider>().SingleInstance();
            builder.RegisterType<DefaultRandomNumberGenerator>().As<IRandomNumberGenerator>().SingleInstance();

            // Running
            //TODO: We should PREVENT direct access to DbContext
            builder.RegisterType<ScpDbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<ScpUnitOfWork>().As<IScpUnitOfWork>().InstancePerRequest();

            // Autofac Integration
            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            //// OPTIONAL: Enable property injection in view pages.
            //builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.
            _container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
            _logger.Info("<< Building Container");
        }
    }
}