using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.Contracts
{
  public  interface IEntityRegistry
  {
      void RegisterWithDbContext(ModelBuilder builder);
  }
}
