using Borg.Platform.EStore.Contracts;

namespace Borg.Platform.EStore.Data.Entities
{
    public class PriceState : Price<int>
    {
        public override int Id { get; set; }
    }
}