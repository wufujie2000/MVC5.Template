As the name implies, it's a project starter template for ASP.NET MVC based solutions.

This project will use the latest technologies and will try to incorporate the best ASP.NET MVC practices.

# Installation
1. Before opening project rename it using "Rename Project.exe".
2. Build project or restore NuGet packages.
3. Open "Package Manager Console" and run "update-database" command on "Data" project.
4. Install necessary VS extensions if you don't have them already:
  - NUnit Tests Adapter.
  - Web Essentials 201X.
  - If you are using VS2012, change T4Scaffolding.Core package version to 1.0.0.
  - If you are using VS2013, download [Windows Management Framework 4.0](http://www.microsoft.com/en-us/download/details.aspx?id=40855). Which is needed for VS2013 scaffolding to work.

# Features
- [Model-View-ViewModel](http://en.wikipedia.org/wiki/Model_View_ViewModel) architectural design.
- Latest technologies and frameworks.
- Lowercase or normal ASP.NET urls.
- Protection from CSRF, XSS, etc.
- Custom membership providers.
- [Basic module scaffolding](https://github.com/NonFactors/MVC.Template/wiki/Scaffolding).
- Easy project renaming.
- Dependency injection.
- Custom error pages.
- Globalization.
- Audit log.
- Site map.
- Tests.

# Frameworks used
- [ASP.NET MVC](http://www.asp.net/mvc/) - main framework.
- [Bootstrap](http://getbootstrap.com/) - design.
- [JQuery](http://jquery.com/) - client side javascript.
- [JQuery.UI](http://jqueryui.com/) - client side javascript.
- [LightInject](http://www.lightinject.net/) - dependency injector.
- [AutoMapper](http://automapper.org/) - model<->view object mapper.
- [T4Scaffolding](https://www.nuget.org/packages/T4Scaffolding.Core/) - program module scaffolder.
- [GridMvc](http://gridmvc.codeplex.com/) - grid controls.
- [Datalist](http://non-factors.com/Datalist/) - data list controls.
- [NSubstitute](http://nsubstitute.github.io/) - mocking framework.
- [NUnit](http://www.nunit.org/) - testing framework.
