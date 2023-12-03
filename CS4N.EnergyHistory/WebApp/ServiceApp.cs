using CS4N.EnergyHistory.Core;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace CS4N.EnergyHistory.WebApp
{
  public class ServiceApp : IHostedService
  {
    public ServiceApp()
    {
      // Die Web-Server-Implementierung Kestrel wird als Default verwendet
      Builder = WebApplication.CreateBuilder(new WebApplicationOptions
      {
        EnvironmentName = EnvironmentHelper.IsDebug ? "Development" : "Production",
        // Gebe explizit den Namen der Assembly/Anwendung als Quelle für die WebApplication an, damit in deren
        // öffentlichen Klassen nach Controllern gesucht wird, welche via REST veröffentlicht werden
        ApplicationName = Assembly.GetExecutingAssembly().FullName,
        // Führ den externen Aufruf (bspw. aus einem WindowsService) muss der RootPath explizit angegeben werden
        ContentRootPath = AppContext.BaseDirectory
      });
    }

    public WebApplicationBuilder Builder { get; init; }
    public WebApplication? WebApp { get; private set; }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      ConfigureServices(Builder.Services);
      ConfigureWebHost(Builder.WebHost);

      // Generiere App
      WebApp = Builder.Build();

      ConfigureApplication(WebApp);

      return WebApp.RunAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      if (WebApp == null)
        return Task.CompletedTask;

      return WebApp.StopAsync(cancellationToken);
    }

    static void ConfigureServices(IServiceCollection services)
    {
      // Erweitert die Server-Implementierung um die Nutzung von Controller-Klassen für die API
      services.AddControllers();
    }

    public static void ConfigureApplication(WebApplication app)
    {
      // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-6.0
      // Mit UseDefaultFiles() wird der Website-Request gegen das Standard-Verzeichnis wwwroot geleitet und dort wird gesucht nach:
      // default.htm
      // default.html
      // index.htm
      // index.html
      app.UseDefaultFiles(new DefaultFilesOptions
      {
        // Abweichenden Pfad für Default-Files bekannt machen!
        FileProvider = new PhysicalFileProvider(PathHelper.GetRootPath()),
      });

      // Zusätzliche Dateitypen & -inhalte müssen explizit erlaubt werden
      var provider = new FileExtensionContentTypeProvider();
      provider.Mappings[".properties"] = "text/html";

      // UseDefaultFiles() muss immer VOR UseStaticFiles() genutzt werdem
      app.UseStaticFiles(new StaticFileOptions
      {
        ContentTypeProvider = provider,
        FileProvider = new PhysicalFileProvider(PathHelper.GetRootPath())
      });

      // Verwendung der Authentifizierung und(!) Authorisierung beginnen
      app.UseAuthentication();
      app.UseAuthorization();

      // Mache alle Controller im Projekt für die API bekannt
      app.MapControllers();
    }

    static void ConfigureWebHost(ConfigureWebHostBuilder webHost)
    {
      webHost.ConfigureKestrel(options =>
      {
        options.ListenAnyIP(5678);
      });
    }
  }
}