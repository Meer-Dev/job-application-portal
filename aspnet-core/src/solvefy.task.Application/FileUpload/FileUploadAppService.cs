using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;

namespace solvefy.task.FileUpload
{
    public class FileUploadAppService : ApplicationService
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadAppService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new UserFriendlyException("No file uploaded");

            var allowedExtensions = new[] { ".pdf", ".docx", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (Array.IndexOf(allowedExtensions, fileExtension) == -1)
                throw new UserFriendlyException("Invalid file type. Only PDF, DOCX, JPG, and PNG files are allowed.");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/uploads/" + uniqueFileName;
        }
    }
}