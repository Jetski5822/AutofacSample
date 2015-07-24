using System;
using System.IO;
using Autofac;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.DependencyInjection.Autofac;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Runtime.Infrastructure;

namespace MvcSample.Web
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
                // Create the autofac container
            var builder = new ContainerBuilder();
            builder.RegisterModule<TestModule>();
            builder.Populate(services);

            var container = builder.Build();

            return container.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app)
        {
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "controllerActionRoute",
                    "{controller}/{action}",
                    new { controller = "Home", action = "Index" },
                    constraints: null,
                    dataTokens: new { NameSpace = "default" });
            });
        }

        private class TestModule : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                var services2 = new ServiceCollection();
                services2.AddMvc();
                services2.ConfigureRazorViewEngine(options =>
                {
                    options.ViewLocationExpanders.Add(new TestViewLocationExpander());
                });

                builder.Populate(services2);
            }
        }
    }
}
