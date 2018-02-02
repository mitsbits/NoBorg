using System.ComponentModel;

namespace Borg.Infra
{
    //TODO: this is in the wrong assembly
    public class PaginationInfoStyle : IPaginationInfoStyle
    {
        private string _ItemSliceAndTotalFormat = "{0} to {1} of {2}";
        private string _PageCountAndLocationFormat = "{0} of {1}";
        private string _NextDisplay = ">";
        private string _LastDisplay = ">>";
        private string _PreviousDisplay = "<";
        private string _FirstDisplay = "<<";
        private string _PageDisplayFormat = "{0}";
        private string _PageVariableName = "p";
        private string _ElementClass = "pagination";
        private string _CurrentClass = "active";
        private string _UnavailableClass = "disabled";
        private string _ArrowClass = "";
        private string _Ellipses = "...";
        private string _OutputTagElement = "ul";
        private string _OutputItemTagElement = "li";

        [DefaultValue("ul")]
        public virtual string OutputTagElement
        {
            get { return _OutputTagElement; }
            set { _OutputTagElement = value; }
        }

        [DefaultValue("li")]
        public virtual string OutputItemTagElement
        {
            get { return _OutputItemTagElement; }
            set { _OutputItemTagElement = value; }
        }

        [DefaultValue("{0} to {1} of {2}")]
        public virtual string ItemSliceAndTotalFormat
        {
            get { return _ItemSliceAndTotalFormat; }
            set { _ItemSliceAndTotalFormat = value; }
        }

        [DefaultValue("{0} of {1}")]
        public virtual string PageCountAndLocationFormat
        {
            get { return _PageCountAndLocationFormat; }
            set { _PageCountAndLocationFormat = value; }
        }

        [DefaultValue(">")]
        public virtual string NextDisplay
        {
            get { return _NextDisplay; }
            set { _NextDisplay = value; }
        }

        [DefaultValue(">>")]
        public virtual string LastDisplay
        {
            get { return _LastDisplay; }
            set { _LastDisplay = value; }
        }

        [DefaultValue("<")]
        public virtual string PreviousDisplay
        {
            get { return _PreviousDisplay; }
            set { _PreviousDisplay = value; }
        }

        [DefaultValue("<<")]
        public virtual string FirstDisplay
        {
            get { return _FirstDisplay; }
            set { _FirstDisplay = value; }
        }

        [DefaultValue("{0}")]
        public virtual string PageDisplayFormat
        {
            get { return _PageDisplayFormat; }
            set { _PageDisplayFormat = value; }
        }

        [DefaultValue("page")]
        public virtual string PageVariableName
        {
            get { return _PageVariableName; }
            set { _PageVariableName = value; }
        }

        [DefaultValue("pagination")]
        public virtual string ElementClass
        {
            get { return _ElementClass; }
            set { _ElementClass = value; }
        }

        [DefaultValue("current")]
        public virtual string CurrentClass
        {
            get { return _CurrentClass; }
            set { _CurrentClass = value; }
        }

        [DefaultValue("unavailable")]
        public virtual string UnavailableClass
        {
            get { return _UnavailableClass; }
            set { _UnavailableClass = value; }
        }

        [DefaultValue("arrow")]
        public virtual string ArrowClass
        {
            get { return _ArrowClass; }
            set { _ArrowClass = value; }
        }

        [DefaultValue("...")]
        public string Ellipses
        {
            get { return _Ellipses; }
            set { _Ellipses = value; }
        }
    }
}