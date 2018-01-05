using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Infra.DDD
{
    public interface ICanBeDeleted
    {
        void Delete();

        bool IsDeleted { get; }
    }
}
