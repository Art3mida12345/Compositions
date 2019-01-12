using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using WorkOfFiction.Helpers;
using WorkOfFiction.Services;

namespace WorkOfFiction
{
    public static class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<GenreService>().AsSelf().InstancePerDependency();
            builder.RegisterType<OracleHelper>().AsSelf().InstancePerDependency();
            builder.RegisterType<CountryService>().AsSelf().InstancePerDependency();
            builder.RegisterType<TypeService>().AsSelf().InstancePerDependency();
            builder.RegisterType<LanguageService>().AsSelf().InstancePerDependency();
            builder.RegisterType<AuthorService>().AsSelf().InstancePerDependency();
            builder.RegisterType<CompositionService>().AsSelf().InstancePerDependency();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}