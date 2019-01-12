using Autofac;
using Autofac.Integration.Mvc;
using WorkOfFiction.Helpers;
using WorkOfFiction.Services;

namespace WorkOfFiction.App_Start
{
    public static class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<GenreService>().AsSelf().InstancePerDependency();
            builder.RegisterType<OracleHelper>().AsSelf().InstancePerRequest();

            var container = builder.Build();


        
        }
    }
}