using Application.Interface;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly AppDbContext _context;
        private readonly string _imageRootPath;
        public ImageService(AppDbContext context , string imageRootPath)
        {
            _context = context;
            _imageRootPath = imageRootPath;
        }
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null");

            // Ensure ProfilePicture directory exists
            string profilePictureDir = Path.Combine(_imageRootPath, "ProfilePicture");
            if (!Directory.Exists(profilePictureDir))
            {
                Directory.CreateDirectory(profilePictureDir);
            }

            // Generate unique filename
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(profilePictureDir, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path (API will serve this)
            return $"/images/ProfilePicture/{fileName}";
        }

            
        
        public Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            // Implementation for downloading an image
            throw new NotImplementedException();
        }
        public Task<bool> DeleteImageAsync(string imageUrl)
        {
            // Implementation for deleting an image
            throw new NotImplementedException();
        }
        public Task<string> GetImageUrlAsync(Guid imageId)
        {
            // Implementation for getting the URL of an image
            throw new NotImplementedException();
        }

        
    }
}
