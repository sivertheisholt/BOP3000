using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace API.Interfaces.IServices
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<DeletionResult> DeletePhotoAsync(string publicId);

        Task<List<string>> GetBackgroundPictures();
    }
}