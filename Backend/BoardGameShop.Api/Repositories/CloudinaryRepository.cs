using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGameShop.Api.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace BoardGameShop.Api.Repositories
{
    public class CloudinaryRepository : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryRepository(IConfiguration config)
        {
            var account = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null.");

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };
            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                throw new Exception(result.Error.Message);

            return result.SecureUrl.ToString();
        }
    }
}