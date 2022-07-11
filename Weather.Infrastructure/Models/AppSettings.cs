namespace Weather.Infrastructure.Models
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string CsvDelimiter { get; set; }
    }
}
