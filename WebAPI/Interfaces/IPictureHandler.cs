namespace WebAPI.Interfaces
{
    public interface IPictureHandler
    {
        Task<bool> Save(IFormFile picture);

        Task<List<string>> GetAll();

        Task<bool> DeleteByName(string pictureName);

        Task<byte[]> GetByName(string pictureName);
    }
}
