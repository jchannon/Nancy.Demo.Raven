# Nancy.Demo.Raven

This is a demo for showing how you can implement [RavenDB](http://ravendb.net/) in your Nancy applications.

The Bootstrapper configures the UOW pattern and injects the IDocumentSession per request into your Nancy modules and saves the changes at the end of each request.
