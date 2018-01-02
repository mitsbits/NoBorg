using Borg.Cms.Basic.Lib.Features.Navigation.Services;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Navigation.Contracts
{
    public interface IMenuProvider
    {
        Task<MenuContainer> Tree(string @group);
    }
}