using Weather.BusinessLogic.Models.Interfaces;

namespace Weather.BusinessLogic.Models
{
    public class Device : IFileReader
    {
        public string Id { get; set; }
        public string Type { get; set; }

        public void Populate(string id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}
