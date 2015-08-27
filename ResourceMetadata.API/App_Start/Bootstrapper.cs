using Autofac;
using Microsoft.AspNet.Identity;
using ResourceMetadata.API.Mappers;
using ResourceMetadata.Data.Infrastructure;
using ResourceMetadata.Data.Repositories;
using ResourceMetadata.Model;
using ResourceMetadata.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity.EntityFramework;
using ResourceMetadata.Data;
using ResourceMetadata.API.Controllers;
using System.Reflection;

namespace ResourceMetadata.API.App_Start
{
    public static class Bootstrapper
    {
        public static void Configure()
        {
            ConfigureAutofacContainer();
            AutoMapperConfiguration.Configure();
        }

        public static void ConfigureAutofacContainer()
        {

            var webApiContainerBuilder = new ContainerBuilder();
            ConfigureWebApiContainer(webApiContainerBuilder);
        }

        public static void ConfigureWebApiContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().AsImplementedInterfaces().InstancePerApiRequest();
            containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().AsImplementedInterfaces().InstancePerApiRequest();
            containerBuilder.RegisterType<WebWorkContext>().As<IWorkContext>().AsImplementedInterfaces().InstancePerApiRequest();
            containerBuilder.RegisterAssemblyTypes(typeof(UserService).Assembly).Where(t => t.Name.EndsWith("Service")).AsSelf().AsImplementedInterfaces().InstancePerApiRequest();
            containerBuilder.RegisterAssemblyTypes(typeof(UserRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerApiRequest();

            containerBuilder.Register(c => new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ResourceManagerEntities())
            {
                /*Avoids UserStore invoking SaveChanges on every actions.*/
                //AutoSaveChanges = false
            })).As<UserManager<ApplicationUser>>().InstancePerApiRequest();

            containerBuilder.RegisterApiControllers(System.Reflection.Assembly.GetExecutingAssembly());
            containerBuilder.RegisterApiControllers(typeof(AccountController).Assembly);
            IContainer container = containerBuilder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

    }
}