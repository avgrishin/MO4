using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace MO.Hubs
{
  public class RHub : Hub
  {
    public void Hello()
    {
      Clients.Caller.hello();
    }
  }
}