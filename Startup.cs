using System;
using Autofac;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.DependencyInjection.Autofac;

namespace MvcSample.Web {
    public class Startup {
        public IServiceProvider ConfigureServices(IServiceCollection services) {
            var builder1 = new ContainerBuilder();
            builder1.RegisterModule<TestModule>();
            builder1.Populate(services);

            var container1 = builder1.Build();

            var nonScopeOptions = container1.Resolve<IServiceProvider>()
                .GetService<Microsoft.Framework.OptionsModel.IOptions<RazorViewEngineOptions>>()
                .Options;

            Console.WriteLine("{0} Options", nonScopeOptions.ViewLocationExpanders.Count);
            Console.WriteLine("");

            var builder2 = new ContainerBuilder();
            builder2.Populate(services);

            var container2 = builder2.Build();

            var scope = container2.BeginLifetimeScope((inner) => {
                inner.RegisterModule<TestModule>();
            }).Resolve<IServiceProvider>();

            var scopeOptions = scope
                .GetService<Microsoft.Framework.OptionsModel.IOptions<RazorViewEngineOptions>>()
                .Options;

            Console.WriteLine("{0} Options", scopeOptions.ViewLocationExpanders.Count);


            return container1.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app) {
            app.UseMvc(routes => {
                routes.MapRoute(
                    "controllerActionRoute",
                    "{controller}/{action}",
                    new { controller = "Home", action = "Index" },
                    constraints: null,
                    dataTokens: new { NameSpace = "default" });
            });
        }
        

        private class TestModule : Autofac.Module {
            protected override void Load(ContainerBuilder builder) {
                Console.WriteLine("Registering Module");

                var services2 = new ServiceCollection();
                services2.AddMvc();
                services2.ConfigureRazorViewEngine(options => {
                    Console.WriteLine("Registering TestViewLocationExpander");
                    options.ViewLocationExpanders.Add(new TestViewLocationExpander());
                });

                builder.Populate(services2);
            }
        }
    }
}
