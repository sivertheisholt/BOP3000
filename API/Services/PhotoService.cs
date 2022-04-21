using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using API.Interfaces.IServices;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config, IConfiguration envConfig)
        {
            var acc = new Account { };
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env == "Development")
            {
                acc = new Account
                (
                    config.Value.CLOUDINARY_SETTINGS_CLOUDNAME,
                    config.Value.CLOUDINARY_SETTINGS_API_KEY,
                    config.Value.CLOUDINARY_SETTINGS_API_SECRET
                );
            }
            else
            {
                acc = new Account
                (
                    Environment.GetEnvironmentVariable("CLOUDINARY_SETTINGS_CLOUDNAME"),
                    Environment.GetEnvironmentVariable("CLOUDINARY_SETTINGS_API_KEY"),
                    Environment.GetEnvironmentVariable("CLOUDINARY_SETTINGS_API_SECRET")
                );
            }

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }

        public async Task<List<string>> GetBackgroundPictures()
        {
            var something = _cloudinary.Search();

            var data = await something.Expression("folder:AccountBackgrounds/*").ExecuteAsync();

            var queryResult = data.JsonObj["resources"].Select(x => x["url"]);

            return queryResult.Values<string>().ToList();
        }
    }
}