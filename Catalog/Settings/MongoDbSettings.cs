namespace Catalog.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }

        public string ConectionString 
        { 
            get
            {
                return $"mongodb://{Host}:{Port}";
            } 
        }
    }
}