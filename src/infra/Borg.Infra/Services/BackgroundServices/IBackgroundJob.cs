using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Borg.Infra.Services.BackgroundServices
{
    public interface IBackgroundJob
    {
    }

    public interface IEnqueueJob: IBackgroundJob
    {
        Task Execute(string[] args);
    }


}
