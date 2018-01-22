using Borg.Cms.Basic.Lib.Features.Navigation.Contracts;
using Borg.Cms.Basic.Lib.Features.Navigation.Queries;
using Borg.Infra.DTO;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Navigation.Services
{
    public class MenuProvider : IMenuProvider
    {
        private readonly IMediator _dispatcher;

        public MenuProvider(IMediator dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task<MenuContainer> Tree(string @group)
        {
            var result = await _dispatcher.Send(new MenuGroupRecordsRequest(@group, true));
            if (!result.Succeded)
            {
                //TODO: log
                return new MenuContainer() { Group = @group, Trees = new Tidings() };
            }
            var set = result.Payload; //TODO: over engineered
            //var lookup = set.Trees().Flatten();
            //var toRemove = (from l in set.OrderByDescending(x => x.ParentId)
            //        .ThenByDescending(x => x.Id)
            //        .ThenBy(x => x.IsPublished)
            //                let p = lookup.FirstOrDefault(x => x.Key == l.ParentId.ToString())
            //                let parentActive = p == null || bool.Parse(p.Flag)
            //                where !parentActive || !l.IsPublished
            //                select l.Id).ToList();
            //var filteredSet = set.Where(x => !toRemove.Contains(x.Id));
            return new MenuContainer() { Group = @group/*, Trees = filteredSet.Trees()*/ };
        }
    }
}