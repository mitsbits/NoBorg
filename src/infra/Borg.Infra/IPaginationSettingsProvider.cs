using System;

namespace Borg.Infra
{
    public interface IPaginationSettingsProvider
    {
        IPaginationInfoStyle Style { get; }
    }



    public class NullPaginationSettingsProvider : IPaginationSettingsProvider
    {
        private static readonly IPaginationInfoStyle _paginationInfoStyle;
        static NullPaginationSettingsProvider()
        {
            _paginationInfoStyle = new PaginationInfoStyle();
        }

        public IPaginationInfoStyle Style => _paginationInfoStyle;
    }

    public class FactoryPaginationSettingsProvider<TSettings> : IPaginationSettingsProvider where TSettings : IPaginationInfoStyle
    {
        private  readonly TSettings _paginationInfoStyle;
        public FactoryPaginationSettingsProvider(Func<TSettings> factory)
        {
            _paginationInfoStyle = factory.Invoke();
        }

        public IPaginationInfoStyle Style => _paginationInfoStyle;
    }

    public class InstancePaginationSettingsProvider<TSettings> : IPaginationSettingsProvider where TSettings : IPaginationInfoStyle
    {
        private readonly TSettings _paginationInfoStyle;
        public InstancePaginationSettingsProvider(TSettings instance)
        {
            _paginationInfoStyle = instance;
        }

        public IPaginationInfoStyle Style => _paginationInfoStyle;
    }
}