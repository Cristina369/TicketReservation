using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TicketReservationSystem.Models.DTO;
using TicketReservationSystem.Repositories.Interface;

namespace TicketReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;
        private readonly IEventRepository eventRepository;

        public ImagesController(IImageRepository imageRepository, IEventRepository eventRepository)
        {
            this.imageRepository = imageRepository;
            this.eventRepository = eventRepository;
        }

        [HttpGet("GetAllImages")]
        public async Task<IActionResult> GetAllImages()
        {
            try
            {
                var images = await imageRepository.GetAllImagesAsync();

                var response = images.Select(image => new EventImageRequest
                {
                    imageUrl = image
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("UploadAsync")]
        public async Task<IActionResult> UploadAsync([FromForm] IFormFile file,[FromForm] string title)
        {
            try
            {
                Console.WriteLine("File Format API: " + file.GetType());
                ValidateFileUpload(file);

                var imageURL = await imageRepository.UploadAsync(file, title);

                if (imageURL == null)
                {
                    return Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);
                }

                return new JsonResult(new { link = imageURL });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }

    }
}
