using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Demo.RavenDB.Model;
using Nancy.Demo.RavenDB.Raven;
using Raven.Client;

namespace Nancy.Demo.RavenDB.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(IDocumentSession documentSession)
        {
            Get["/"] = parameters =>
                           {
                              // var documentSession = ravenSessionProvider.GetSession();
                               var model = documentSession.Load<Person>(1);
                               
                               return View["home", model];

                           };
        }
    }
}