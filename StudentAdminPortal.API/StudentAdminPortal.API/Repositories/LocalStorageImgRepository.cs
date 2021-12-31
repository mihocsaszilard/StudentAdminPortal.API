using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Repositories
{
    public class LocalStorageImgRepository : IImageRepository
    {
        public async Task<string> Upload(IFormFile file, string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Resources","Img", fileName);
            using Stream fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return GetServerRelativePath(filePath);
        }

        private string GetServerRelativePath(string fileName)
        {
            return Path.Combine(@"Resources/Img", fileName);
        }
    }
}
