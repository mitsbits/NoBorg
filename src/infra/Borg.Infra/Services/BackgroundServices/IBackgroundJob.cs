using System.Threading.Tasks;

namespace Borg.Infra.Services.BackgroundServices
{
    public interface IBackgroundJob
    {
    }

    public interface IEnqueueJob : IBackgroundJob
    {
        Task Execute(string[] args);
    }
}