using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Core;

namespace FoodAndDrink
{
    public class ApplicationEvents : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }
        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            AutofacDIRegister();            
        }

        private void AutofacDIRegister()
        {
            // Need to load all assemblies in the BIN folder as 3rd party plugins can have their own controllers
            var dlls = Directory.GetFiles(HostingEnvironment.MapPath("~/bin"), "*.dll");
            var assemblies = dlls.Select(x => Assembly.LoadFrom(Path.Combine(x))).ToArray();

            //TODO: for quickly, just register Type here, but if it grows up, should move it to a seperate Project
            var builder = new ContainerBuilder();

            // MVC controller registrations
            builder.RegisterControllers(assemblies);
            // API controller registrations
            builder.RegisterApiControllers(assemblies);
            
            var container = builder.Build();
            // MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //API
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}