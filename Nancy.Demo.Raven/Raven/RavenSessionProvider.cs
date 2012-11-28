using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace Nancy.Demo.RavenDB.Raven
{
    public class RavenSessionProvider
    {
        private static EmbeddableDocumentStore _documentStore;

        public bool SessionInitialized { get; set; }

        public static EmbeddableDocumentStore DocumentStore
        {
            get { return (_documentStore ?? (_documentStore = CreateDocumentStore())); }
        }

        private static EmbeddableDocumentStore CreateDocumentStore()
        {
            var store = new EmbeddableDocumentStore()
            {
                ConnectionStringName = "RavenDB"
            };

            store.Initialize();

            return store;
        }

        public IDocumentSession GetSession()
        {
            SessionInitialized = true;

            var session = DocumentStore.OpenSession();
            return session;
        }
    }
}