namespace Borg.Platform.Azure.Cosmos
{
    public class DocDbConfig
    {
        public DocDbConfig()
        {
            string nc = "Not configured";
            Endpoint = nc;
            AuthKey = nc;
            Database = nc;
            Collection = string.Empty;
        }

        public string Endpoint { get; set; }
        public string AuthKey { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }
}