using System.IO;
using System.Threading.Tasks;
using API.Helper;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloud;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc=new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloud=new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadresult=new ImageUploadResult();
            if (file.Length>0)
            {
                using var stream=file.OpenReadStream();
                var uploadparams=new ImageUploadParams
                {
                    File=new FileDescription(file.FileName,stream),
                    Transformation=new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    
                };
            uploadresult=await _cloud.UploadAsync(uploadparams);
                
            }
            return uploadresult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
           var deleteparmas=new DeletionParams(publicId);
           var result=await _cloud.DestroyAsync(deleteparmas);
           return result;
        }
    }
}