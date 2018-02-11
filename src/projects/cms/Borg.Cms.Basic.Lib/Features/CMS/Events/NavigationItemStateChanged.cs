using Borg.Infra.DAL;
using Borg.Infra.Messaging;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.CMS.Events
{
    public class NavigationItemStateChanged : MessageBase, INotification
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