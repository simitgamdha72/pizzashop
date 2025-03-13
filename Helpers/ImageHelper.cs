using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace pizzashop.Helpers
{
    public class ImageHelper
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _imagesFolder = "images/profiles";

        public ImageHelper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            // Validate file extension
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (fileExtension != ".jpg" && fileExtension != ".jpeg")
            {
                throw new ArgumentException("Only JPG files are allowed.");
            }

            // Create a unique filename
            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

            // Create folder if it doesn't exist
            var uploadPath = Path.Combine(_environment.WebRootPath, _imagesFolder);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Save the file
            var filePath = Path.Combine(uploadPath, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }
    }
}