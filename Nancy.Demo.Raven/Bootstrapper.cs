using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Bootstrapper;
using Nancy.Demo.RavenDB.Raven;
using Nancy.TinyIoc;
using Raven.Client;

namespace Nancy.Demo.RavenDB
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register<RavenSessionProvider>().AsSingleton();

            base.ConfigureRequestContainer(container, context);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            pipelines.AfterRequest.AddItemToEndOfPipeline(
                (ctx) =>
                {
                    var documentSessionProvider = container.Resolve<RavenSessionProvider>();

                    if (!documentSessionProvider.SessionInitialized)
                    {
                        return;
                    }

                    var documentSession = documentSessionProvider.GetSession();

                    if (ctx.Response.StatusCode != HttpStatusCode.InternalServerError)
                    {
                        documentSession.SaveChanges();
                    }

                    documentSession.Dispose();
                });
        }
    }
}