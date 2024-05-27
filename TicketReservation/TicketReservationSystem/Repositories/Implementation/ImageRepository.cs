using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using TicketReservationSystem.Data;
using TicketReservationSystem.Repositories.Interface;

namespace TicketReservationSystem.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext carRentalDbContext;
        private readonly Account account;

        public ImageRepository(IConfiguration configuration, ApplicationDbContext carRentalDbContext)
        {
            this.configuration = configuration;
            this.carRentalDbContext = carRentalDbContext;
            this.carRentalDbContext = carRentalDbContext;
            account = new Account(
                configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]
                );
        }

        public async Task<List<string>> GetAllImagesAsync()
        {
            var client = new Cloudinary(account);

            var result = await client.ListResourcesAsync(new ListResourcesParams());

            if (result != null && result.Resources != null && result.Resources.Any())
            {
                return result.Resources.Select(resource => resource.SecureUrl.AbsoluteUri).ToList();
            }

            return new List<string>();
        }

        public async Task<string> UploadAsync(IFormFile file, string title)
        {
            var client = new Cloudinary(account);


            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = title
            };

            var uploadResult = await client.UploadAsync(uploadParams);

            if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUri.ToString();
            }
            return null;
        }


    }
}

