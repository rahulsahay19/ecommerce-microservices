using Microsoft.AspNetCore.Authentication;
using Ocelot.APIGateway.Handlers;

namespace Ocelot.APIGateway
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using DependencyInjection;
    using Middleware;
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot().AddDelegatingHandler<FakeHandler>(true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseOcelot(GetOcelotConfiguration()).GetAwaiter().GetResult();
        }

        private static OcelotPipelineConfiguration GetOcelotConfiguration()
            => new OcelotPipelineConfiguration
            {
                AuthenticationMiddleware = async (context, next) =>
                {
                    if (!context.DownstreamReRoute.IsAuthenticated)
                    {
                        await next.Invoke();
                        return;
                    }

                    //if (context.HttpContext.RequestServices.GetRequiredService<IAnonymousRouteValidator>()
                    //    .HasAccess(context.HttpContext.Request.Path))
                    //{
                    //    await next.Invoke();
                    //    return;
                    //}

                    var authenticateResult = await context.HttpContext.AuthenticateAsync();
                    if (authenticateResult.Succeeded)
                    {
                        context.HttpContext.User = authenticateResult.Principal;
                        await next.Invoke();
                        return;
                    }

                    context.Errors.Add(new UnauthenticatedError("Unauthenticated"));
                }
            };
    }
}
