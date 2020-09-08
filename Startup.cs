using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MO.Startup))]
namespace MO
{

  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      app.MapSignalR();
    }
  }
}