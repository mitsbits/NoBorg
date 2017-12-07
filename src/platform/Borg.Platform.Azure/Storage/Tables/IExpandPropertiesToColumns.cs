using Microsoft.WindowsAzure.Storage.Table;

namespace Borg.Platform.Azure.Storage.Tables
{
    public interface IExpandPropertiesToColumns
    {
        ITableEntity Expanded();
    }
}