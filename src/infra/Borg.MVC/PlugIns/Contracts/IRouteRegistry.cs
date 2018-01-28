using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Routing;

namespace Borg.MVC.PlugIns.Contracts
{
   public interface IRouteRegistry
   {
       void Register(IRouteBuilder builder);
   }
}
