using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Bootstrapper;
using Nancy.Demo.RavenDB.Raven;
using Nancy.TinyIoc;
using Raven.Client;
using Raven.Client.Embedded;

namespace Nancy.Demo.RavenDB
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var store = new EmbeddableDocumentStore()
            {
                ConnectionStringName = "RavenDB"
            };

            store.Initialize();

            container.Register<IDocumentStore>(store);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            var store = container.Resolve<IDocumentStore>();
            var documentSession = store.OpenSession();

            container.Register<IDocumentSession>(documentSession);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            pipelines.AfterRequest.AddItemToEndOfPipeline(
                (ctx) =>
                {
                    var documentSession = container.Resolve<IDocumentSession>();

                    if (ctx.Response.StatusCode != HttpStatusCode.InternalServerError)
                    {
                        documentSession.SaveChanges();
                    }

                    documentSession.Dispose();
                });
        }
    }
}