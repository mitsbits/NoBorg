using Borg.Infra.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timesheets.Web.Domain;

namespace Timesheets.Web.Features.Taxonomies.Services
{
    public interface ITaxonomyService
    {
        Task<IEnumerable<Tiding>> FlatTree();

        Task<Tidings> Tree();

        Task<Tidings> Tree(IEnumerable<Taxonomy> items);

        Task Invalidate();
    }

    public static class ITaxonomyServiceExtensions
    {
        //TODO: implement
        //public static async Task<Tidings> AvailableParents(this ITaxonomyService service, Guid taxonomyId)
        //{
        //    var collection = await service.Tree();
        //}
    }
}