namespace DragonSouvenirs.Services.Data.Common
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class AppCloudinary
    {
        public static async Task<string> UploadImage(Cloudinary cloudinary, IFormFile image, string name)
        {
            byte[] destinationImage;
            await using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                destinationImage = memoryStream.ToArray();
            }

            await using (var memoryStream = new MemoryStream(destinationImage))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(name, memoryStream),
                    PublicId = name,
                };

                var uploadResult = cloudinary.Upload(uploadParams);
#pragma warning disable CS0618 // Type or member is obsolete
                return uploadResult.SecureUri.AbsoluteUri;
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        public static void DeleteImage(Cloudinary cloudinary, string name)
        {
            var deleteParams = new DelResParams()
            {
                PublicIds = new List<string>() { name },
                Invalidate = true,
            };

            cloudinary.DeleteResources(deleteParams);
        }
    }
}
