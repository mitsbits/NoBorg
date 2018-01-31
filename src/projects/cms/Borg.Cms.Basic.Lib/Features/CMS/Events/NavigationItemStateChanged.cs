using Borg.Infra.DAL;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.CMS.Events
{
    public class NavigationItemStateChanged : INotification
    {
        public NavigationItemStateChanged(int id, DmlOperation dmlOperation)
        {
            Id = id;
            DmlOperation = dmlOperation;
        }

        public int Id { get; }
        public DmlOperation DmlOperation { get; }
    }
}