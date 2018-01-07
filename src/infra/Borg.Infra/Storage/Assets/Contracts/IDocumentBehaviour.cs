using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Infra.Storage.Assets.Contracts
{
    public interface IDocumentBehaviour
    {
        IVersionInfo CheckOut();
        IVersionInfo Checkin(IVersionInfo edit);

        DocumentState DocumentState { get; }
    }

    public enum DocumentState
    {
        Commited = 1,
        InProgress = 2

    }
}
