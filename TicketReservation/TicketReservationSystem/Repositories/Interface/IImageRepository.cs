namespace TicketReservationSystem.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<string> UploadAsync(IFormFile file, string title);

        Task<List<string>> GetAllImagesAsync();
    }
}
