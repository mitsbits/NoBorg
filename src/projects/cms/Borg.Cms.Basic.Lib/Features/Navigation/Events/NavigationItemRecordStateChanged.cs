using Borg.Infra.DAL;
using MediatR;

namespace Borg.Cms.Basic.Lib.Features.Navigation.Events
{
    public class NavigationItemRecordStateChanged : INotification
    {
        public NavigationItemRecordStateChanged(int id, DmlOperation dmlOperation)
        {
            Id = id;
            DmlOperation = dmlOperation;
        }

        public int Id { get; }
        public DmlOperation DmlOperation { get; }
    }
}