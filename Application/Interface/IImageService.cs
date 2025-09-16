using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<byte[]> DownloadImageAsync(string imageUrl);
        Task<bool> DeleteImageAsync(string imageUrl);
        Task<string> GetImageUrlAsync(Guid imageId);
    }
}
