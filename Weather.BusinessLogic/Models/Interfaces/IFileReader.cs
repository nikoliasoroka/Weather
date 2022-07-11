namespace Weather.BusinessLogic.Models.Interfaces
{
    public interface IFileReader
    {
        void Populate(string id, string type);
    }
}
